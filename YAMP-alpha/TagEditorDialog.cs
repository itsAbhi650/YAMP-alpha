using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using YAMP_alpha.Controls;

namespace YAMP_alpha
{
    public partial class TagEditorDialog : Form
    {
        private TagLib.File file;
        private TagEditorCapabilities _capabilities = TagEditorCapabilities.None;
        private string LoadedTrackFile = "";

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
            else
            {
                ApplyCapabilities();
            }
        }

        private void LoadFile(string Path)
        {
            tbpgCoverArt.Controls.Clear();
            LoadedTrackFile = Path;
            file?.Dispose();
            file = null;
            _capabilities = TagEditorCapabilities.None;

            try
            {
                file = TagLib.File.Create(LoadedTrackFile);
                _capabilities = TagEditorCapabilities.FromTagTypes(file.TagTypes);
                LoadTagInformation(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Tags for this file type could not be loaded.{0}{0}{1}", Environment.NewLine, ex.Message),
                    "Unsupported Tags",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            ApplyCapabilities();
        }

        private void LoadTagInformation(TagLib.File loadedFile)
        {
            tbTitle.Text = loadedFile.Tag.Title;
            tbArtist.Text = loadedFile.Tag.JoinedPerformers;
            tbAlbum.Text = loadedFile.Tag.Album;
            tbAlbumArtists.Text = loadedFile.Tag.JoinedAlbumArtists;
            tbComposers.Text = loadedFile.Tag.JoinedComposers;
            tbConductor.Text = loadedFile.Tag.Conductor;
            tbGenres.Text = loadedFile.Tag.JoinedGenres;
            tbGrouping.Text = loadedFile.Tag.Grouping;
            tbISRC.Text = loadedFile.Tag.ISRC;
            tbPublisher.Text = loadedFile.Tag.Publisher;
            tbInitKey.Text = loadedFile.Tag.InitialKey;
            tbComments.Text = loadedFile.Tag.Comment;
            tbLyrics.Text = loadedFile.Tag.Lyrics ?? string.Empty;
            tbBPM.Text = ToEditorNumber(loadedFile.Tag.BeatsPerMinute);
            tbDate.Text = ToEditorNumber(loadedFile.Tag.Year);
            tbDisk.Text = ToEditorNumber(loadedFile.Tag.Disc);
            tbTrack.Text = ToEditorNumber(loadedFile.Tag.Track);
            tbCopyright.Text = loadedFile.Tag.Copyright;

            foreach (TagLib.IPicture pic in loadedFile.Tag.Pictures)
            {
                CoverArtDetailPanel ArtPanel = new CoverArtDetailPanel()
                {
                    CoverType = pic.Type,
                    Description = pic.Description,
                    OriginalPicture = pic,
                    Dock = DockStyle.Top
                };
                ArtPanel.CoverImageBox.Image = CreateImageFromBytes(pic.Data.Data);
                ArtPanel.CoverDoubleClick += ArtPanel_CoverDoubleClick;
                tbpgCoverArt.Controls.Add(ArtPanel);
            }
        }

        private void ArtPanel_CoverDoubleClick(object sender, EventArgs e)
        {
            Image image = sender as Image;
            if (image != null)
            {
                new BigArt(image).ShowDialog();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveTags();
                MessageBox.Show("Tags saved.", "Tag Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cannot save tags.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveTags()
        {
            if (file == null)
                throw new InvalidOperationException("No tag file is loaded.");

            if (!file.Writeable)
                throw new InvalidOperationException(string.Join(Environment.NewLine, file.CorruptionReasons));

            if (_capabilities.Basic)
            {
                file.Tag.Title = tbTitle.Text;
                file.Tag.Performers = SplitMultiValue(tbArtist.Text);
                file.Tag.Album = tbAlbum.Text;
                file.Tag.AlbumArtists = SplitMultiValue(tbAlbumArtists.Text);
                file.Tag.Genres = SplitMultiValue(tbGenres.Text);
                file.Tag.Year = ParseOptionalUInt(tbDate.Text, "Year");
                file.Tag.Track = ParseOptionalUInt(tbTrack.Text, "Track");
                file.Tag.Disc = ParseOptionalUInt(tbDisk.Text, "Disk");
                file.Tag.Comment = tbComments.Text;
            }

            if (_capabilities.ExtendedCredits)
            {
                file.Tag.Composers = SplitMultiValue(tbComposers.Text);
                file.Tag.Conductor = tbConductor.Text;
            }

            if (_capabilities.AdvancedIds)
            {
                file.Tag.Grouping = tbGrouping.Text;
                file.Tag.ISRC = tbISRC.Text;
                file.Tag.Publisher = tbPublisher.Text;
                file.Tag.InitialKey = tbInitKey.Text;
                file.Tag.BeatsPerMinute = ParseOptionalUInt(tbBPM.Text, "BPM");
                file.Tag.Copyright = tbCopyright.Text;
            }

            if (_capabilities.Lyrics)
            {
                file.Tag.Lyrics = tbLyrics.Text;
            }

            if (_capabilities.Pictures)
            {
                file.Tag.Pictures = tbpgCoverArt.Controls
                    .OfType<CoverArtDetailPanel>()
                    .Select(x => x.ToPicture())
                    .Where(x => x != null)
                    .ToArray();
            }

            file.Save();
        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OFD = new OpenFileDialog() { Filter = AudioFileSupport.OpenFileFilter })
            {
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    LoadFile(OFD.FileName);
                }
            }
        }

        private void ApplyCapabilities()
        {
            SetEnabled(_capabilities.Basic, tbTitle, tbArtist, tbAlbum, tbAlbumArtists, tbGenres, tbDate, tbTrack, tbDisk, tbComments);
            SetEnabled(_capabilities.ExtendedCredits, tbComposers, tbConductor);
            SetEnabled(_capabilities.AdvancedIds, tbGrouping, tbISRC, tbPublisher, tbInitKey, tbBPM, tbCopyright);
            tbLyrics.Enabled = _capabilities.Lyrics;
            tbpgCoverArt.Enabled = _capabilities.Pictures;
            btnSave.Enabled = file != null && file.Writeable && !_capabilities.IsEmpty;
            Text = file == null
                ? "TagEditorDialog"
                : string.Format("TagEditorDialog - {0} ({1})", Path.GetFileName(LoadedTrackFile), file.TagTypes);
        }

        private static void SetEnabled(bool enabled, params Control[] controls)
        {
            foreach (Control control in controls)
            {
                control.Enabled = enabled;
            }
        }

        private static string[] SplitMultiValue(string text)
        {
            return (text ?? string.Empty)
                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => x.Length > 0)
                .ToArray();
        }

        private static uint ParseOptionalUInt(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            uint result;
            if (uint.TryParse(value, out result))
                return result;

            throw new FormatException(fieldName + " must be a whole number.");
        }

        private static string ToEditorNumber(uint value)
        {
            return value == 0 ? string.Empty : value.ToString();
        }

        private static Image CreateImageFromBytes(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            try
            {
                using (MemoryStream stream = new MemoryStream(imageData))
                {
                    using (Image temp = Image.FromStream(stream))
                    {
                        return new Bitmap(temp);
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        private class TagEditorCapabilities
        {
            public static readonly TagEditorCapabilities None = new TagEditorCapabilities();

            public bool Basic { get; private set; }
            public bool ExtendedCredits { get; private set; }
            public bool AdvancedIds { get; private set; }
            public bool Lyrics { get; private set; }
            public bool Pictures { get; private set; }
            public bool IsEmpty { get { return !Basic && !ExtendedCredits && !AdvancedIds && !Lyrics && !Pictures; } }

            public static TagEditorCapabilities FromTagTypes(TagLib.TagTypes tagTypes)
            {
                bool rich =
                    tagTypes.HasFlag(TagLib.TagTypes.Id3v2) ||
                    tagTypes.HasFlag(TagLib.TagTypes.Xiph) ||
                    tagTypes.HasFlag(TagLib.TagTypes.Apple) ||
                    tagTypes.HasFlag(TagLib.TagTypes.Asf) ||
                    tagTypes.HasFlag(TagLib.TagTypes.FlacMetadata) ||
                    tagTypes.HasFlag(TagLib.TagTypes.Matroska);

                bool limited =
                    tagTypes.HasFlag(TagLib.TagTypes.Id3v1) ||
                    tagTypes.HasFlag(TagLib.TagTypes.RiffInfo);

                return new TagEditorCapabilities
                {
                    Basic = rich || limited,
                    ExtendedCredits = rich,
                    AdvancedIds = rich && !tagTypes.HasFlag(TagLib.TagTypes.RiffInfo),
                    Lyrics = rich,
                    Pictures = rich || tagTypes.HasFlag(TagLib.TagTypes.FlacMetadata)
                };
            }
        }
    }
}
