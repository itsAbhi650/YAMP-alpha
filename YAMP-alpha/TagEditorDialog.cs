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
using YAMP_alpha.Controls;

namespace YAMP_alpha
{
    public partial class TagEditorDialog : Form
    {
        TagLib.File file;

        string LoadedTrackFile = "";
        /// <summary>
        /// Initialize TaglibDialog with a file.
        /// </summary>
        /// <param name="Path">Path of file to be tagged.</param>
        public TagEditorDialog(string Path)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(Path))
            {
                LoadFile(Path);
            }
        }

        private void LoadFile(string Path)
        {
            tbpgCoverArt.Controls.Clear();
            LoadedTrackFile = Path;
            file = TagLib.File.Create(LoadedTrackFile);
            LoadTagInformation(file);
        }

        private void LoadTagInformation(TagLib.File file)
        {
            tbTitle.Text = file.Tag.Title;
            tbArtist.Text = file.Tag.JoinedPerformers;
            tbAlbum.Text = file.Tag.Album;
            tbAlbumArtists.Text = file.Tag.JoinedAlbumArtists;
            tbComposers.Text = file.Tag.JoinedComposers;
            tbConductor.Text = file.Tag.Conductor;
            tbGenres.Text = file.Tag.JoinedGenres;
            tbGrouping.Text = file.Tag.Grouping;
            tbISRC.Text = file.Tag.ISRC;
            tbPublisher.Text = file.Tag.Publisher;
            tbInitKey.Text = file.Tag.InitialKey;
            tbLyrics.Lines = @file.Tag.Lyrics?.Split(Environment.NewLine.ToCharArray());
            tbBPM.Text = file.Tag.BeatsPerMinute.ToString();
            tbDate.Text = file.Tag.Year.ToString();
            tbDisk.Text = file.Tag.DiscCount.ToString();
            tbTrack.Text = file.Tag.TrackCount.ToString();
            tbCopyright.Text = file.Tag.Copyright;

            foreach (TagLib.IPicture pic in file.Tag.Pictures)
            {
                CoverArtDetailPanel ArtPanel = new CoverArtDetailPanel()
                {
                    CoverType = pic.Type,
                    Description = pic.Description,
                    Dock = DockStyle.Top
                };
                ArtPanel.CoverImageBox.Image = Image.FromStream(new MemoryStream(pic.Data.Data));
                ArtPanel.CoverDoubleClick += ArtPanel_CoverDoubleClick;
                tbpgCoverArt.Controls.Add(ArtPanel);
            }
        }

        private void ArtPanel_CoverDoubleClick(object sender, EventArgs e)
        {
            new BigArt((Image)sender).ShowDialog();
            //throw new NotImplementedException();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            file.Tag.Title = tbTitle.Text;
            file.Tag.Performers = tbArtist.Text.Split(';');
            file.Tag.Album = tbAlbum.Text;
            file.Tag.AlbumArtists = tbAlbumArtists.Text.Split(';');
            file.Tag.Composers = tbComposers.Text.Split(';');
            file.Tag.Conductor = tbConductor.Text;
            file.Tag.Genres = tbGenres.Text.Split(';'); ;
            file.Tag.Grouping = tbGrouping.Text;
            file.Tag.ISRC = tbISRC.Text;
            file.Tag.Publisher = tbPublisher.Text;
            file.Tag.InitialKey = tbInitKey.Text;
            file.Tag.Lyrics = tbLyrics.Text;
            file.Tag.BeatsPerMinute = Convert.ToUInt32(tbBPM.Text);
            file.Tag.Year = Convert.ToUInt32(tbDate.Text);
            file.Tag.DiscCount = Convert.ToUInt32(tbDisk.Text);
            file.Tag.TrackCount = Convert.ToUInt32(tbTrack.Text);
            file.Tag.Copyright = tbCopyright.Text;

            file.Tag.Pictures = tbpgCoverArt.Controls
                .OfType<CoverArtDetailPanel>()
                .Select(x => new TagLib.Picture(x.LoadedImagePath))
                .ToArray();

            if (file.Writeable)
            {
                file.Save();
            }
            else
            {
                MessageBox.Show(string.Join(Environment.NewLine, file.CorruptionReasons), "Cannot write tags.", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OFD = new OpenFileDialog() { Filter = CSCore.Codecs.CodecFactory.SupportedFilesFilterEn })
            {
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    LoadFile(OFD.FileName);
                }
            }
        }
    }
}
