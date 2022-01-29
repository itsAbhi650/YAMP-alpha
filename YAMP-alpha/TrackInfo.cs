using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;


namespace YAMP_alpha
{
    class TrackInfo
    {
        private FileInfo Info = null;
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
        }
        public TrackInfo()
        {
                
        }
    }
}
