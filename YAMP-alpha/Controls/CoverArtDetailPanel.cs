using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace YAMP_alpha.Controls
{
    public partial class CoverArtDetailPanel : UserControl
    {
        public string LoadedImagePath;


        public CoverArtDetailPanel()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(Enum.GetNames(typeof(TagLib.PictureType)).OrderBy(x => x).ToArray());

            comboBox1.SelectedItem = "Media";
        }

        [Category("Data"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PictureBox CoverImageBox
        {
            get { return CoverArtBox; }
            set { CoverArtBox = value; }
        }
        [Category("Data"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TagLib.PictureType CoverType
        {
            get { return (TagLib.PictureType)Enum.Parse(typeof(TagLib.PictureType), comboBox1.SelectedItem.ToString()); }
            set { comboBox1.SelectedItem = value.ToString(); }
        }

        [Category("Data"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string Description { get { return tbPicDesc.Text; } set { tbPicDesc.Text = value; } }

        [Category("Action"), Description("Occurs when cover image is double clicked.")]
        public event EventHandler CoverDoubleClick;
        private void CoverArtBox_DoubleClick(object sender, EventArgs e)
        {
            CoverDoubleClick.Invoke(CoverImageBox.Image, null);
        }

        private void browseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OFD = new OpenFileDialog() { Filter = "Image Files | *.jpg; *.png" })
            {
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    CoverArtBox.Image = Image.FromFile(OFD.FileName);
                    LoadedImagePath = OFD.FileName;
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog SFD = new SaveFileDialog()
            {
                Filter = "Bitmap Files|*.bmp|" + "JPEG Files|*.jpg|" + "PNG Files|*.png",
                AddExtension = true
            })
            {
                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    string ext = new FileInfo(SFD.FileName).Extension;
                    switch (ext)
                    {
                        case ".bmp":
                            CoverArtBox.Image?.Save(SFD.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                        case ".jpg":
                            CoverArtBox.Image?.Save(SFD.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;
                        case ".png":
                            CoverArtBox.Image?.Save(SFD.FileName, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                    }
                }
            }
        }
    }
}
