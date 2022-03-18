using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class StreamDialog : Form
    {
        public StreamDialog()
        {
            InitializeComponent();
        }

        public StreamDialog(float FileSize)
        {
            InitializeComponent();
            string SizeMessage = string.Format($"Estimated File size: {FileSize:n2} MBs. Download temporarily?");
            Lbl_FileInfo.Text = SizeMessage;
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
