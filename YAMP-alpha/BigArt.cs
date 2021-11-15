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
    public partial class BigArt : Form
    {
        private bool dragging;

        public Point StartPoint { get; private set; }
        internal Image AlbumArt { get; set; } = null;
        public BigArt()
        {
            InitializeComponent();
        }

        private void BigArt_Load(object sender, EventArgs e)
        {
            //var ArtSize = AlbumArt.Size;
            Size ArtSize = AlbumArt.Size.Height > 600 ? new Size(600, 600) : AlbumArt.Size;
            Bitmap bmp = new Bitmap(AlbumArt, ArtSize);
            pictureBox1.Size = ArtSize;
            pictureBox1.Image = bmp;
            Size = ArtSize;
            //Text = "H: " + Height.ToString() + " W: " + Width.ToString();
        }

        private void BigArt_SizeChanged(object sender, EventArgs e)
        {
            //Text = "H: " + Height.ToString() + " W: " + Width.ToString();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            StartPoint = e.Location;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - StartPoint.X, p.Y - StartPoint.Y);
            }
        }
    }
}
