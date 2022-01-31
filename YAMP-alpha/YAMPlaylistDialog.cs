using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class YAMPlaylistDialog : Form
    {
        //ObservableCollection<TrackInfo> trackInfo = YAMPVars.TrackList;

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
            if (YAMPVars.CORE.PlayerSource != null)
            {
                var source = new BindingSource
                {
                    DataSource = YAMPVars.TrackList
                };
                
                dataGridView1.DataSource = source;
                //TrackInfo tinfo = new TrackInfo(YAMPVars.CORE.PlayingFile);
            }
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
                if (item.HeaderText != "Title" && item.HeaderText != "Length" && item.HeaderText != "Info")
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
            using (OpenFileDialog OFD = new OpenFileDialog() { Filter = "mp3 files (*.mp3)|*.mp3" })
            {
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    TrackInfo track = new TrackInfo(OFD.FileName);
                    if (!TrackExist(track))
                    {
                        ((BindingSource)dataGridView1.DataSource).Add(track);
                        
                    }
                    else
                    {
                        MessageBox.Show("Already Exist!");
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_NewRowNeeded(object sender, DataGridViewRowEventArgs e)
        {

        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            dataGridView1.Invalidate();
        }
    }
}
