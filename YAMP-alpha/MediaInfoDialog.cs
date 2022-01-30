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
    public partial class MediaInfoDialog : Form
    {
        public MediaInfoDialog(string TrackPath)
        {
            InitializeComponent();
            
            MediaInfo.MediaInfoWrapper minfo = new MediaInfo.MediaInfoWrapper(TrackPath);
            textBox1.Text = minfo.Text.TrimEnd('\n', ' ');
        }
    }
}
