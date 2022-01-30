using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;


namespace YAMP_alpha
{
    public class TrackInfo
    {
        public FileInfo Info { get; }
        public string _title = null;
        public string _album = null;
        public string _artist = null;
        public string _albumArtist = null;
        public string _composer = null;
        public string _comment = null;
        public string _diskNumber = null;
        public string _genre = null;
        public string _trackNum = null;
        public string _length = null;
        public string _bitRate = null;
        public string _sampleRate = null;
        public string _conductor = null;
        public string _year = null;
        private List<Image> _pictures = new List<Image>();


        public TrackInfo(string Filename)
        {
            Info = new FileInfo(Filename);
            MediaInfo.MediaInfoWrapper minfo = new MediaInfo.MediaInfoWrapper(Filename);
            _title = minfo.AudioStreams[0].Tags.Title;
            _length = minfo.AudioStreams[0].Duration.ToString(@"mm\:ss");
            _album = minfo.AudioStreams[0].Tags.Album;
            _albumArtist = minfo.AudioStreams[0].Tags.AlbumArtist;
            _genre = minfo.AudioStreams[0].Tags.Genre;
            _bitRate = minfo.AudioStreams[0].Bitrate.ToString();
            _sampleRate = minfo.AudioStreams[0].SamplingRate.ToString();
            _trackNum = minfo.AudioStreams[0].Tags.TrackPosition?.ToString();
        }
        public TrackInfo()
        {
                
        }
    }
}
