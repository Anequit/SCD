using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace SCD.Core.Utilities;

public static class PathUtilities
{
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

    public static string NormalizePath(string path)
    {
        string invalidChars = new string(Path.GetInvalidPathChars());

        foreach(char invalidChar in invalidChars)
            path = path.Replace($"{invalidChar}", string.Empty);

        return path;
    }
}
