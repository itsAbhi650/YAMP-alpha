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
    public partial class PanSlider : Form
    {
        public PanSlider()
        {
            InitializeComponent();
            ShowInTaskbar = false;
            Deactivate += PanSlider_Deactivate;
            Location = Cursor.Position;
        }

        private void PanSlider_Deactivate(object sender, EventArgs e)
        {
            Close();
        }

        private void Tb_PanSlider_ValueChanged(object sender, EventArgs e)
        {
            if (YAMPVars.CORE.Player != null)
            {
                YAMPVars.ChannelPan.Pan = (Tb_PanSlider.Value / (float)Tb_PanSlider.Maximum);
            }
        }

        private void PanSlider_Load(object sender, EventArgs e)
        {
            Tb_PanSlider.Value = (int)YAMPVars.ChannelPan.Pan * 10;
        }
    }
}
