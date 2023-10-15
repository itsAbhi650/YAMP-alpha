using KoenZomers.OneDrive.Api.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Schema;

namespace YAMP_alpha
{
    public partial class YAMPlaylistDialog : Form
    {
        static BindingSource PlaylistSource;
        private string _loadedPlaylist = "";
        private int CurrentColoredRowIndex;

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

        public YAMPlaylistDialog()
        {
            InitializeComponent();
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

            DataGridViewButtonColumn DGVBC = new DataGridViewButtonColumn()
            {
                HeaderText = "Info",
                Text = "i",
                UseColumnTextForButtonValue = true,
                Name = "clm_MediaInfo",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
            };
            dataGridView1.Columns.Add(DGVBC);


            foreach (DataGridViewColumn item in dataGridView1.Columns)
            {
                if (item.HeaderText != "Title" && item.HeaderText != "Duration" && item.HeaderText != "Info")
                {
                    item.Visible = false;
                }
            }

            DGVBC.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void Btn_info_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Clicked");
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OFD = new OpenFileDialog() { Filter = "mp3 files (*.mp3)|*.mp3|m4a files (*.m4a)|*.m4a", Multiselect = true })
            {
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    InsertTracks(OFD.FileNames);
                }
            }
        }

        private void InsertTracks(string[] Tracks)
        {
            foreach (string TrackName in Tracks)
            {
                TrackInfo track = new TrackInfo(TrackName);
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
                .Any(row => (string)row.Cells["path"].Value == track.File.FullName);
            return Exst;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1[e.ColumnIndex, e.RowIndex].OwningColumn.GetType() == typeof(DataGridViewButtonColumn))
            {
                MediaInfoDialog MediaInfo = new MediaInfoDialog(YAMPVars.TrackList[e.RowIndex].File.FullName);
                MediaInfo.ShowDialog();
            }
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged == DataGridViewElementStates.Selected)
            {
                pictureBox1.Image = null;
                if (YAMPVars.TrackList[e.Row.Index].Covers.Count > 0)
                {
                    pictureBox1.Image = YAMPVars.TrackList[e.Row.Index].Covers[0];
                }
            }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            new BigArt(pictureBox1.Image).ShowDialog();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            dataGridView1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int SelectedRowIndex = dataGridView1.CurrentRow.Index;
            int NewTrackIndex = SelectedRowIndex + Convert.ToInt32(((Button)sender).Tag);
            if (NewTrackIndex >= 0 && NewTrackIndex <= PlaylistSource.Count - 1)
            {
                var temptrack = PlaylistSource[NewTrackIndex] as TrackInfo;
                PlaylistSource[NewTrackIndex] = PlaylistSource[SelectedRowIndex];
                PlaylistSource[SelectedRowIndex] = temptrack;
                dataGridView1.CurrentCell = dataGridView1[0, NewTrackIndex];
            }
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
        }

        private void directoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDirectory();
        }

        public static void LoadDirectory()
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo dir = new DirectoryInfo(FBD.SelectedPath);
                FileInfo[] Files = dir.GetFiles().Where(x => x.Extension == ".mp3").ToArray();
                foreach (FileInfo item in Files)
                {
                    try
                    {
                        TrackInfo Track = new TrackInfo(item.FullName);
                        PlaylistSource.Add(Track);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                YAMPVars.CORE.UIRef.PlayfromPlaylist(YAMPVars.TrackList[e.RowIndex]);
                UpdateCurrentPlayingRowStyle();
            }
        }

        private void UpdateCurrentPlayingRowStyle()
        {
            int CPR = GetCurrentPlayingRowIndex();
            if (CPR >= 0)
            {
                dataGridView1.Rows[CurrentColoredRowIndex].DefaultCellStyle = new DataGridViewCellStyle();
                dataGridView1.Rows[CPR].DefaultCellStyle.BackColor = Color.Blue;
                dataGridView1.Rows[CPR].DefaultCellStyle.ForeColor = Color.White;
                CurrentColoredRowIndex = CPR;
            }
        }

        private int GetCurrentPlayingRowIndex()
        {
            int CurrentPlayingIndex = -1;
            if (YAMPVars.CORE != null && YAMPVars.CORE.CurrentTrack != null)
            {
                foreach (DataGridViewRow item in dataGridView1.Rows)
                {
                    if (item.Cells["Path"].EditedFormattedValue.ToString() == YAMPVars.CORE.CurrentTrack.Path)
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
                TrackInfo Track = new TrackInfo(File.FullName);
                PlaylistSource.Add(Track);
                label1_DragLeave(sender, e);
            }
        }

        private void delselectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var CurrentowIndex = dataGridView1.CurrentRow.Index;
            PlaylistSource.RemoveAt(CurrentowIndex);
            dataGridView1.ClearSelection();
            if (PlaylistSource.Count > 0)
            {
                if (CurrentowIndex >= PlaylistSource.Count - 1)
                {
                    dataGridView1.CurrentCell = dataGridView1[0, PlaylistSource.Count - 1];
                }
                else
                {
                    dataGridView1.CurrentCell = dataGridView1[0, CurrentowIndex];
                }
                dataGridView1.CurrentCell.Selected = true;
            }
            else
            {
                pictureBox1.Image = null;
            }
            dataGridView1.RefreshEdit();
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
            for (int i = 1; i < dataGridView1.Columns.Count; i++)
            {
                string colHeadText = dataGridView1.Columns[i].HeaderText;
                colHeadText = System.Text.RegularExpressions.Regex.Replace(colHeadText, "[-/, ]", "_");
                dt.Columns.Add(colHeadText);
            }
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                DataRow dtrow = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string colName = dt.Columns[i].ColumnName;
                    dtrow[colName] = item.Cells[colName].EditedFormattedValue.ToString();
                }
                dt.Rows.Add(dtrow);
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
                    try
                    {
                        Stream plstr = new FileStream(OFD.FileName, FileMode.Open);
                        ValidatePlaylist(plstr);
                        ((FileStream)plstr).Close();
                        xdoc.Load(OFD.FileName);
                        string[] Tracks = new string[xdoc.DocumentElement.ChildNodes.Count];
                        for (int i = 0; i < Tracks.Length; i++)
                        {
                            Tracks[i] = xdoc.DocumentElement.ChildNodes[i].FirstChild.InnerText;
                        }
                        InsertTracks(Tracks);
                        LoadedPlaylist = OFD.FileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Playlist file seems to be corrupted." + Environment.NewLine + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ValidatePlaylistFile(string Filename)
        {
            ValidatePlaylist(new FileStream(Filename, FileMode.Open));
        }

        private void ValidatePlaylist(Stream s)
        {
            XmlSchemaSet xSet = new XmlSchemaSet();

            System.Xml.Linq.XDocument xDocument = System.Xml.Linq.XDocument.Load(s);
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
            UpdateCurrentPlayingRowStyle();
        }

        private async void oneDriveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SaveTo = System.Configuration.ConfigurationManager.AppSettings["CloudDownloadLocation"];
            List<string> DownloadedTracks = new List<string>();
            if (YAMPVars.OneDriveApi != null && YAMPVars.OneDriveApi.AccessTokenValidUntil > DateTime.Now)
            {
                var RootItem = await YAMPVars.OneDriveApi.GetDriveRoot();
                //var FolderFacet = RootItem.Folder;
                foreach (var item in await YAMPVars.OneDriveApi.GetAllChildrenByParentItem(RootItem))
                {
                    var AudioFacet = item.Audio;
                    if (AudioFacet != null)
                    {
                        bool isDownloaded = await YAMPVars.OneDriveApi.DownloadItem(item, SaveTo);
                        if (isDownloaded)
                        {
                            DownloadedTracks.Add(SaveTo + item.Name);
                        }
                    }
                }
                if (DownloadedTracks.Count > 0)
                {
                    InsertTracks(DownloadedTracks.ToArray());
                }
            }
        }
    }
}
