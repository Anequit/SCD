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
    public static Version? CurrentVersion { get; } = Assembly.GetExecutingAssembly().GetName().Version;

    /// <summary>
    /// Checks if there is a newer version on github.
    /// </summary>
    /// <returns>True if update is available.</returns>
    public static async Task CheckForUpdateAsync()
    {
        try
        {
            if(CurrentVersion is null)
                return;

            Release latestRelease = await WebUtilities.FetchLatestRelease();

            Version latestVersion = new Version(latestRelease.VersionNumber.Remove(0, 1));

            // Normalizes the version to match how latestVersion is parsed
            // Without normalization it wouldn't be able to tell if there was a new version
            Version currentVersion = new Version(CurrentVersion.ToString(3));

            if(latestVersion > currentVersion)
                NavigationService.ShowUpdateAlert("Update Available", "Would you like to update?", latestRelease);
        }
        catch(Exception exception)
        {
            switch(exception)
            {
                case NullReleaseException:
                    NavigationService.ShowErrorAlert("Error", "Failed to get latest release.");
                    break;

                case NullOrEmptyVersionNumberException:
                    NavigationService.ShowErrorAlert("Error", "Failed to get latest version number.");
                    break;

                case NullOrEmptyUrlException:
                    NavigationService.ShowErrorAlert("Error", "Failed to get latest version url.");
                    break;

                default:
                    NavigationService.ShowErrorAlert("Unknown Error", exception.Message);
                    break;
            }
        }
    }
}
