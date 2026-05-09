using CSCore.Codecs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YAMP_alpha
{
    internal static class AudioFileSupport
    {
        private static readonly Lazy<HashSet<string>> SupportedExtensions =
            new Lazy<HashSet<string>>(() => new HashSet<string>(
                CodecFactory.Instance.GetSupportedFileExtensions()
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(NormalizeExtension),
                StringComparer.OrdinalIgnoreCase));

        public static string OpenFileFilter
        {
            get
            {
                string supported = CodecFactory.SupportedFilesFilterEn;
                return string.IsNullOrWhiteSpace(supported)
                    ? "Audio files|*.*"
                    : supported + "|All files|*.*";
            }
        }

        public static bool IsSupportedAudioFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            string extension = Path.GetExtension(path);
            if (string.IsNullOrWhiteSpace(extension))
                return false;

            return SupportedExtensions.Value.Contains(NormalizeExtension(extension));
        }

        public static IEnumerable<FileInfo> EnumerateSupportedAudioFiles(DirectoryInfo directory)
        {
            if (directory == null || !directory.Exists)
                return Enumerable.Empty<FileInfo>();

            return directory.EnumerateFiles()
                .Where(file => IsSupportedAudioFile(file.FullName));
        }

        private static string NormalizeExtension(string extension)
        {
            return extension.Trim().TrimStart('.');
        }
    }
}
