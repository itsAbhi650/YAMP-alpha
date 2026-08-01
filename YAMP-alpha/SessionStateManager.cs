using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace YAMP_alpha
{
    [Serializable]
    public class SessionState
    {
        public string LoadedPlaylistPath { get; set; }
        public List<string> PlaylistTrackPaths { get; set; } = new List<string>();
        public string CurrentTrackPath { get; set; }
        public double CurrentPositionSeconds { get; set; }
        public bool ShuffleEnabled { get; set; }
        public PlaylistLoopMode LoopMode { get; set; }
        public List<string> PendingQueuePaths { get; set; } = new List<string>();
        public DateTime SavedAtUtc { get; set; }
    }

    public class SessionRestoreResult
    {
        public bool PlaylistLoaded { get; set; }
        public bool TrackLoaded { get; set; }
        public bool PositionRestored { get; set; }
    }

    public static class SessionStateManager
    {
        private static string GetStateFilePath()
        {
            string baseDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "YAMP-alpha");

            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }

            return Path.Combine(baseDir, "session-state.xml");
        }

        private static SessionState LoadStateInternal()
        {
            string path = GetStateFilePath();
            if (!File.Exists(path))
                return new SessionState();

            try
            {
                var serializer = new XmlSerializer(typeof(SessionState));
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var state = serializer.Deserialize(fs) as SessionState;
                    return state ?? new SessionState();
                }
            }
            catch
            {
                return new SessionState();
            }
        }

        private static void SaveStateInternal(SessionState state)
        {
            if (state == null)
                return;

            state.SavedAtUtc = DateTime.UtcNow;

            try
            {
                var serializer = new XmlSerializer(typeof(SessionState));
                string path = GetStateFilePath();
                using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    serializer.Serialize(fs, state);
                }
            }
            catch
            {
            }
        }

        public static void SavePlaybackSettings()
        {
            SessionState state = LoadStateInternal();
            state.ShuffleEnabled = YAMPVars.ShuffleEnabled;
            state.LoopMode = YAMPVars.PlaylistLoopMode;
            SaveStateInternal(state);
        }

        public static SessionState LoadSessionState()
        {
            SessionState state = LoadStateInternal();
            if (state.PlaylistTrackPaths == null)
            {
                state.PlaylistTrackPaths = new List<string>();
            }
            if (state.PendingQueuePaths == null)
            {
                state.PendingQueuePaths = new List<string>();
            }
            return state;
        }

        public static void RestorePlaybackSettings(SessionState state)
        {
            if (state == null)
                return;

            YAMPVars.ShuffleEnabled = state.ShuffleEnabled;
            YAMPVars.PlaylistLoopMode = state.LoopMode;
        }

        public static void SaveSessionState(YAMP_Core core)
        {
            SessionState state = LoadStateInternal();

            state.ShuffleEnabled = YAMPVars.ShuffleEnabled;
            state.LoopMode = YAMPVars.PlaylistLoopMode;
            state.LoadedPlaylistPath = YAMPVars.LoadedPlaylist;
            state.CurrentTrackPath = core != null && core.CurrentTrack != null ? core.CurrentTrack.Path : string.Empty;
            state.PlaylistTrackPaths = YAMPVars.TrackList
                .Where(track => track != null && !string.IsNullOrWhiteSpace(track.Path))
                .Select(track => track.Path)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (core != null && core.PlayerSource != null && (!core.NetPlay || core.PlayerSource.CanSeek))
            {
                state.CurrentPositionSeconds = core.CurrentTime.TotalSeconds;
            }
            else
            {
                state.CurrentPositionSeconds = 0;
            }

            state.PendingQueuePaths = YAMPVars.PendingQueue
                .Where(track => track != null && !string.IsNullOrWhiteSpace(track.Path))
                .Select(track => track.Path)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            SaveStateInternal(state);
        }

        public static SessionRestoreResult RestoreSessionState(YAMP_Core core)
        {
            var result = new SessionRestoreResult();
            if (core == null)
                return result;

            SessionState state = LoadSessionState();
            RestorePlaybackSettings(state);

            bool loadedPlaylist = false;
            if (state != null && !string.IsNullOrWhiteSpace(state.LoadedPlaylistPath))
            {
                loadedPlaylist = RestorePlaylistFromSavedPath(state.LoadedPlaylistPath, true);
            }

            if (!loadedPlaylist && state != null && state.PlaylistTrackPaths != null && state.PlaylistTrackPaths.Count > 0)
            {
                loadedPlaylist = RestorePlaylistFromSavedTrackPaths(state.PlaylistTrackPaths);
            }

            if (loadedPlaylist && state.PendingQueuePaths != null && state.PendingQueuePaths.Count > 0)
            {
                YAMPVars.PendingQueue.Clear();
                foreach (string queuePath in state.PendingQueuePaths)
                {
                    TrackInfo queueTrack = YAMPVars.TrackList.FirstOrDefault(t =>
                        t != null && string.Equals(t.Path, queuePath, StringComparison.OrdinalIgnoreCase));
                    if (queueTrack != null)
                    {
                        YAMPVars.PendingQueue.Add(queueTrack);
                    }
                }
            }

            result.PlaylistLoaded = loadedPlaylist;
            if (!loadedPlaylist || YAMPVars.TrackList.Count == 0)
                return result;

            TrackInfo trackToLoad = null;
            if (!string.IsNullOrWhiteSpace(state.CurrentTrackPath))
            {
                trackToLoad = YAMPVars.TrackList.FirstOrDefault(t =>
                    t != null && string.Equals(t.Path, state.CurrentTrackPath, StringComparison.OrdinalIgnoreCase));
            }

            if (trackToLoad == null)
            {
                trackToLoad = YAMPVars.TrackList[0];
            }

            if (trackToLoad != null && core.LoadTrackInfo(trackToLoad))
            {
                result.TrackLoaded = true;

                if (state.CurrentPositionSeconds > 0 && (!core.NetPlay || core.PlayerSource.CanSeek))
                {
                    core.Seek(TimeSpan.FromSeconds(state.CurrentPositionSeconds));
                    result.PositionRestored = true;
                }
            }

            return result;
        }

        private static bool RestorePlaylistFromSavedPath(string path, bool clearExisting)
        {
            if (string.IsNullOrWhiteSpace(path) || File.Exists(path) == false)
                return false;

            try
            {
                var xdoc = new System.Xml.XmlDocument();
                xdoc.Load(path);

                if (clearExisting)
                {
                    YAMPVars.TrackList.Clear();
                    YAMPVars.PendingQueue.Clear();
                }

                if (xdoc.DocumentElement == null)
                    return false;

                foreach (System.Xml.XmlNode node in xdoc.DocumentElement.ChildNodes)
                {
                    string filePath = node != null && node.FirstChild != null ? node.FirstChild.InnerText : string.Empty;
                    if (string.IsNullOrWhiteSpace(filePath) || File.Exists(filePath) == false)
                        continue;

                    if (!AudioFileSupport.IsSupportedAudioFile(filePath))
                        continue;

                    bool exists = YAMPVars.TrackList.Any(t => t != null && string.Equals(t.Path, filePath, StringComparison.OrdinalIgnoreCase));
                    if (!exists)
                    {
                        YAMPVars.TrackList.Add(new TrackInfo(filePath));
                    }
                }

                YAMPVars.LoadedPlaylist = path;
                return YAMPVars.TrackList.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        private static bool RestorePlaylistFromSavedTrackPaths(List<string> savedPaths)
        {
            if (savedPaths == null || savedPaths.Count == 0)
                return false;

            YAMPVars.TrackList.Clear();
            YAMPVars.PendingQueue.Clear();

            foreach (string path in savedPaths)
            {
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                    continue;

                if (!AudioFileSupport.IsSupportedAudioFile(path))
                    continue;

                bool exists = YAMPVars.TrackList.Any(t =>
                    t != null && string.Equals(t.Path, path, StringComparison.OrdinalIgnoreCase));
                if (!exists)
                {
                    YAMPVars.TrackList.Add(new TrackInfo(path));
                }
            }

            return YAMPVars.TrackList.Count > 0;
        }
    }
}
