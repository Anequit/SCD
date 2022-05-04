using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SCD.Core.Utilities;

public static class Explorer
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
}
