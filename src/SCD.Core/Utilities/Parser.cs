using System;
using System.IO;

namespace SCD.Core.Utilities;

public static class Parser
{
    public static string ParseAlbumIdentifierFromUrl(string url) => url.Substring(url.Length - 8);

    public static string ParseValidPath(string path) => string.Concat(path.Split(Path.GetInvalidPathChars(), StringSplitOptions.RemoveEmptyEntries));

    public static string ParseValidFilename(string filename) => string.Concat(filename.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));
}