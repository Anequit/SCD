using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SCD.Core.Utilities;

public static class Web
{
    public static void Open(string url)
    {
        // https://stackoverflow.com/a/43232486
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            url = url.Replace("&", "^&");

            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}")
            {
                CreateNoWindow = true
            });
        }
        else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", url);
        }
        else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", url);
        }
    }
}