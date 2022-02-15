using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;

namespace YAMP_alpha
{
    public partial class YAMPlaylistDialog : Form
    {
        //ObservableCollection<TrackInfo> trackInfo = YAMPVars.TrackList;
        BindingSource PlaylistSource;
        public YAMPlaylistDialog()
        {
            InitializeComponent();
            //DataGridViewTextBoxCell DTCell = new DataGridViewTextBoxCell();

            //DataGridViewColumn DGVTC = new DataGridViewColumn()
            //{
            //    HeaderText = "Title",
            //    Name = "clm_Title",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet,
            //    ValueType = typeof(string),
            //    CellTemplate = DTCell
            //};
            //DataGridViewColumn DGVDC = new DataGridViewColumn()
            //{
            //    HeaderText = "Duration",
            //    Name = "clm_Duration",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet,
            //    ValueType = typeof(TimeSpan),
            //    CellTemplate = DTCell
            //};

            //DataGridViewColumn DGVFPC = new DataGridViewColumn()
            //{
            //    Name = "clm_FilePath",
            //    HeaderText = "Path",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet,
            //    Visible = false,
            //    CellTemplate = DTCell,
            //    ValueType = typeof(string)
            //};
            //dataGridView1.columns
            //if (YAMPVars.CORE.PlayerSource != null)
            //{
            PlaylistSource = new BindingSource
            {
                DataSource = YAMPVars.TrackList
            };
            dataGridView1.DataSource = PlaylistSource;
            dataGridView1.Columns["Duration"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            
            DataGridViewButtonColumn DGVBC = new DataGridViewButtonColumn()
            {
                HeaderText = "Info",
                Text = "i",
                UseColumnTextForButtonValue = true,
                Name = "clm_MediaInfo",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader,
            };

            foreach (DataGridViewColumn item in dataGridView1.Columns)
            {
                if (item.HeaderText != "Title" && item.HeaderText != "Duration" && item.HeaderText != "Info")
                {
                    item.Visible = false;
                }
            }

            DGVBC.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dataGridView1.Columns.Add(DGVTC);
            //dataGridView1.Columns.Add(DGVDC);
            //dataGridView1.Columns.Add(DGVFPC);
            dataGridView1.Columns.Add(DGVBC);
        }

        private void Btn_info_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Clicked");
        }

        private void YAMPlaylistDialog_Load(object sender, EventArgs e)
        {
            //MediaInfo.MediaInfoWrapper minfo = new MediaInfo.MediaInfoWrapper(YAMPVars.CORE.PlayingFile);
            //var bestStream = minfo.BestAudioStream;
            //var CovArr = minfo.Tags.Covers.ToArray();
            //var CoverArt = CovArr[0].Data;

            //Image img = Image.FromStream(new MemoryStream(CoverArt));
            //pictureBox1.Image = img;
            //dataGridView1.ClearSelection();
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("Fired");

        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OFD = new OpenFileDialog() { Filter = "mp3 files (*.mp3)|*.mp3", Multiselect = true })
            {
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    foreach (string TrackName in OFD.FileNames)
                    {
                        TrackInfo track = new TrackInfo(TrackName);
                        if (!TrackExist(track))
                        {
                            PlaylistSource.Add(track);
                        }
                        else
                        {
                            MessageBox.Show(string.Format("{0} Already Exist!",track.Title));
                        }
                    }

                }
            }
        }

        private bool TrackExist(TrackInfo track)
        {
            bool exist = false;
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if ((string)item.Cells["Path"].Value == track.File.FullName)
                {
                    exist = true;
                    break;
                }
            }
            return exist;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1[e.ColumnIndex, e.RowIndex].OwningColumn.GetType() == typeof(DataGridViewButtonColumn))
            {
                MediaInfoDialog MediaInfo = new MediaInfoDialog(YAMPVars.TrackList[e.RowIndex].File.FullName);
                MediaInfo.ShowDialog();
            }
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged == DataGridViewElementStates.Selected)
            {
                pictureBox1.Image = YAMPVars.TrackList[e.Row.Index].Covers[0];
            }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            BigArt bigart = new BigArt()
            {
                AlbumArt = pictureBox1.Image
            };
            bigart.ShowDialog();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            dataGridView1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int SelectedRowIndex = dataGridView1.CurrentRow.Index;
            int TotalTrackZ = PlaylistSource.Count - 1;
            int NewTrackIndex = dataGridView1.CurrentRow.Index + int.Parse(((Button)sender).Tag.ToString());
            if (NewTrackIndex <= TotalTrackZ && NewTrackIndex >= 0)
            {
                TrackInfo temptrack = PlaylistSource[NewTrackIndex] as TrackInfo;
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
            if (folderBrowserDialog1.ShowDialog()== DialogResult.OK)
            {
                DirectoryInfo dir = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
                FileInfo[] Files = dir.GetFiles();
                foreach (FileInfo item in Files)
                {
                    PlaylistSource.Add(new TrackInfo(item.FullName));
                }
            }
            
        }

        private void dataGridView1_DragOver(object sender, DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(DataFormats.FileDrop) & e.Effect == DragDropEffects.Move)
            //{
            //    e.Effect = DragDropEffects.Link;
            //}
        }

        private void dataGridView1_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            //if (e.Effect == DragDropEffects.Move)
            //{

            //}
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            //FileInfo[] DroppedFiles = ((string[])e.Data.GetData(DataFormats.FileDrop)).Select(x => new FileInfo(x)).ToArray();
            //foreach (FileInfo File in DroppedFiles)
            //{
            //    TrackInfo Track = new TrackInfo(File.FullName);
            //    PlaylistSource.Add(Track);
            //}
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            YAMPVars.CORE.UIRef.PlayfromPlaylist(YAMPVars.TrackList[e.RowIndex]);
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
    }
}
