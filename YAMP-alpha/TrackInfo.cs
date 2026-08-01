using MediaInfo.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace YAMP_alpha
{
    public class TrackInfo
    {
        public FileInfo File { get; }
        public string Title { get; private set; }
        public string TrackNum { get; private set; }
        public string Album { get; private set; }
        public string Year { get; }
        public string Artist { get; }
        public string AlbumArtist { get; private set; }
        public string Composer { get; }
        public string Genre { get; private set; }
        public List<KeyValuePair<double, string>> Lyrics { get; set; }
        public string DiskNumber { get; }
        public string Comment { get; }
        public string Path { get { return File.FullName; } }
        public string Duration { get; private set; }
        public string BitRate { get; private set; }
        public string SampleRate { get; private set; }
        public List<Image> Covers { get; private set; } = null;
        public int Rating { get; set; }
        public int PlayCount { get; set; }
        public int SkipCount { get; set; }
        public DateTime? LastPlayedAt { get; set; }
        public DateTime AddedAt { get; private set; }

        public TrackInfo(string filename)
        {
            File = new FileInfo(filename);
            AddedAt = DateTime.Now;
            Title = File.Name;
            Duration = string.Empty;
            Album = string.Empty;
            AlbumArtist = string.Empty;
            Genre = string.Empty;
            BitRate = string.Empty;
            SampleRate = string.Empty;
            TrackNum = string.Empty;
            Rating = 0;
            PlayCount = 0;
            SkipCount = 0;
            LastPlayedAt = null;
            Covers = new List<Image>();

            try
            {
                var mediaInfo = new MediaInfo.MediaInfoWrapper(filename);
                if (mediaInfo.AudioStreams != null && mediaInfo.AudioStreams.Count > 0)
                {
                    var audioStream = mediaInfo.AudioStreams[0];
                    AudioTags tags = audioStream.Tags;

                    Title = string.IsNullOrEmpty(tags?.Title) ? File.Name : tags.Title;
                    Duration = audioStream.Duration.ToString(@"mm\:ss");
                    Album = tags?.Album ?? string.Empty;
                    AlbumArtist = tags?.AlbumArtist ?? string.Empty;
                    Genre = tags?.Genre ?? string.Empty;
                    BitRate = audioStream.Bitrate.ToString();
                    SampleRate = audioStream.SamplingRate.ToString();
                    TrackNum = tags?.TrackPosition?.ToString() ?? string.Empty;
                    Covers = GetCoverArts(tags?.Covers);
                    return;
                }
            }
            catch
            {
            }

            TryLoadTagLibFallback();
        }

        public TrackInfo()
        {
            AddedAt = DateTime.Now;
            Rating = 0;
            PlayCount = 0;
            SkipCount = 0;
        }

        private List<Image> GetCoverArts(IEnumerable<CoverInfo> covers)
        {
            var loadedCovers = new List<Image>();

            if (covers != null)
            {
                foreach (var item in covers)
                {
                    Image coverImage = BufferToImage(item.Data);
                    if (coverImage != null)
                    {
                        loadedCovers.Add(coverImage);
                    }
                }
            }

            if (loadedCovers.Count == 0)
            {
                TryLoadTagLibCovers(loadedCovers);
            }

            return loadedCovers;
        }

        private void TryLoadTagLibFallback()
        {
            try
            {
                using (var tagFile = TagLib.File.Create(File.FullName, TagLib.ReadStyle.PictureLazy))
                {
                    Title = string.IsNullOrEmpty(tagFile.Tag.Title) ? File.Name : tagFile.Tag.Title;
                    Duration = tagFile.Properties.Duration == TimeSpan.Zero
                        ? string.Empty
                        : tagFile.Properties.Duration.ToString(@"mm\:ss");
                    Album = tagFile.Tag.Album ?? string.Empty;
                    AlbumArtist = tagFile.Tag.JoinedAlbumArtists ?? string.Empty;
                    Genre = tagFile.Tag.JoinedGenres ?? string.Empty;
                    BitRate = tagFile.Properties.AudioBitrate > 0 ? tagFile.Properties.AudioBitrate.ToString() : string.Empty;
                    SampleRate = tagFile.Properties.AudioSampleRate > 0 ? tagFile.Properties.AudioSampleRate.ToString() : string.Empty;
                    TrackNum = tagFile.Tag.Track > 0 ? tagFile.Tag.Track.ToString() : string.Empty;
                    Covers = GetCoverArts(null);
                }
            }
            catch
            {
                Covers = new List<Image>();
            }
        }

        private void TryLoadTagLibCovers(List<Image> covers)
        {
            try
            {
                using (var tagFile = TagLib.File.Create(File.FullName, TagLib.ReadStyle.PictureLazy))
                {
                    foreach (var item in tagFile.Tag.Pictures)
                    {
                        Image coverImage = BufferToImage(item.Data.Data);
                        if (coverImage != null)
                        {
                            covers.Add(coverImage);
                            break;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private Image BufferToImage(byte[] imageBuffer)
        {
            if (imageBuffer == null)
                return null;

            try
            {
                using (MemoryStream stream = new MemoryStream(imageBuffer))
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
    }
}
