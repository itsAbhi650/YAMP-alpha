using System.Collections.Generic;
using System.Drawing;
using System.IO;


namespace YAMP_alpha
{
    public class TrackInfo
    {
        public FileInfo File { get; }
        public string Title { get; }
        public string TrackNum { get; }
        public string Album { get; }
        public string Year { get; }
        public string Artist { get; }
        public string AlbumArtist { get; }
        public string Composer { get; }
        public string Genre { get; }
        public string DiskNumber { get; }
        public string Comment { get; }
        public string Path { get { return File.FullName; } }
        public string Duration { get; }
        public string BitRate { get; }
        public string SampleRate { get; }
        public List<Image> Covers { get; private set; } = null;


        public TrackInfo(string Filename)
        {
            File = new FileInfo(Filename);
            MediaInfo.MediaInfoWrapper minfo = new MediaInfo.MediaInfoWrapper(Filename);
            Title = minfo.AudioStreams[0].Tags.Title;
            Duration = minfo.AudioStreams[0].Duration.ToString(@"mm\:ss");
            Album = minfo.AudioStreams[0].Tags.Album;
            AlbumArtist = minfo.AudioStreams[0].Tags.AlbumArtist;
            Genre = minfo.AudioStreams[0].Tags.Genre;
            BitRate = minfo.AudioStreams[0].Bitrate.ToString();
            SampleRate = minfo.AudioStreams[0].SamplingRate.ToString();
            TrackNum = minfo.AudioStreams[0].Tags.TrackPosition?.ToString();
            GetCoverArts(minfo);
        }

        private void GetCoverArts(MediaInfo.MediaInfoWrapper MINFO)
        {
            Covers = new List<Image>();
            foreach (var item in MINFO.BestAudioStream.Tags.Covers)
            {
                byte[] ImgData = item.Data;
                Image CoverImage = BufferToImage(ImgData);
                if (CoverImage != null)
                {
                    Covers.Add(CoverImage);
                }
            }
            bool filled = Covers.Count > 0;
            if (!filled)
            {
                var tg = TagLib.File.Create(File.FullName, TagLib.ReadStyle.PictureLazy);
                foreach (var item in tg.Tag.Pictures)
                {
                    byte[] ImgData = item.Data.Data;
                    Image CoverImage = BufferToImage(ImgData);
                    if (CoverImage != null)
                    {
                        Covers.Add(CoverImage);
                        break;
                    }
                }
                tg.Mode = TagLib.File.AccessMode.Closed;
                tg.Dispose();
            }
        }

        private Image BufferToImage(byte[] ImageBuffer)
        {
            if (ImageBuffer != null)
            {
                using (MemoryStream Ms = new MemoryStream(ImageBuffer))
                {
                    Image img = Image.FromStream(Ms);
                    return img;
                }
            }
            else
            {
                return null;
            }
        }
        public TrackInfo()
        {

        }
    }
}
