using SCD.Core.Utilities;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SCD.Avalonia.Services;

public static class UpdateService
{
    /// <summary>
    /// Current version of the application.
    /// </summary>
    public static Version? CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version;

    /// <summary>
    /// Checks if there is a newer version on github.
    /// </summary>
    /// <returns>True if update is available.</returns>
    public static bool CheckForUpdateAsync()
    {
        bool needsUpdated = false;

        Task.Run(async () =>
        {
            Version latestVersion = new Version(await WebUtilities.FetchLatestVersion());

            if(CurrentVersion < latestVersion)
                needsUpdated = true;
        });

        return needsUpdated;
    }
}
