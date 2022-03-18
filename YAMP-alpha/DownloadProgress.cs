using System;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class DownloadProgress : Form
    {
        public int Percent { get { return progressBar1.Value; } set { progressBar1.Value = value; OnPercentChanged(); } }

        public DownloadProgress()
        {
            InitializeComponent();
            Percent = 0;
            PercentChanged += DownloadProgress_PercentChanged;
        }

        private void DownloadProgress_PercentChanged(object sender, EventArgs e)
        {
            
        }

        public event EventHandler PercentChanged;
        private void OnPercentChanged()
        {
            PercentChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
