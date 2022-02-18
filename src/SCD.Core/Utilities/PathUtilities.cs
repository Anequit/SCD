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
    /// Removes invalid characters from path.
    /// </summary>
    /// <param name="path">Path to normalize.</param>
    /// <returns>Normalized path.</returns>
    public static string NormalizePath(string path)
    {
        // Get a string of the invalid characters.
        string invalidChars = new string(Path.GetInvalidPathChars());

        // Iterate over the invalid characters, replacing them one at a time with an empty string.
        foreach(char invalidChar in invalidChars)
            path = path.Replace($"{invalidChar}", string.Empty);

        return path;
    }
}
