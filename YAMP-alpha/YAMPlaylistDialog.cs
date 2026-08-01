//using KoenZomers.OneDrive.Api.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Schema;

namespace YAMP_alpha
{
    public partial class YAMPlaylistDialog : Form
    {
        static BindingSource PlaylistSource;
        private string _loadedPlaylist = "";
        private int CurrentColoredRowIndex;
        private string _activeFilter = string.Empty;
        private int _dragSourceRowIndex = -1;
        private bool _coreTrackChangedSubscribed = false;
        private ToolStripMenuItem _endBehaviorMenuItem;
        private ToolStripMenuItem _endBehaviorStopItem;
        private ToolStripMenuItem _endBehaviorRepeatCurrentItem;
        private ToolStripMenuItem _endBehaviorLoopPlaylistItem;
        private bool _diskPanelVisible = false;
        private bool _autoRefreshEnabled = false;
        private readonly HashSet<string> _watchedDirectories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, FileSystemWatcher> _watchers = new Dictionary<string, FileSystemWatcher>(StringComparer.OrdinalIgnoreCase);
        private readonly List<string> _pendingNewTracks = new List<string>();
        private readonly List<string> _pendingMissingTracks = new List<string>();
        private readonly object _diskSyncLock = new object();
        private Timer _diskRescanDebounceTimer;
        private bool _queuePanelVisible = false;
        public event EventHandler<TrackSelectedEventArgs> TrackSelected;

        public string LoadedPlaylist
        {
            get { return _loadedPlaylist; }
            set
            {
                if (_loadedPlaylist != value)
                {
                    _loadedPlaylist = value;
                    if (YAMPVars.LoadedPlaylist != value)
                    {
                        YAMPVars.LoadedPlaylist = value;
                    }
                    Text = "Playlist: " + new FileInfo(_loadedPlaylist).Name;
                }
            }
        }



        private void UpdatePlaylistCounters()
        {
            int totalTracks = YAMPVars.TrackList != null ? YAMPVars.TrackList.Count : 0;
            int queueCount = YAMPVars.PendingQueue != null ? YAMPVars.PendingQueue.Count : 0;

            int missingCount = 0;
            int unsupportedCount = 0;
            if (YAMPVars.TrackList != null)
            {
                foreach (TrackInfo track in YAMPVars.TrackList)
                {
                    if (track == null || string.IsNullOrWhiteSpace(track.Path) || File.Exists(track.Path) == false)
                    {
                        missingCount++;
                    }
                    else if (!AudioFileSupport.IsSupportedAudioFile(track.Path))
                    {
                        unsupportedCount++;
                    }
                }
            }

            lblTtlTracks.Text = string.Format("Tracks: {0} | Queue: {1} | Missing: {2} | Unsupported: {3}", totalTracks, queueCount, missingCount, unsupportedCount);
            RefreshQueuePanel();
        }

        private void InitializeQueuePanel()
        {
            _queueUpButton.Click += queueUpButton_Click;
            _queueDownButton.Click += queueDownButton_Click;
            _queueRemoveButton.Click += queueRemoveButton_Click;
            _queueClearButton.Click += queueClearButton_Click;

            _queueToggleButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _queueRefreshButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _diskToggleButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            LayoutBottomBarButtons();
            _queueToggleButton.Click += queueToggleButton_Click;
            _queueRefreshButton.Click += queueRefreshButton_Click;
            _diskToggleButton.Click += diskToggleButton_Click;
            PnlPlaylistBottomBar.SizeChanged += (s, e) =>
            {
                LayoutBottomBarButtons();
            };
            _queuePanel.BringToFront();

            InitializeDiskPanel();
        }

        private void InitializeDiskPanel()
        {
            _diskRescanButton.Click += diskRescanButton_Click;
            _diskApplyNewButton.Click += diskApplyNewButton_Click;
            _diskRemoveMissingButton.Click += diskRemoveMissingButton_Click;
            _diskAutoRefreshCheckBox.CheckedChanged += diskAutoRefreshCheckBox_CheckedChanged;
            _diskPanel.BringToFront();

            _diskRescanDebounceTimer = new Timer();
            _diskRescanDebounceTimer.Interval = 900;
            _diskRescanDebounceTimer.Tick += diskRescanDebounceTimer_Tick;
        }

        private void LayoutBottomBarButtons()
        {
            if (_queueToggleButton == null || PnlPlaylistBottomBar == null)
                return;

            _queueToggleButton.Left = Math.Max(0, PnlPlaylistBottomBar.Width - _queueToggleButton.Width - 6);

            if (_queueRefreshButton != null)
            {
                _queueRefreshButton.Left = Math.Max(0, _queueToggleButton.Left - _queueRefreshButton.Width - 6);
            }

            if (_diskToggleButton != null)
            {
                _diskToggleButton.Left = Math.Max(0, _queueRefreshButton.Left - _diskToggleButton.Width - 6);
            }
        }

        private void queueToggleButton_Click(object sender, EventArgs e)
        {
            if (!_queuePanelVisible)
            {
                SetDiskPanelVisible(false);
            }

            _queuePanelVisible = !_queuePanelVisible;
            _queuePanel.Visible = _queuePanelVisible;
            _queueToggleButton.Text = _queuePanelVisible ? "Hide Queue" : "Queue";
            RefreshQueuePanel();
        }

        private void queueRefreshButton_Click(object sender, EventArgs e)
        {
            UpdateCurrentPlayingRowStyle();
            UpdatePlaylistCounters();
            dataGridView1.Refresh();
            PerformDiskScanAndUpdatePanel();
        }

        private void diskToggleButton_Click(object sender, EventArgs e)
        {
            if (!_diskPanelVisible)
            {
                SetQueuePanelVisible(false);
            }

            _diskPanelVisible = !_diskPanelVisible;
            _diskPanel.Visible = _diskPanelVisible;
            _diskToggleButton.Text = _diskPanelVisible ? "Hide Disk" : "Disk";
            if (_diskPanelVisible)
            {
                PerformDiskScanAndUpdatePanel();
            }
        }

        private void SetQueuePanelVisible(bool visible)
        {
            _queuePanelVisible = visible;
            if (_queuePanel != null)
            {
                _queuePanel.Visible = visible;
            }
            if (_queueToggleButton != null)
            {
                _queueToggleButton.Text = visible ? "Hide Queue" : "Queue";
            }
        }

        private void SetDiskPanelVisible(bool visible)
        {
            _diskPanelVisible = visible;
            if (_diskPanel != null)
            {
                _diskPanel.Visible = visible;
            }
            if (_diskToggleButton != null)
            {
                _diskToggleButton.Text = visible ? "Hide Disk" : "Disk";
            }
        }

        private void HideAuxPanels()
        {
            SetQueuePanelVisible(false);
            SetDiskPanelVisible(false);
        }

        private void diskRescanButton_Click(object sender, EventArgs e)
        {
            PerformDiskScanAndUpdatePanel();
        }

        private void diskApplyNewButton_Click(object sender, EventArgs e)
        {
            if (_pendingNewTracks.Count == 0)
                return;

            DialogResult result = MessageBox.Show(
                string.Format("Add {0} newly found track(s) to playlist?", _pendingNewTracks.Count),
                "Confirm Add Tracks",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            ApplyPendingNewTracks();
        }

        private void diskRemoveMissingButton_Click(object sender, EventArgs e)
        {
            if (_pendingMissingTracks.Count == 0)
                return;

            DialogResult result = MessageBox.Show(
                string.Format("Remove {0} missing track(s) from playlist?", _pendingMissingTracks.Count),
                "Confirm Remove Missing",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            RemovePendingMissingTracks();
        }

        private void diskAutoRefreshCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _autoRefreshEnabled = _diskAutoRefreshCheckBox != null && _diskAutoRefreshCheckBox.Checked;
        }

        private void diskRescanDebounceTimer_Tick(object sender, EventArgs e)
        {
            if (_diskRescanDebounceTimer != null)
            {
                _diskRescanDebounceTimer.Stop();
            }

            PerformDiskScanAndUpdatePanel();
        }

        private void RegisterWatchDirectory(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath) || Directory.Exists(directoryPath) == false)
                return;

            string fullPath = Path.GetFullPath(directoryPath);
            if (_watchedDirectories.Contains(fullPath))
                return;

            _watchedDirectories.Add(fullPath);

            FileSystemWatcher watcher = new FileSystemWatcher(fullPath)
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite,
                EnableRaisingEvents = true
            };

            watcher.Created += watchedDirectory_Changed;
            watcher.Deleted += watchedDirectory_Changed;
            watcher.Renamed += watchedDirectory_Changed;
            watcher.Changed += watchedDirectory_Changed;

            _watchers[fullPath] = watcher;
        }

        private void EnsureWatchDirectoriesFromPlaylist()
        {
            if (YAMPVars.TrackList == null)
                return;

            foreach (TrackInfo track in YAMPVars.TrackList)
            {
                if (track == null || string.IsNullOrWhiteSpace(track.Path))
                    continue;

                string directory = Path.GetDirectoryName(track.Path);
                RegisterWatchDirectory(directory);
            }
        }

        private void watchedDirectory_Changed(object sender, FileSystemEventArgs e)
        {
            if (IsDisposed)
                return;

            if (_diskRescanDebounceTimer == null)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() =>
                {
                    _diskRescanDebounceTimer.Stop();
                    _diskRescanDebounceTimer.Start();
                }));
            }
            else
            {
                _diskRescanDebounceTimer.Stop();
                _diskRescanDebounceTimer.Start();
            }
        }

        private void PerformDiskScanAndUpdatePanel()
        {
            EnsureWatchDirectoriesFromPlaylist();

            HashSet<string> existingPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (YAMPVars.TrackList != null)
            {
                foreach (TrackInfo track in YAMPVars.TrackList)
                {
                    if (track != null && !string.IsNullOrWhiteSpace(track.Path))
                    {
                        existingPaths.Add(track.Path);
                    }
                }
            }

            List<string> discovered = new List<string>();
            foreach (string directory in _watchedDirectories)
            {
                if (!Directory.Exists(directory))
                    continue;

                try
                {
                    DirectoryInfo dir = new DirectoryInfo(directory);
                    IEnumerable<FileInfo> files = AudioFileSupport.EnumerateSupportedAudioFiles(dir);
                    foreach (FileInfo file in files)
                    {
                        if (file != null && !existingPaths.Contains(file.FullName))
                        {
                            discovered.Add(file.FullName);
                        }
                    }
                }
                catch
                {
                }
            }

            List<string> missing = new List<string>();
            if (YAMPVars.TrackList != null)
            {
                foreach (TrackInfo track in YAMPVars.TrackList)
                {
                    if (track == null || string.IsNullOrWhiteSpace(track.Path))
                        continue;

                    if (!File.Exists(track.Path))
                    {
                        missing.Add(track.Path);
                    }
                }
            }

            lock (_diskSyncLock)
            {
                _pendingNewTracks.Clear();
                _pendingNewTracks.AddRange(discovered.Distinct(StringComparer.OrdinalIgnoreCase));
                _pendingMissingTracks.Clear();
                _pendingMissingTracks.AddRange(missing.Distinct(StringComparer.OrdinalIgnoreCase));
            }

            RefreshDiskPanel();

            if (_autoRefreshEnabled)
            {
                if (_pendingNewTracks.Count > 0)
                {
                    ApplyPendingNewTracks();
                }

                if (_pendingMissingTracks.Count > 0)
                {
                    RemovePendingMissingTracks();
                }
            }

            UpdateCurrentPlayingRowStyle();
            UpdatePlaylistCounters();
        }

        private void RefreshDiskPanel()
        {
            if (_diskListBox == null)
                return;

            _diskListBox.Items.Clear();
            _diskListBox.Items.Add(string.Format("Watched folders: {0}", _watchedDirectories.Count));
            _diskListBox.Items.Add(string.Format("New tracks found: {0}", _pendingNewTracks.Count));
            _diskListBox.Items.Add(string.Format("Missing tracks: {0}", _pendingMissingTracks.Count));

            foreach (string path in _pendingNewTracks.Take(6))
            {
                _diskListBox.Items.Add("+ " + Path.GetFileName(path));
            }

            foreach (string path in _pendingMissingTracks.Take(6))
            {
                _diskListBox.Items.Add("- " + Path.GetFileName(path));
            }

            if (_pendingNewTracks.Count > 6 || _pendingMissingTracks.Count > 6)
            {
                _diskListBox.Items.Add("...");
            }
        }

        private void ApplyPendingNewTracks()
        {
            List<string> toAdd;
            lock (_diskSyncLock)
            {
                toAdd = _pendingNewTracks.ToList();
            }

            if (toAdd.Count == 0)
                return;

            InsertTracks(toAdd.ToArray());
            PerformDiskScanAndUpdatePanel();
        }

        private void RemovePendingMissingTracks()
        {
            List<string> missing;
            lock (_diskSyncLock)
            {
                missing = _pendingMissingTracks.ToList();
            }

            if (missing.Count == 0)
                return;

            HashSet<string> missingSet = new HashSet<string>(missing, StringComparer.OrdinalIgnoreCase);

            for (int i = YAMPVars.TrackList.Count - 1; i >= 0; i--)
            {
                TrackInfo track = YAMPVars.TrackList[i];
                if (track == null || string.IsNullOrWhiteSpace(track.Path))
                    continue;

                if (!missingSet.Contains(track.Path))
                    continue;

                if (YAMPVars.CORE != null &&
                    YAMPVars.CORE.CurrentTrack != null &&
                    string.Equals(YAMPVars.CORE.CurrentTrack.Path, track.Path, StringComparison.OrdinalIgnoreCase))
                {
                    YAMPVars.CORE.Stop();
                    YAMPVars.CORE.CurrentTrack = null;
                }

                YAMPVars.PendingQueue.RemoveAll(x => x != null && x.Path == track.Path);
                PlaylistSource.RemoveAt(i);
            }

            PerformDiskScanAndUpdatePanel();
        }

        private void DisposeWatchers()
        {
            foreach (KeyValuePair<string, FileSystemWatcher> pair in _watchers)
            {
                FileSystemWatcher watcher = pair.Value;
                if (watcher == null)
                    continue;

                watcher.EnableRaisingEvents = false;
                watcher.Created -= watchedDirectory_Changed;
                watcher.Deleted -= watchedDirectory_Changed;
                watcher.Renamed -= watchedDirectory_Changed;
                watcher.Changed -= watchedDirectory_Changed;
                watcher.Dispose();
            }

            _watchers.Clear();
            _watchedDirectories.Clear();

            if (_diskRescanDebounceTimer != null)
            {
                _diskRescanDebounceTimer.Stop();
                _diskRescanDebounceTimer.Tick -= diskRescanDebounceTimer_Tick;
                _diskRescanDebounceTimer.Dispose();
                _diskRescanDebounceTimer = null;
            }
        }

        private void RefreshQueuePanel()
        {
            if (_queueListBox == null)
                return;

            int selectedIndex = _queueListBox.SelectedIndex;
            _queueListBox.Items.Clear();

            if (YAMPVars.PendingQueue != null)
            {
                for (int i = 0; i < YAMPVars.PendingQueue.Count; i++)
                {
                    TrackInfo track = YAMPVars.PendingQueue[i];
                    string name = track != null ? track.Title : "<unknown>";
                    _queueListBox.Items.Add(string.Format("{0}. {1}", i + 1, name));
                }
            }

            if (_queueListBox.Items.Count > 0)
            {
                _queueListBox.SelectedIndex = Math.Max(0, Math.Min(selectedIndex, _queueListBox.Items.Count - 1));
            }
        }

        private void queueClearButton_Click(object sender, EventArgs e)
        {
            YAMPVars.PendingQueue.Clear();
            UpdatePlaylistCounters();
        }

        private void queueRemoveButton_Click(object sender, EventArgs e)
        {
            int index = _queueListBox != null ? _queueListBox.SelectedIndex : -1;
            if (index < 0 || index >= YAMPVars.PendingQueue.Count)
                return;

            YAMPVars.PendingQueue.RemoveAt(index);
            UpdatePlaylistCounters();
        }

        private void queueUpButton_Click(object sender, EventArgs e)
        {
            int index = _queueListBox != null ? _queueListBox.SelectedIndex : -1;
            if (index <= 0 || index >= YAMPVars.PendingQueue.Count)
                return;

            TrackInfo temp = YAMPVars.PendingQueue[index - 1];
            YAMPVars.PendingQueue[index - 1] = YAMPVars.PendingQueue[index];
            YAMPVars.PendingQueue[index] = temp;
            UpdatePlaylistCounters();
            if (_queueListBox.Items.Count > 0)
            {
                _queueListBox.SelectedIndex = index - 1;
            }
        }

        private void queueDownButton_Click(object sender, EventArgs e)
        {
            int index = _queueListBox != null ? _queueListBox.SelectedIndex : -1;
            if (index < 0 || index >= YAMPVars.PendingQueue.Count - 1)
                return;

            TrackInfo temp = YAMPVars.PendingQueue[index + 1];
            YAMPVars.PendingQueue[index + 1] = YAMPVars.PendingQueue[index];
            YAMPVars.PendingQueue[index] = temp;
            UpdatePlaylistCounters();
            if (_queueListBox.Items.Count > 0)
            {
                _queueListBox.SelectedIndex = index + 1;
            }
        }
        public YAMPlaylistDialog()
        {
            InitializeComponent();
            KeyPreview = true;
            FormClosed += YAMPlaylistDialog_FormClosed;
            Deactivate += YAMPlaylistDialog_Deactivate;
            KeyDown += YAMPlaylistDialog_KeyDown;
            InitializeQueuePanel();
            InitializePlaybackBehaviorMenu();
            //LoadedPlaylist = YAMPVars.LoadedPlaylist;
            if (!string.IsNullOrEmpty(YAMPVars.LoadedPlaylist))
            {
                LoadedPlaylist = YAMPVars.LoadedPlaylist;
                Text = "Playlist: " + new FileInfo(LoadedPlaylist).Name;
            }

            if (PlaylistSource == null)
            {
                PlaylistSource = new BindingSource
                {
                    DataSource = YAMPVars.TrackList
                };
            }

            dataGridView1.DataSource = PlaylistSource;
            dataGridView1.Columns["Duration"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

            // Info button column
            DataGridViewButtonColumn DGVBC_Info = new DataGridViewButtonColumn()
            {
                HeaderText = "Info",
                Text = "i",
                UseColumnTextForButtonValue = true,
                Name = "clm_MediaInfo",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader,
                DisplayIndex = dataGridView1.Columns.Count
            };
            dataGridView1.Columns.Add(DGVBC_Info);

            // Remove button column
            DataGridViewButtonColumn DGVBC_Remove = new DataGridViewButtonColumn()
            {
                HeaderText = "Remove",
                Text = "X",
                UseColumnTextForButtonValue = true,
                Name = "clm_Remove",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader,
                DisplayIndex = dataGridView1.Columns.Count
            };
            dataGridView1.Columns.Add(DGVBC_Remove);

            foreach (DataGridViewColumn item in dataGridView1.Columns)
            {
                if (item.HeaderText != "Title" && item.HeaderText != "Duration" && item.HeaderText != "Info" && item.HeaderText != "Remove")
                {
                    item.Visible = false;
                }
            }

            DGVBC_Info.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DGVBC_Remove.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void Btn_info_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Clicked");
        }

        private void shortcutInfoButton_Click(object sender, EventArgs e)
        {
            ShowShortcutsDialog();
        }

        private void ShowShortcutsDialog()
        {
            string shortcutsText =
                "Add\r\n" +
                "- Ctrl+Shift+F: Add file(s)\r\n" +
                "- Ctrl+Shift+D: Add directory\r\n" +
                "\r\n" +
                "Playback\r\n" +
                "- Enter: Play selected track\r\n" +
                "\r\n" +
                "Queue\r\n" +
                "- Ctrl+Shift+Q: Queue selected as next\r\n" +
                "- Ctrl+Q: Queue selected to end\r\n" +
                "- Ctrl+L: Clear queue\r\n" +
                "\r\n" +
                "Track Actions\r\n" +
                "- Delete: Remove selected track(s)\r\n" +
                "- I: Open media info for selected track\r\n" +
                "\r\n" +
                "Selection and Order\r\n" +
                "- Ctrl+A: Select all tracks\r\n" +
                "- Ctrl+Up: Move selected tracks up\r\n" +
                "- Ctrl+Down: Move selected tracks down\r\n" +
                "\r\n" +
                "Help\r\n" +
                "- F1: Show this shortcuts help";

            using (Form helpForm = new Form())
            {
                helpForm.Text = "Keyboard Shortcuts";
                helpForm.StartPosition = FormStartPosition.CenterParent;
                helpForm.Size = new Size(520, 470);
                helpForm.MinimizeBox = false;
                helpForm.MaximizeBox = false;

                TextBox txt = new TextBox();
                txt.Multiline = true;
                txt.Dock = DockStyle.Fill;
                txt.ScrollBars = ScrollBars.Vertical;
                txt.Enabled = false;
                txt.Text = shortcutsText;

                helpForm.Controls.Add(txt);
                helpForm.ShowDialog(this);
            }
        }

        private void YAMPlaylistDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && e.KeyCode == Keys.F)
            {
                fileToolStripMenuItem_Click(this, EventArgs.Empty);
                e.Handled = true;
                return;
            }

            if (e.Control && e.Shift && e.KeyCode == Keys.D)
            {
                directoryToolStripMenuItem_Click(this, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void PlayTrackAtRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= YAMPVars.TrackList.Count)
                return;

            TrackSelected?.Invoke(this, new TrackSelectedEventArgs(YAMPVars.TrackList[rowIndex]));
            UpdateCurrentPlayingRowStyle();
        }

        private void ShowMediaInfoForRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= YAMPVars.TrackList.Count)
                return;

            MediaInfoDialog mediaInfo = new MediaInfoDialog(YAMPVars.TrackList[rowIndex].File.FullName);
            mediaInfo.ShowDialog();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OFD = new OpenFileDialog() { Filter = AudioFileSupport.OpenFileFilter, Multiselect = true })
            {
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    InsertTracks(OFD.FileNames);
                    UpdatePlaylistCounters();
                }
            }
        }

        private void InsertTracks(string[] Tracks)
        {
            foreach (string TrackName in Tracks)
            {
                string resolvedPath = ResolveMissingTrackPath(TrackName);
                if (string.IsNullOrEmpty(resolvedPath))
                    continue;

                if (!AudioFileSupport.IsSupportedAudioFile(resolvedPath))
                {
                    MessageBox.Show(string.Format("{0} is not a supported audio file.", Path.GetFileName(resolvedPath)));
                    continue;
                }

                TrackInfo track = new TrackInfo(resolvedPath);
                if (!TrackExist(track))
                {
                    PlaylistSource.Add(track);
                }
                else
                {
                    MessageBox.Show(string.Format("{0} Already Exist!", track.Title));
                }
            }
        }

        private bool TrackExist(TrackInfo track)
        {
            bool Exst = dataGridView1.Rows.OfType<DataGridViewRow>()
                .Any(row => string.Equals(
                    Convert.ToString(row.Cells["path"].Value),
                    track.File.FullName,
                    StringComparison.OrdinalIgnoreCase));
            return Exst;
        }

        private string ResolveMissingTrackPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return null;

            if (File.Exists(path))
                return path;

            DialogResult result = MessageBox.Show(
                string.Format("Missing file: {0}\n\nTry locating it in another folder?", path),
                "Missing Track",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return null;

            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() != DialogResult.OK)
                    return null;

                string fileName = Path.GetFileName(path);
                string[] matches = Directory.GetFiles(folderDialog.SelectedPath, fileName, SearchOption.AllDirectories);
                if (matches.Length > 0)
                {
                    return matches[0];
                }
            }

            MessageBox.Show("Could not find a replacement file.", "Missing Track", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;
        }

        private void UpdatePlaybackMenuLabels()
        {
            if (_shuffleMenuItem != null)
            {
                _shuffleMenuItem.Text = "Shuffle: " + (YAMPVars.ShuffleEnabled ? "On" : "Off");
            }

            if (_loopModeMenuItem != null)
            {
                _loopModeMenuItem.Text = "Loop: " + YAMPVars.PlaylistLoopMode;
            }

            if (_endBehaviorStopItem != null)
            {
                _endBehaviorStopItem.Checked = YAMPVars.PlaylistLoopMode == PlaylistLoopMode.None;
                _endBehaviorRepeatCurrentItem.Checked = YAMPVars.PlaylistLoopMode == PlaylistLoopMode.One;
                _endBehaviorLoopPlaylistItem.Checked = YAMPVars.PlaylistLoopMode == PlaylistLoopMode.All;
            }
        }

        private void InitializePlaybackBehaviorMenu()
        {
            if (playbackMenu == null || _endBehaviorMenuItem != null)
                return;

            _endBehaviorMenuItem = new ToolStripMenuItem("When Queue Ends");
            _endBehaviorStopItem = new ToolStripMenuItem("Stop");
            _endBehaviorRepeatCurrentItem = new ToolStripMenuItem("Repeat Current");
            _endBehaviorLoopPlaylistItem = new ToolStripMenuItem("Loop Playlist");

            _endBehaviorStopItem.Click += (s, e) => SetLoopMode(PlaylistLoopMode.None);
            _endBehaviorRepeatCurrentItem.Click += (s, e) => SetLoopMode(PlaylistLoopMode.One);
            _endBehaviorLoopPlaylistItem.Click += (s, e) => SetLoopMode(PlaylistLoopMode.All);

            _endBehaviorMenuItem.DropDownItems.Add(_endBehaviorStopItem);
            _endBehaviorMenuItem.DropDownItems.Add(_endBehaviorRepeatCurrentItem);
            _endBehaviorMenuItem.DropDownItems.Add(_endBehaviorLoopPlaylistItem);
            playbackMenu.DropDownItems.Add(new ToolStripSeparator());
            playbackMenu.DropDownItems.Add(_endBehaviorMenuItem);
        }

        private void SetLoopMode(PlaylistLoopMode mode)
        {
            YAMPVars.PlaylistLoopMode = mode;
            UpdatePlaybackMenuLabels();
            SessionStateManager.SavePlaybackSettings();
        }

        private void clearFilterItem_Click(object sender, EventArgs e)
        {
            _searchBox.Text = string.Empty;
            ApplyFilter(string.Empty);
        }

        private void shuffleMenuItem_Click(object sender, EventArgs e)
        {
            if (YAMPVars.CORE == null)
                return;
            YAMPVars.CORE.ToggleShuffle();
            UpdatePlaybackMenuLabels();
            SessionStateManager.SavePlaybackSettings();
        }

        private void loopModeMenuItem_Click(object sender, EventArgs e)
        {
            if (YAMPVars.CORE == null)
                return;
            YAMPVars.CORE.CycleLoopMode();
            UpdatePlaybackMenuLabels();
            SessionStateManager.SavePlaybackSettings();
        }

        private void mostPlayedMenuItem_Click(object sender, EventArgs e)
        {
            ApplySmartSort((a, b) => b.PlayCount.CompareTo(a.PlayCount));
        }

        private void recentlyPlayedMenuItem_Click(object sender, EventArgs e)
        {
            ApplySmartSort((a, b) => Nullable.Compare(b.LastPlayedAt, a.LastPlayedAt));
        }

        private void neverPlayedMenuItem_Click(object sender, EventArgs e)
        {
            ApplyFilterPredicate(track => track.PlayCount == 0);
        }

        private void byFolderMenuItem_Click(object sender, EventArgs e)
        {
            ApplySmartSort((a, b) => string.Compare(
                a.File.DirectoryName,
                b.File.DirectoryName,
                StringComparison.OrdinalIgnoreCase));
        }

        private void showAllSmartMenuItem_Click(object sender, EventArgs e)
        {
            RestoreOriginalOrder();
            ApplyFilter(_activeFilter);
        }

        private void playNextMenuItem_Click(object sender, EventArgs e)
        {
            QueueSelectedTracks(next: true);
        }

        private void queueLastMenuItem_Click(object sender, EventArgs e)
        {
            QueueSelectedTracks(next: false);
        }

        private void removeSelectedMenuItem_Click(object sender, EventArgs e)
        {
            RemoveSelectedRows();
        }

        private void rate1MenuItem_Click(object sender, EventArgs e) { RateSelectedTracks(1); }
        private void rate2MenuItem_Click(object sender, EventArgs e) { RateSelectedTracks(2); }
        private void rate3MenuItem_Click(object sender, EventArgs e) { RateSelectedTracks(3); }
        private void rate4MenuItem_Click(object sender, EventArgs e) { RateSelectedTracks(4); }
        private void rate5MenuItem_Click(object sender, EventArgs e) { RateSelectedTracks(5); }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter(_searchBox != null ? _searchBox.Text : string.Empty);
        }

        private void ApplyFilter(string filterText)
        {
            _activeFilter = filterText ?? string.Empty;
            string needle = _activeFilter.Trim();

            dataGridView1.CurrentCell = null;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                TrackInfo track = row.DataBoundItem as TrackInfo;
                if (track == null || string.IsNullOrEmpty(needle))
                {
                    row.Visible = true;
                    continue;
                }

                bool matched = ContainsIgnoreCase(track.Title, needle) ||
                               ContainsIgnoreCase(track.AlbumArtist, needle) ||
                               ContainsIgnoreCase(track.Album, needle) ||
                               ContainsIgnoreCase(track.Genre, needle) ||
                               ContainsIgnoreCase(track.Path, needle);

                row.Visible = matched;
            }

            UpdateCurrentPlayingRowStyle();
        }

        private void ApplyFilterPredicate(Func<TrackInfo, bool> predicate)
        {
            dataGridView1.CurrentCell = null;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                TrackInfo track = row.DataBoundItem as TrackInfo;
                row.Visible = track != null && predicate(track);
            }

            UpdateCurrentPlayingRowStyle();
        }

        private static bool ContainsIgnoreCase(string text, string value)
        {
            return !string.IsNullOrEmpty(text) &&
                   !string.IsNullOrEmpty(value) &&
                   text.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void ApplySmartSort(Comparison<TrackInfo> comparison)
        {
            List<TrackInfo> sorted = YAMPVars.TrackList.ToList();
            sorted.Sort(comparison);

            YAMPVars.TrackList.Clear();
            YAMPVars.TrackList.AddRange(sorted);
            PlaylistSource.ResetBindings(false);
            ApplyFilter(_activeFilter);
            UpdateCurrentPlayingRowStyle();
        }

        private void RestoreOriginalOrder()
        {
            List<TrackInfo> ordered = YAMPVars.TrackList
                .OrderBy(x => x.AddedAt)
                .ToList();

            YAMPVars.TrackList.Clear();
            YAMPVars.TrackList.AddRange(ordered);
            PlaylistSource.ResetBindings(false);
            UpdateCurrentPlayingRowStyle();
        }

        private List<int> GetSelectedRowIndices()
        {
            return dataGridView1.SelectedRows
                .Cast<DataGridViewRow>()
                .Select(row => row.Index)
                .Where(index => index >= 0 && index < YAMPVars.TrackList.Count)
                .Distinct()
                .OrderBy(index => index)
                .ToList();
        }

        private void RemoveSelectedRows()
        {
            List<int> selectedRows = GetSelectedRowIndices();
            if (selectedRows.Count == 0)
                return;

            DialogResult result = MessageBox.Show(
                "Remove selected tracks from playlist?",
                "Confirm Removal",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            foreach (int rowIndex in selectedRows.OrderByDescending(x => x))
            {
                if (rowIndex >= 0 && rowIndex < PlaylistSource.Count)
                {
                    TrackInfo toRemove = YAMPVars.TrackList[rowIndex];

                    if (YAMPVars.CORE != null &&
                        YAMPVars.CORE.CurrentTrack != null &&
                        string.Equals(YAMPVars.CORE.CurrentTrack.Path, toRemove.Path, StringComparison.OrdinalIgnoreCase))
                    {
                        YAMPVars.CORE.Stop();
                        YAMPVars.CORE.CurrentTrack = null;
                    }

                    YAMPVars.PendingQueue.RemoveAll(x => x != null && x.Path == toRemove.Path);
                    PlaylistSource.RemoveAt(rowIndex);
                }
            }

            if (PlaylistSource.Count == 0)
            {
                pictureBox1.Image = null;
            }

            UpdateCurrentPlayingRowStyle();
            UpdatePlaylistCounters();
            dataGridView1.Refresh();
        }

        private void QueueSelectedTracks(bool next)
        {
            if (YAMPVars.CORE == null)
                return;

            List<int> selectedRows = GetSelectedRowIndices();
            if (selectedRows.Count == 0)
                return;

            IEnumerable<int> rows;
            if (next)
            {
                rows = selectedRows.OrderByDescending(x => x);
            }
            else
            {
                rows = selectedRows;
            }
            foreach (int rowIndex in rows)
            {
                TrackInfo track = YAMPVars.TrackList[rowIndex];
                if (next)
                {
                    YAMPVars.CORE.EnqueueNext(track);
                }
                else
                {
                    YAMPVars.CORE.EnqueueLast(track);
                }
            }

            UpdatePlaylistCounters();
        }

        private void RateSelectedTracks(int rating)
        {
            foreach (int rowIndex in GetSelectedRowIndices())
            {
                YAMPVars.TrackList[rowIndex].Rating = rating;
            }
            PlaylistSource.ResetBindings(false);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= YAMPVars.TrackList.Count)
                return;

            if (dataGridView1[e.ColumnIndex, e.RowIndex].OwningColumn.GetType() == typeof(DataGridViewButtonColumn))
            {
                string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

                if (columnName == "clm_MediaInfo")
                {
                    ShowMediaInfoForRow(e.RowIndex);
                }
                else if (columnName == "clm_Remove")
                {
                    // Remove track from playlist
                    RemoveTrackAt(e.RowIndex);
                }
            }
        }

        /// <summary>
        /// Removes a track at the specified row index from the playlist
        /// </summary>
        /// <param name="rowIndex">The index of the row to remove</param>
        private void RemoveTrackAt(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= PlaylistSource.Count)
                return;

            TrackInfo removedTrack = YAMPVars.TrackList[rowIndex];
            bool removedCurrentTrack = YAMPVars.CORE != null &&
                                      YAMPVars.CORE.CurrentTrack != null &&
                                      string.Equals(YAMPVars.CORE.CurrentTrack.Path, removedTrack.Path, StringComparison.OrdinalIgnoreCase);

            // Confirm removal
            string trackTitle = YAMPVars.TrackList[rowIndex].Title;
            DialogResult result = MessageBox.Show(
                string.Format("Remove '{0}' from playlist?", trackTitle),
                "Confirm Removal",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                YAMPVars.PendingQueue.RemoveAll(x => x != null && x.Path == removedTrack.Path);
                PlaylistSource.RemoveAt(rowIndex);

                if (removedCurrentTrack && YAMPVars.CORE != null)
                {
                    YAMPVars.CORE.Stop();
                    YAMPVars.CORE.CurrentTrack = null;
                }

                // Clear selection and refresh
                dataGridView1.ClearSelection();

                // Select next available row
                if (PlaylistSource.Count > 0)
                {
                    if (rowIndex >= PlaylistSource.Count)
                    {
                        dataGridView1.CurrentCell = dataGridView1[0, PlaylistSource.Count - 1];
                    }
                    else
                    {
                        dataGridView1.CurrentCell = dataGridView1[0, rowIndex];
                    }
                    dataGridView1.CurrentCell.Selected = true;
                }
                else
                {
                    // No tracks left, clear cover image
                    pictureBox1.Image = null;
                }

                // Update row styling if current track is affected
                UpdateCurrentPlayingRowStyle();
                UpdatePlaylistCounters();
                dataGridView1.RefreshEdit();
            }
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged == DataGridViewElementStates.Selected)
            {
                if (e.Row == null || e.Row.Index < 0 || e.Row.Index >= YAMPVars.TrackList.Count)
                    return;

                pictureBox1.Image = null;
                if (YAMPVars.TrackList[e.Row.Index].Covers.Count > 0 &&
                    IsUsableImage(YAMPVars.TrackList[e.Row.Index].Covers[0]))
                {
                    pictureBox1.Image = YAMPVars.TrackList[e.Row.Index].Covers[0];
                }
            }
        }

        private bool IsUsableImage(Image image)
        {
            if (image == null)
                return false;

            try
            {
                return image.Width > 0 && image.Height > 0;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                new BigArt(pictureBox1.Image).ShowDialog();
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            dataGridView1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || PlaylistSource == null || PlaylistSource.Count == 0)
                return;

            int direction = Convert.ToInt32(((Button)sender).Tag);
            List<int> selectedRows = GetSelectedRowIndices();
            if (selectedRows.Count == 0)
            {
                selectedRows.Add(dataGridView1.CurrentRow.Index);
            }

            MoveRows(selectedRows, direction);
            UpdateCurrentPlayingRowStyle();
        }

        private void MoveRows(List<int> selectedRows, int direction)
        {
            if (selectedRows == null || selectedRows.Count == 0)
                return;

            if (direction < 0)
            {
                foreach (int row in selectedRows.OrderBy(x => x))
                {
                    int target = row - 1;
                    if (target >= 0)
                    {
                        SwapTracks(row, target);
                    }
                }
            }
            else
            {
                foreach (int row in selectedRows.OrderByDescending(x => x))
                {
                    int target = row + 1;
                    if (target < PlaylistSource.Count)
                    {
                        SwapTracks(row, target);
                    }
                }
            }

            PlaylistSource.ResetBindings(false);
            List<int> movedIndices = selectedRows
                .Select(x => Math.Max(0, Math.Min(PlaylistSource.Count - 1, x + direction)))
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            dataGridView1.ClearSelection();

            foreach (int index in movedIndices)
            {
                if (index >= 0 && index < dataGridView1.Rows.Count)
                {
                    dataGridView1.Rows[index].Selected = true;
                }
            }

            int focusIndex = movedIndices.FirstOrDefault();
            if (focusIndex >= 0 && focusIndex < dataGridView1.Rows.Count && dataGridView1.Rows[focusIndex].Visible)
            {
                dataGridView1.CurrentCell = dataGridView1[0, focusIndex];
            }
        }

        private void SwapTracks(int firstIndex, int secondIndex)
        {
            if (firstIndex < 0 || secondIndex < 0 || firstIndex >= YAMPVars.TrackList.Count || secondIndex >= YAMPVars.TrackList.Count)
                return;

            TrackInfo temp = YAMPVars.TrackList[firstIndex];
            YAMPVars.TrackList[firstIndex] = YAMPVars.TrackList[secondIndex];
            YAMPVars.TrackList[secondIndex] = temp;
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
        }

        private void directoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                AddTracksFromDirectory(folderBrowserDialog1.SelectedPath);
                UpdatePlaylistCounters();
            }
        }

        public static void LoadDirectory()
        {
            using (FolderBrowserDialog FBD = new FolderBrowserDialog())
            {
                if (FBD.ShowDialog() == DialogResult.OK)
                {
                    AddTracksFromDirectory(FBD.SelectedPath);
                }
            }
        }

        private static void AddTracksFromDirectory(string selectedPath)
        {
            if (string.IsNullOrWhiteSpace(selectedPath) || Directory.Exists(selectedPath) == false)
                return;

            DirectoryInfo dir = new DirectoryInfo(selectedPath);
            FileInfo[] files = AudioFileSupport.EnumerateSupportedAudioFiles(dir).ToArray();
            foreach (FileInfo item in files)
            {
                try
                {
                    TrackInfo track = new TrackInfo(item.FullName);
                    if (PlaylistSource == null)
                    {
                        PlaylistSource = new BindingSource()
                        {
                            DataSource = YAMPVars.TrackList
                        };
                    }
                    PlaylistSource.Add(track);
                }
                catch (Exception)
                {
                }
            }

            // Keep bottom counters in sync after bulk directory add.
            // Static path can be called externally; guard UI update by checking open forms.
            foreach (Form form in Application.OpenForms)
            {
                YAMPlaylistDialog playlistForm = form as YAMPlaylistDialog;
                if (playlistForm != null && !playlistForm.IsDisposed)
                {
                    playlistForm.RegisterWatchDirectory(selectedPath);
                    playlistForm.PerformDiskScanAndUpdatePanel();
                    playlistForm.UpdatePlaylistCounters();
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < YAMPVars.TrackList.Count)
            {
                PlayTrackAtRow(e.RowIndex);
            }
        }

        private void UpdateCurrentPlayingRowStyle()
        {
            ApplyMissingTrackStyles();

            int CPR = GetCurrentPlayingRowIndex();
            if (CPR >= 0 && CPR < dataGridView1.Rows.Count)
            {
                dataGridView1.Rows[CPR].DefaultCellStyle.BackColor = Color.SeaGreen;
                dataGridView1.Rows[CPR].DefaultCellStyle.ForeColor = Color.White;
                CurrentColoredRowIndex = CPR;
            }
            else
            {
                CurrentColoredRowIndex = -1;
            }
        }

        private void ApplyMissingTrackStyles()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.DefaultCellStyle = new DataGridViewCellStyle();

                TrackInfo track = row.DataBoundItem as TrackInfo;
                if (track == null || string.IsNullOrWhiteSpace(track.Path))
                    continue;

                if (!File.Exists(track.Path))
                {
                    row.DefaultCellStyle.BackColor = Color.Khaki;
                    row.DefaultCellStyle.ForeColor = Color.Red;
                }
            }
        }

        private int GetCurrentPlayingRowIndex()
        {
            int CurrentPlayingIndex = -1;
            if (YAMPVars.CORE != null && YAMPVars.CORE.CurrentTrack != null)
            {
                foreach (DataGridViewRow item in dataGridView1.Rows)
                {
                    object pathValue = item.Cells["Path"].EditedFormattedValue;
                    if (pathValue != null && pathValue.ToString() == YAMPVars.CORE.CurrentTrack.Path)
                    {
                        CurrentPlayingIndex = item.Index;
                        break;
                    }
                }
            }
            return CurrentPlayingIndex;
        }

        private void label1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
                label1.BackColor = Color.Green;
                label1.ForeColor = Color.White;
            }
        }

        private void label1_DragLeave(object sender, EventArgs e)
        {
            label1.BackColor = SystemColors.Control;
            label1.ForeColor = Color.Black;
        }

        private void label1_DragDrop(object sender, DragEventArgs e)
        {
            FileInfo[] DroppedFiles = ((string[])e.Data.GetData(DataFormats.FileDrop)).Select(x => new FileInfo(x)).ToArray();
            foreach (FileInfo File in DroppedFiles)
            {
                string resolvedPath = ResolveMissingTrackPath(File.FullName);
                if (string.IsNullOrEmpty(resolvedPath) || !AudioFileSupport.IsSupportedAudioFile(resolvedPath))
                    continue;

                TrackInfo Track = new TrackInfo(resolvedPath);
                PlaylistSource.Add(Track);
                label1_DragLeave(sender, e);
            }

            UpdatePlaylistCounters();
        }

        private void delselectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || PlaylistSource == null || PlaylistSource.Count == 0)
                return;

            RemoveSelectedRows();
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PlaylistSource == null || PlaylistSource.Count == 0)
                return;

            DialogResult result = MessageBox.Show(
                "Clear all tracks from playlist?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            YAMPVars.PendingQueue.Clear();
            PlaylistSource.Clear();
            if (YAMPVars.CORE != null)
            {
                YAMPVars.CORE.Stop();
                YAMPVars.CORE.CurrentTrack = null;
            }
            pictureBox1.Image = null;
            UpdateCurrentPlayingRowStyle();
            UpdatePlaylistCounters();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                ShowShortcutsDialog();
                e.Handled = true;
                return;
            }

            if (e.Control && e.Shift && e.KeyCode == Keys.Q)
            {
                QueueSelectedTracks(true);
                e.Handled = true;
                return;
            }

            if (e.Control && e.KeyCode == Keys.Q)
            {
                QueueSelectedTracks(false);
                e.Handled = true;
                return;
            }

            if (e.Control && e.KeyCode == Keys.L)
            {
                YAMPVars.PendingQueue.Clear();
                UpdatePlaylistCounters();
                e.Handled = true;
                return;
            }

            if (e.KeyCode == Keys.Delete)
            {
                RemoveSelectedRows();
                e.Handled = true;
                return;
            }

            if (e.Control && e.KeyCode == Keys.A)
            {
                dataGridView1.SelectAll();
                e.Handled = true;
                return;
            }

            if (e.Control && e.KeyCode == Keys.Up)
            {
                MoveRows(GetSelectedRowIndices(), -1);
                e.Handled = true;
                return;
            }

            if (e.Control && e.KeyCode == Keys.Down)
            {
                MoveRows(GetSelectedRowIndices(), 1);
                e.Handled = true;
                return;
            }

            if (e.KeyCode == Keys.Enter && dataGridView1.CurrentRow != null)
            {
                PlayTrackAtRow(dataGridView1.CurrentRow.Index);
                e.Handled = true;
                return;
            }

            if (e.KeyCode == Keys.I && dataGridView1.CurrentRow != null)
            {
                ShowMediaInfoForRow(dataGridView1.CurrentRow.Index);
                e.Handled = true;
            }
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dataGridView1.HitTest(e.X, e.Y);
            _dragSourceRowIndex = hit.RowIndex;

            if (e.Button == MouseButtons.Right &&
                hit.RowIndex >= 0 &&
                hit.RowIndex < dataGridView1.Rows.Count)
            {
                if (!dataGridView1.Rows[hit.RowIndex].Selected || dataGridView1.SelectedRows.Count > 1)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[hit.RowIndex].Selected = true;
                }

                int columnIndex = hit.ColumnIndex >= 0 ? hit.ColumnIndex : 0;
                if (columnIndex < dataGridView1.Columns.Count)
                {
                    dataGridView1.CurrentCell = dataGridView1[columnIndex, hit.RowIndex];
                }
            }
        }

        private void dataGridView1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
                return;

            if (_dragSourceRowIndex < 0 || _dragSourceRowIndex >= YAMPVars.TrackList.Count)
                return;

            dataGridView1.DoDragDrop(_dragSourceRowIndex, DragDropEffects.Move);
        }

        private void dataGridView1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            Point clientPoint = dataGridView1.PointToClient(new Point(e.X, e.Y));

            if (clientPoint.Y < 30 && dataGridView1.FirstDisplayedScrollingRowIndex > 0)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex -= 1;
            }
            else if (clientPoint.Y > dataGridView1.Height - 30 &&
                     dataGridView1.FirstDisplayedScrollingRowIndex < dataGridView1.RowCount - 1)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex += 1;
            }
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(int)))
                return;

            int sourceIndex = (int)e.Data.GetData(typeof(int));
            Point clientPoint = dataGridView1.PointToClient(new Point(e.X, e.Y));
            int targetIndex = dataGridView1.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            if (sourceIndex < 0 || targetIndex < 0 || sourceIndex == targetIndex)
                return;

            TrackInfo moved = YAMPVars.TrackList[sourceIndex];
            YAMPVars.TrackList.RemoveAt(sourceIndex);
            YAMPVars.TrackList.Insert(targetIndex, moved);

            PlaylistSource.ResetBindings(false);
            dataGridView1.ClearSelection();
            if (targetIndex >= 0 && targetIndex < dataGridView1.Rows.Count)
            {
                dataGridView1.Rows[targetIndex].Selected = true;
                dataGridView1.CurrentCell = dataGridView1[0, targetIndex];
            }
            UpdateCurrentPlayingRowStyle();
            UpdatePlaylistCounters();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveUpdatePlaylist();
        }


        private DataTable GetDataTableFromDataGridView(DataGridView gridView)
        {
            DataTable dt = new DataTable("YAMP_Playlist");
            dt = (DataTable)gridView.DataSource;

            foreach (DataGridViewColumn column in gridView.Columns)
            {
                if (column is DataGridViewTextBoxColumn && column.Visible)
                {
                    dt.Columns.Add(column.HeaderText);
                }
            }

            List<string> data = new List<string>();

            //gridView.Rows.OfType<DataGridViewRow>().Select(x=> new {row = x }).
            foreach (DataGridViewRow row in gridView.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    data.Add(gridView[column.ColumnName, row.Index].FormattedValue.ToString());
                }
                dt.Rows.Add(data);

            }

            return dt;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveUpdatePlaylist(LoadedPlaylist);
        }

        private void SaveUpdatePlaylist(string path = "")
        {
            DataTable dt = new DataTable()
            {
                TableName = "Music"
            };

            var TextColumns = dataGridView1.Columns.OfType<DataGridViewTextBoxColumn>()
                .Select(x => Regex.Replace(x.HeaderText, "[-/, ]", "_"))
                .Select(y => new DataColumn(y));

            dt.Columns.AddRange(TextColumns.ToArray());

            foreach (DataGridViewRow gridRow in dataGridView1.Rows)
            {
                DataRow dtRow = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dtRow[i] = gridRow.Cells[dt.Columns[i].ColumnName]?.EditedFormattedValue?.ToString() ?? string.Empty;
                }
                dt.Rows.Add(dtRow);
            }


            Stream str = new MemoryStream();
            dt.WriteXml(str);
            str.Position = 0;
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            xdoc.Load(str);
            //
            // Changing the Root Element
            System.Xml.XmlDocument xdoc2 = new System.Xml.XmlDocument();
            System.Xml.XmlElement xRootElem = xdoc2.CreateElement("YAMP_Playlist");
            xdoc2.AppendChild(xRootElem);
            xRootElem.InnerXml = xdoc.DocumentElement.InnerXml;
            if (path == string.Empty)
            {
                using (SaveFileDialog SFD = new SaveFileDialog() { Filter = "XML files | *.xml" })
                {
                    if (SFD.ShowDialog() == DialogResult.OK)
                    {
                        path = SFD.FileName;
                        xdoc2.Save(path);
                    }
                }
            }
            else
            {
                xdoc2.Save(path);
            }
            LoadedPlaylist = path;
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            using (OpenFileDialog OFD = new OpenFileDialog() { Filter = "XML files | *.xml", Multiselect = false })
            {
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    //Stream plstr = null;
                    try
                    {
                        //plstr = new FileStream(OFD.FileName, FileMode.Open);
                        //ValidatePlaylist(plstr);
                        //plstr.Close();
                        xdoc.Load(OFD.FileName);
                        string[] Tracks = new string[xdoc.DocumentElement.ChildNodes.Count];
                        for (int i = 0; i < Tracks.Length; i++)
                        {
                            Tracks[i] = xdoc.DocumentElement.ChildNodes[i].FirstChild.InnerText;
                        }
                        InsertTracks(Tracks);
                        LoadedPlaylist = OFD.FileName;
                        UpdatePlaylistCounters();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Playlist file seems to be corrupted." + Environment.NewLine + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        //plstr?.Close();
                    }
                }
            }
        }

        private void ValidatePlaylist(Stream s)
        {
            XmlSchemaSet xSet = new XmlSchemaSet();

            System.Xml.Linq.XDocument xDocument = System.Xml.Linq.XDocument.Load(s);
            var ValidationFile = System.Configuration.ConfigurationManager.AppSettings["PLSchemaFile"];
            xSet.Add("", System.Configuration.ConfigurationManager.AppSettings["PLSchemaFile"]);
            xDocument.Validate(xSet, ValidationEventHandler);
        }

        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            XmlSeverityType type = XmlSeverityType.Warning;
            if (Enum.TryParse("Error", out type))
            {
                if (type == XmlSeverityType.Error) throw new Exception(e.Message);
            }
        }

        private void YAMPlaylistDialog_Load(object sender, EventArgs e)
        {
            SubscribeToCoreTrackChanged();
            EnsureWatchDirectoriesFromPlaylist();
            PerformDiskScanAndUpdatePanel();
            UpdateCurrentPlayingRowStyle();
            UpdatePlaybackMenuLabels();
            UpdatePlaylistCounters();
        }

        public static bool LoadPlaylistFromPath(string path, bool clearExisting = true)
        {
            if (string.IsNullOrWhiteSpace(path) || File.Exists(path) == false)
                return false;

            try
            {
                var xdoc = new System.Xml.XmlDocument();
                xdoc.Load(path);

                if (clearExisting)
                {
                    YAMPVars.TrackList.Clear();
                    YAMPVars.PendingQueue.Clear();
                }

                if (xdoc.DocumentElement == null)
                    return false;

                foreach (System.Xml.XmlNode node in xdoc.DocumentElement.ChildNodes)
                {
                    string filePath = node != null && node.FirstChild != null ? node.FirstChild.InnerText : string.Empty;
                    if (string.IsNullOrWhiteSpace(filePath) || File.Exists(filePath) == false)
                        continue;

                    if (!AudioFileSupport.IsSupportedAudioFile(filePath))
                        continue;

                    bool exists = YAMPVars.TrackList.Any(t => t != null && string.Equals(t.Path, filePath, StringComparison.OrdinalIgnoreCase));
                    if (!exists)
                    {
                        YAMPVars.TrackList.Add(new TrackInfo(filePath));
                    }
                }

                YAMPVars.LoadedPlaylist = path;

                foreach (Form form in Application.OpenForms)
                {
                    YAMPlaylistDialog playlistForm = form as YAMPlaylistDialog;
                    if (playlistForm != null && !playlistForm.IsDisposed)
                    {
                        PlaylistSource?.ResetBindings(false);
                        playlistForm.EnsureWatchDirectoriesFromPlaylist();
                        playlistForm.PerformDiskScanAndUpdatePanel();
                        playlistForm.UpdatePlaylistCounters();
                        playlistForm.UpdateCurrentPlayingRowStyle();
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void SubscribeToCoreTrackChanged()
        {
            if (_coreTrackChangedSubscribed || YAMPVars.CORE == null)
                return;

            YAMPVars.CORE.TrackChanged += Core_TrackChanged;
            _coreTrackChangedSubscribed = true;
        }

        private void UnsubscribeFromCoreTrackChanged()
        {
            if (!_coreTrackChangedSubscribed || YAMPVars.CORE == null)
                return;

            YAMPVars.CORE.TrackChanged -= Core_TrackChanged;
            _coreTrackChangedSubscribed = false;
        }

        private void Core_TrackChanged(object sender, EventArgs e)
        {
            if (IsDisposed)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() =>
                {
                    UpdateCurrentPlayingRowStyle();
                    UpdatePlaylistCounters();
                }));
            }
            else
            {
                UpdateCurrentPlayingRowStyle();
                UpdatePlaylistCounters();
            }
        }

        private void YAMPlaylistDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            UnsubscribeFromCoreTrackChanged();
            DisposeWatchers();
        }

        private void YAMPlaylistDialog_Deactivate(object sender, EventArgs e)
        {
            HideAuxPanels();
        }

        private async void oneDriveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string SaveTo = System.Configuration.ConfigurationManager.AppSettings["CloudDownloadLocation"];
            //List<string> DownloadedTracks = new List<string>();
            //if (YAMPVars.OneDriveApi != null && YAMPVars.OneDriveApi.AccessTokenValidUntil > DateTime.Now)
            //{
            //    var RootItem = await YAMPVars.OneDriveApi.GetDriveRoot();
            //    //var FolderFacet = RootItem.Folder;
            //    foreach (var item in await YAMPVars.OneDriveApi.GetAllChildrenByParentItem(RootItem))
            //    {
            //        var AudioFacet = item.Audio;
            //        if (AudioFacet != null)
            //        {
            //            bool isDownloaded = await YAMPVars.OneDriveApi.DownloadItem(item, SaveTo);
            //            if (isDownloaded)
            //            {
            //                DownloadedTracks.Add(SaveTo + item.Name);
            //            }
            //        }
            //    }
            //    if (DownloadedTracks.Count > 0)
            //    {
            //        InsertTracks(DownloadedTracks.ToArray());
            //    }
            //}
        }
    }

    public class TrackSelectedEventArgs : EventArgs
    {
        public TrackSelectedEventArgs(TrackInfo track)
        {
            Track = track;
        }

        public TrackInfo Track { get; private set; }
    }
}
