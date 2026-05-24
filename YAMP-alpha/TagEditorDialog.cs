using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using YAMP_alpha.Controls;

namespace YAMP_alpha
{
    public partial class TagEditorDialog : Form
    {
        private TagLib.File file;
        private readonly List<string> _loadedFiles = new List<string>();
        private bool _multiFileMode;
        private bool _isLoadingUi;
        private readonly HashSet<TagField> _dirtyFields = new HashSet<TagField>();
        private TagEditorCapabilities _capabilities = TagEditorCapabilities.None;
        private string LoadedTrackFile = "";

        /// <summary>
        /// Initialize TaglibDialog with a file.
        /// </summary>
        /// <param name="Path">Path of file to be tagged.</param>
        public TagEditorDialog(string Path)
        {
            InitializeComponent();
            HookDirtyTracking();
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
            _multiFileMode = false;
            _loadedFiles.Clear();
            _loadedFiles.Add(Path);
            _dirtyFields.Clear();
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

        private void LoadFiles(IEnumerable<string> paths)
        {
            var files = (paths ?? Enumerable.Empty<string>())
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            _dirtyFields.Clear();
            tbpgCoverArt.Controls.Clear();
            file?.Dispose();
            file = null;

            _loadedFiles.Clear();
            _loadedFiles.AddRange(files);
            _multiFileMode = _loadedFiles.Count > 1;

            if (_loadedFiles.Count == 0)
            {
                _capabilities = TagEditorCapabilities.None;
                ApplyCapabilities();
                return;
            }

            if (_loadedFiles.Count == 1)
            {
                LoadFile(_loadedFiles[0]);
                return;
            }

            try
            {
                var snapshots = _loadedFiles.Select(ReadSnapshot).ToList();
                _capabilities = TagEditorCapabilities.ForMultipleFiles(snapshots);
                LoadMultiFileInformation(snapshots);
            }
            catch (Exception ex)
            {
                _capabilities = TagEditorCapabilities.None;
                MessageBox.Show(ex.Message, "Cannot load files.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ApplyCapabilities();
        }

        private void LoadTagInformation(TagLib.File loadedFile)
        {
            _isLoadingUi = true;
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
            _isLoadingUi = false;

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

        private void LoadMultiFileInformation(List<TagSnapshot> snapshots)
        {
            _isLoadingUi = true;

            tbTitle.Text = CommonOrMixed(snapshots.Select(s => s.Title));
            tbArtist.Text = CommonOrMixed(snapshots.Select(s => s.PerformersJoined));
            tbAlbum.Text = CommonOrMixed(snapshots.Select(s => s.Album));
            tbAlbumArtists.Text = CommonOrMixed(snapshots.Select(s => s.AlbumArtistsJoined));
            tbComposers.Text = CommonOrMixed(snapshots.Select(s => s.ComposersJoined));
            tbConductor.Text = CommonOrMixed(snapshots.Select(s => s.Conductor));
            tbGenres.Text = CommonOrMixed(snapshots.Select(s => s.GenresJoined));
            tbGrouping.Text = CommonOrMixed(snapshots.Select(s => s.Grouping));
            tbISRC.Text = CommonOrMixed(snapshots.Select(s => s.Isrc));
            tbPublisher.Text = CommonOrMixed(snapshots.Select(s => s.Publisher));
            tbInitKey.Text = CommonOrMixed(snapshots.Select(s => s.InitialKey));
            tbComments.Text = CommonOrMixed(snapshots.Select(s => s.Comment));

            // In multi-file mode, lyrics and cover art are disabled by capabilities for safety.
            tbLyrics.Text = string.Empty;

            tbBPM.Text = CommonOrMixedNumber(snapshots.Select(s => s.BeatsPerMinute));
            tbDate.Text = CommonOrMixedNumber(snapshots.Select(s => s.Year));
            tbDisk.Text = CommonOrMixedNumber(snapshots.Select(s => s.Disc));
            tbTrack.Text = CommonOrMixedNumber(snapshots.Select(s => s.Track));
            tbCopyright.Text = CommonOrMixed(snapshots.Select(s => s.Copyright));

            _isLoadingUi = false;
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
            SaveReport report = _multiFileMode
                ? SaveTagsMultipleFiles()
                : SaveTagsSingleFile();

            ShowSaveReport(report);
            if (report.Failed == 0)
            {
                _dirtyFields.Clear();
                ApplyCapabilities();
            }
        }

        private SaveReport SaveTagsSingleFile()
        {
            SaveReport report = new SaveReport();

            if (file == null)
            {
                report.AddFailed(LoadedTrackFile, "No tag file is loaded.");
                return report;
            }

            if (!file.Writeable)
            {
                report.AddFailed(LoadedTrackFile, CorruptionReasonText(file));
                return report;
            }

            try
            {
                ValidateSingleFileInputs();

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
                report.AddSaved(LoadedTrackFile);
            }
            catch (Exception ex)
            {
                report.AddFailed(LoadedTrackFile, ex.Message);
            }

            return report;
        }

        private SaveReport SaveTagsMultipleFiles()
        {
            SaveReport report = new SaveReport();

            if (_loadedFiles.Count == 0)
            {
                report.AddFailed(string.Empty, "No files are loaded.");
                return report;
            }

            if (_dirtyFields.Count == 0)
            {
                report.AddSkipped(string.Empty, "No fields were changed.");
                return report;
            }

            try
            {
                ValidateDirtyInputs();
            }
            catch (Exception ex)
            {
                report.AddFailed(string.Empty, ex.Message);
                return report;
            }

            foreach (string path in _loadedFiles)
            {
                try
                {
                    using (var tagFile = TagLib.File.Create(path))
                    {
                        if (!tagFile.Writeable)
                        {
                            report.AddFailed(path, CorruptionReasonText(tagFile));
                            continue;
                        }

                        var caps = TagEditorCapabilities.FromTagTypes(tagFile.TagTypes);
                        int appliedCount = ApplyDirtyFieldsToTag(tagFile, caps, report, path);
                        if (appliedCount == 0)
                        {
                            report.AddSkipped(path, "No changed fields are supported by this file format.");
                            continue;
                        }

                        tagFile.Save();
                        report.AddSaved(path);
                    }
                }
                catch (Exception ex)
                {
                    report.AddFailed(path, ex.Message);
                }
            }

            return report;
        }

        private int ApplyDirtyFieldsToTag(TagLib.File tagFile, TagEditorCapabilities caps, SaveReport report, string path)
        {
            int appliedCount = 0;

            if (caps.Basic)
            {
                if (_dirtyFields.Contains(TagField.Title)) { tagFile.Tag.Title = tbTitle.Text; appliedCount++; }
                if (_dirtyFields.Contains(TagField.Performers)) { tagFile.Tag.Performers = SplitMultiValue(tbArtist.Text); appliedCount++; }
                if (_dirtyFields.Contains(TagField.Album)) { tagFile.Tag.Album = tbAlbum.Text; appliedCount++; }
                if (_dirtyFields.Contains(TagField.AlbumArtists)) { tagFile.Tag.AlbumArtists = SplitMultiValue(tbAlbumArtists.Text); appliedCount++; }
                if (_dirtyFields.Contains(TagField.Genres)) { tagFile.Tag.Genres = SplitMultiValue(tbGenres.Text); appliedCount++; }
                if (_dirtyFields.Contains(TagField.Year)) { tagFile.Tag.Year = ParseOptionalUInt(tbDate.Text, "Year"); appliedCount++; }
                if (_dirtyFields.Contains(TagField.Track)) { tagFile.Tag.Track = ParseOptionalUInt(tbTrack.Text, "Track"); appliedCount++; }
                if (_dirtyFields.Contains(TagField.Disc)) { tagFile.Tag.Disc = ParseOptionalUInt(tbDisk.Text, "Disk"); appliedCount++; }
                if (_dirtyFields.Contains(TagField.Comment)) { tagFile.Tag.Comment = tbComments.Text; appliedCount++; }
            }
            else
            {
                AddUnsupported(report, path, TagField.Title, TagField.Performers, TagField.Album, TagField.AlbumArtists, TagField.Genres, TagField.Year, TagField.Track, TagField.Disc, TagField.Comment);
            }

            if (caps.ExtendedCredits)
            {
                if (_dirtyFields.Contains(TagField.Composers)) { tagFile.Tag.Composers = SplitMultiValue(tbComposers.Text); appliedCount++; }
                if (_dirtyFields.Contains(TagField.Conductor)) { tagFile.Tag.Conductor = tbConductor.Text; appliedCount++; }
            }
            else
            {
                AddUnsupported(report, path, TagField.Composers, TagField.Conductor);
            }

            if (caps.AdvancedIds)
            {
                if (_dirtyFields.Contains(TagField.Grouping)) { tagFile.Tag.Grouping = tbGrouping.Text; appliedCount++; }
                if (_dirtyFields.Contains(TagField.ISRC)) { tagFile.Tag.ISRC = tbISRC.Text; appliedCount++; }
                if (_dirtyFields.Contains(TagField.Publisher)) { tagFile.Tag.Publisher = tbPublisher.Text; appliedCount++; }
                if (_dirtyFields.Contains(TagField.InitialKey)) { tagFile.Tag.InitialKey = tbInitKey.Text; appliedCount++; }
                if (_dirtyFields.Contains(TagField.BPM)) { tagFile.Tag.BeatsPerMinute = ParseOptionalUInt(tbBPM.Text, "BPM"); appliedCount++; }
                if (_dirtyFields.Contains(TagField.Copyright)) { tagFile.Tag.Copyright = tbCopyright.Text; appliedCount++; }
            }
            else
            {
                AddUnsupported(report, path, TagField.Grouping, TagField.ISRC, TagField.Publisher, TagField.InitialKey, TagField.BPM, TagField.Copyright);
            }

            return appliedCount;
        }

        private void ValidateSingleFileInputs()
        {
            if (_capabilities.Basic)
            {
                ValidateOptionalUInt(tbDate.Text, "Year");
                ValidateOptionalUInt(tbTrack.Text, "Track");
                ValidateOptionalUInt(tbDisk.Text, "Disk");
            }

            if (_capabilities.AdvancedIds)
            {
                ValidateOptionalUInt(tbBPM.Text, "BPM");
            }
        }

        private void ValidateDirtyInputs()
        {
            if (_dirtyFields.Contains(TagField.Year))
                ValidateOptionalUInt(tbDate.Text, "Year");

            if (_dirtyFields.Contains(TagField.Track))
                ValidateOptionalUInt(tbTrack.Text, "Track");

            if (_dirtyFields.Contains(TagField.Disc))
                ValidateOptionalUInt(tbDisk.Text, "Disk");

            if (_dirtyFields.Contains(TagField.BPM))
                ValidateOptionalUInt(tbBPM.Text, "BPM");
        }

        private static void ValidateOptionalUInt(string value, string fieldName)
        {
            ParseOptionalUInt(value, fieldName);
        }

        private static string CorruptionReasonText(TagLib.File tagFile)
        {
            if (tagFile == null)
                return "File is not writable.";

            if (tagFile.CorruptionReasons != null)
            {
                string[] reasons = tagFile.CorruptionReasons.ToArray();
                if (reasons.Length > 0)
                    return string.Join(Environment.NewLine, reasons);
            }

            if (tagFile.PossiblyCorrupt)
                return "File may be corrupt and cannot be written safely.";

            return "File is not writable.";
        }

        private void AddUnsupported(SaveReport report, string path, params TagField[] fields)
        {
            if (report == null || fields == null)
                return;

            foreach (TagField field in fields)
            {
                if (_dirtyFields.Contains(field))
                    report.AddUnsupported(path, TagFieldDisplayName(field));
            }
        }

        private static string TagFieldDisplayName(TagField field)
        {
            switch (field)
            {
                case TagField.Performers:
                    return "Artist";
                case TagField.AlbumArtists:
                    return "Album artist";
                case TagField.ISRC:
                    return "ISRC";
                case TagField.InitialKey:
                    return "Initial key";
                case TagField.BPM:
                    return "BPM";
                default:
                    return field.ToString();
            }
        }

        private static void ShowSaveReport(SaveReport report)
        {
            if (report == null)
                return;

            string title = report.Failed > 0
                ? "Some Tags Could Not Be Saved"
                : report.HasIssues
                    ? "Tags Saved With Notes"
                    : "Tags Saved";

            MessageBoxIcon icon = report.Failed > 0
                ? MessageBoxIcon.Error
                : report.HasIssues
                    ? MessageBoxIcon.Warning
                    : MessageBoxIcon.Information;

            MessageBox.Show(report.ToMessage(), title, MessageBoxButtons.OK, icon);
        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OFD = new OpenFileDialog() { Filter = AudioFileSupport.OpenFileFilter, Multiselect = true })
            {
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    LoadFiles(OFD.FileNames);
                }
            }
        }

        private void ApplyCapabilities()
        {
            SetEnabled(_capabilities.Basic, tbTitle, tbArtist, tbAlbum, tbAlbumArtists, tbGenres, tbDate, tbTrack, tbDisk, tbComments);
            SetEnabled(_capabilities.ExtendedCredits, tbComposers, tbConductor);
            SetEnabled(_capabilities.AdvancedIds, tbGrouping, tbISRC, tbPublisher, tbInitKey, tbBPM, tbCopyright);
            tbLyrics.Enabled = !_multiFileMode && _capabilities.Lyrics;
            tbpgCoverArt.Enabled = !_multiFileMode && _capabilities.Pictures;

            bool canSave = _multiFileMode
                ? _loadedFiles.Count > 0 && _dirtyFields.Count > 0 && !_capabilities.IsEmpty
                : file != null && file.Writeable && !_capabilities.IsEmpty;

            btnSave.Enabled = canSave;

            if (_multiFileMode)
            {
                Text = string.Format("TagEditorDialog - {0} files selected", _loadedFiles.Count);
            }
            else
            {
                Text = file == null
                    ? "TagEditorDialog"
                    : string.Format("TagEditorDialog - {0} ({1})", Path.GetFileName(LoadedTrackFile), file.TagTypes);
            }
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

        private static string FormatPathForReport(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return "Selection";

            string fileName = Path.GetFileName(path);
            return string.IsNullOrWhiteSpace(fileName) ? path : fileName;
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

            public static TagEditorCapabilities ForMultipleFiles(List<TagSnapshot> snapshots)
            {
                if (snapshots == null || snapshots.Count == 0)
                    return None;

                TagEditorCapabilities acc = FromTagTypes(snapshots[0].TagTypes);
                for (int i = 1; i < snapshots.Count; i++)
                {
                    TagEditorCapabilities next = FromTagTypes(snapshots[i].TagTypes);
                    acc = new TagEditorCapabilities
                    {
                        Basic = acc.Basic && next.Basic,
                        ExtendedCredits = acc.ExtendedCredits && next.ExtendedCredits,
                        AdvancedIds = acc.AdvancedIds && next.AdvancedIds,
                        Lyrics = acc.Lyrics && next.Lyrics,
                        Pictures = acc.Pictures && next.Pictures
                    };
                }

                // Multi-file mode: keep these false for safety even if formats support it.
                acc.Lyrics = false;
                acc.Pictures = false;
                return acc;
            }
        }

        private enum TagField
        {
            Title,
            Performers,
            Album,
            AlbumArtists,
            Composers,
            Conductor,
            Genres,
            Grouping,
            ISRC,
            Publisher,
            InitialKey,
            Comment,
            BPM,
            Year,
            Disc,
            Track,
            Copyright
        }

        private void HookDirtyTracking()
        {
            tbTitle.TextChanged += (s, e) => MarkDirty(TagField.Title);
            tbArtist.TextChanged += (s, e) => MarkDirty(TagField.Performers);
            tbAlbum.TextChanged += (s, e) => MarkDirty(TagField.Album);
            tbAlbumArtists.TextChanged += (s, e) => MarkDirty(TagField.AlbumArtists);
            tbComposers.TextChanged += (s, e) => MarkDirty(TagField.Composers);
            tbConductor.TextChanged += (s, e) => MarkDirty(TagField.Conductor);
            tbGenres.TextChanged += (s, e) => MarkDirty(TagField.Genres);
            tbGrouping.TextChanged += (s, e) => MarkDirty(TagField.Grouping);
            tbISRC.TextChanged += (s, e) => MarkDirty(TagField.ISRC);
            tbPublisher.TextChanged += (s, e) => MarkDirty(TagField.Publisher);
            tbInitKey.TextChanged += (s, e) => MarkDirty(TagField.InitialKey);
            tbComments.TextChanged += (s, e) => MarkDirty(TagField.Comment);
            tbBPM.TextChanged += (s, e) => MarkDirty(TagField.BPM);
            tbDate.TextChanged += (s, e) => MarkDirty(TagField.Year);
            tbDisk.TextChanged += (s, e) => MarkDirty(TagField.Disc);
            tbTrack.TextChanged += (s, e) => MarkDirty(TagField.Track);
            tbCopyright.TextChanged += (s, e) => MarkDirty(TagField.Copyright);
        }

        private void MarkDirty(TagField field)
        {
            if (_isLoadingUi)
                return;

            _dirtyFields.Add(field);
            ApplyCapabilities();
        }

        private static string CommonOrMixed(IEnumerable<string> values)
        {
            var list = (values ?? Enumerable.Empty<string>())
                .Select(v => v ?? string.Empty)
                .ToList();

            if (list.Count == 0)
                return string.Empty;

            string first = list[0];
            if (list.All(v => string.Equals(v, first, StringComparison.Ordinal)))
                return first;

            return "<multiple>";
        }

        private static string CommonOrMixedNumber(IEnumerable<uint> values)
        {
            var list = (values ?? Enumerable.Empty<uint>()).ToList();
            if (list.Count == 0)
                return string.Empty;

            uint first = list[0];
            if (list.All(v => v == first))
                return first == 0 ? string.Empty : first.ToString();

            return "<multiple>";
        }

        private class SaveReport
        {
            private const int MaxDetails = 10;
            private readonly List<string> _details = new List<string>();

            public int Saved { get; private set; }
            public int Skipped { get; private set; }
            public int Failed { get; private set; }
            public int UnsupportedFields { get; private set; }
            public bool HasIssues { get { return Skipped > 0 || Failed > 0 || UnsupportedFields > 0; } }

            public void AddSaved(string path)
            {
                Saved++;
            }

            public void AddSkipped(string path, string reason)
            {
                Skipped++;
                AddDetail("Skipped", path, reason);
            }

            public void AddFailed(string path, string reason)
            {
                Failed++;
                AddDetail("Failed", path, reason);
            }

            public void AddUnsupported(string path, string fieldName)
            {
                UnsupportedFields++;
                AddDetail("Unsupported field", path, fieldName);
            }

            public string ToMessage()
            {
                List<string> lines = new List<string>
                {
                    string.Format("Saved: {0}", Saved),
                    string.Format("Skipped files: {0}", Skipped),
                    string.Format("Failed files: {0}", Failed),
                    string.Format("Unsupported fields skipped: {0}", UnsupportedFields)
                };

                if (_details.Count > 0)
                {
                    lines.Add(string.Empty);
                    lines.Add("Details:");
                    lines.AddRange(_details.Take(MaxDetails));

                    int remaining = _details.Count - MaxDetails;
                    if (remaining > 0)
                        lines.Add(string.Format("...and {0} more.", remaining));
                }

                return string.Join(Environment.NewLine, lines);
            }

            private void AddDetail(string status, string path, string reason)
            {
                _details.Add(string.Format("{0}: {1} - {2}", status, FormatPathForReport(path), reason));
            }
        }

        private static TagSnapshot ReadSnapshot(string path)
        {
            using (var tagFile = TagLib.File.Create(path))
            {
                return new TagSnapshot
                {
                    Path = path,
                    TagTypes = tagFile.TagTypes,
                    Writeable = tagFile.Writeable,
                    Title = tagFile.Tag.Title ?? string.Empty,
                    PerformersJoined = tagFile.Tag.JoinedPerformers ?? string.Empty,
                    Album = tagFile.Tag.Album ?? string.Empty,
                    AlbumArtistsJoined = tagFile.Tag.JoinedAlbumArtists ?? string.Empty,
                    ComposersJoined = tagFile.Tag.JoinedComposers ?? string.Empty,
                    Conductor = tagFile.Tag.Conductor ?? string.Empty,
                    GenresJoined = tagFile.Tag.JoinedGenres ?? string.Empty,
                    Grouping = tagFile.Tag.Grouping ?? string.Empty,
                    Isrc = tagFile.Tag.ISRC ?? string.Empty,
                    Publisher = tagFile.Tag.Publisher ?? string.Empty,
                    InitialKey = tagFile.Tag.InitialKey ?? string.Empty,
                    Comment = tagFile.Tag.Comment ?? string.Empty,
                    BeatsPerMinute = tagFile.Tag.BeatsPerMinute,
                    Year = tagFile.Tag.Year,
                    Disc = tagFile.Tag.Disc,
                    Track = tagFile.Tag.Track,
                    Copyright = tagFile.Tag.Copyright ?? string.Empty
                };
            }
        }

        private class TagSnapshot
        {
            public string Path;
            public TagLib.TagTypes TagTypes;
            public bool Writeable;
            public string Title;
            public string PerformersJoined;
            public string Album;
            public string AlbumArtistsJoined;
            public string ComposersJoined;
            public string Conductor;
            public string GenresJoined;
            public string Grouping;
            public string Isrc;
            public string Publisher;
            public string InitialKey;
            public string Comment;
            public uint BeatsPerMinute;
            public uint Year;
            public uint Disc;
            public uint Track;
            public string Copyright;
        }
    }
}
