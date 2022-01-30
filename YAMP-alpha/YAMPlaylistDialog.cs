using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class YAMPlaylistDialog : Form
    {
        DataTable PlayListTable = new DataTable();

        public YAMPlaylistDialog()
        {
            InitializeComponent();
            DataGridViewTextBoxCell DTCell = new DataGridViewTextBoxCell();
            
            DataGridViewColumn DGVTC = new DataGridViewColumn()
            {
                HeaderText = "Title",
                Name = "clm_Title",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet,
                ValueType = typeof(string),
                CellTemplate = DTCell
            };
            DataGridViewColumn DGVDC = new DataGridViewColumn()
            {
                HeaderText = "Duration",
                Name = "clm_Duration",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet,
                ValueType = typeof(TimeSpan),
                CellTemplate = DTCell
            };

            DataGridViewColumn DGVFPC = new DataGridViewColumn()
            {
                Name = "clm_FilePath",
                HeaderText = "Path",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet,
                Visible = false,
                CellTemplate = DTCell,
                ValueType = typeof(string)
            };

            DataGridViewButtonColumn DGVBC = new DataGridViewButtonColumn()
            {
                HeaderText = "Info",
                Text = "i",
                UseColumnTextForButtonValue = true,
                Name = "clm_MediaInfo",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader,            
            };


            DGVBC.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns.Add(DGVTC);
            dataGridView1.Columns.Add(DGVDC);
            dataGridView1.Columns.Add(DGVFPC);
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

            if (YAMPVars.CORE.PlayerSource != null)
            {
                TrackInfo tinfo = new TrackInfo(YAMPVars.CORE.PlayingFile);
                dataGridView1.Rows.Add(tinfo._title, tinfo._length, tinfo.Info.FullName);
            }
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

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                MediaInfoDialog MediaInfo = new MediaInfoDialog((string)dataGridView1[2, e.RowIndex].Value);
                MediaInfo.ShowDialog();
            }
            //var btnObj = dataGridView1[e.ColumnIndex, e.RowIndex].Value;
        }
    }
}
