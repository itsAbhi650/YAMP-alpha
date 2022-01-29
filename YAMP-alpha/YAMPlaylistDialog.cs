using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                ValueType = typeof(TimeSpan),
                CellTemplate = DTCell
            };
            dataGridView1.Columns.Add(DGVTC);
            dataGridView1.Columns.Add(DGVDC);
            if (YAMPVars.CORE.PlayerSource != null)
            {
                TrackInfo tinfo = new TrackInfo(YAMPVars.CORE.PlayingFile);
                dataGridView1.Rows.Add(tinfo._title, tinfo._length);
            }
           
        }

        private void YAMPlaylistDialog_Load(object sender, EventArgs e)
        {
            MediaInfo.MediaInfoWrapper minfo = new MediaInfo.MediaInfoWrapper(YAMPVars.CORE.PlayingFile);
            var bestStream = minfo.BestAudioStream;
            var CovArr = minfo.Tags.Covers.ToArray();
            var CoverArt = CovArr[0].Data;
            Image img = Image.FromStream(new MemoryStream(CoverArt));
            pictureBox1.Image = img;
            //dataGridView1.ClearSelection();
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("Fired");

        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            
        }
    }
}
