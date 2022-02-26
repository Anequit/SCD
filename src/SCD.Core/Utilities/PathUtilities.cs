using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace SCD.Core.Utilities;

public static class PathUtilities
{
    /// <summary>
    /// Opens folder in default file explorer for each platform.
    /// </summary>
    /// <param name="path">Path to open.</param>
    public static void Open(string path)
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Process.Start("explorer", path);
        }
        else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", path);
        }
        else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", path);
        }
    }

    /// <summary>
    /// Removes all invalid path characters
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string RemoveInvalidPathChars(string path) => string.Concat(path.Split(Path.GetInvalidPathChars()));

    /// <summary>
    /// Removes all invalid filename characters
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static string RemoveInvalidFilenameChars(string filename) => string.Concat(filename.Split(Path.GetInvalidFileNameChars()));
}
