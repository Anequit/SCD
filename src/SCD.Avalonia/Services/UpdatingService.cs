using SCD.Core.DataModels;
using SCD.Core.Exceptions;
using SCD.Core.Extensions;
using SCD.Core.Helpers;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SCD.Avalonia.Services;

public static class UpdateService
{
    /// <summary>
    ///     Current version of the application.
    /// </summary>
    public static Version? CurrentVersion { get; } = Assembly.GetExecutingAssembly().GetName().Version;

    /// <summary>
    ///     Checks if there is a newer version on github.
    /// </summary>
    /// <returns>True if update is available.</returns>
    public static async Task CheckForUpdateAsync()
    {
        try
        {
            Release latestRelease = await HttpClientHelper.HttpClient.FetchLatestReleaseAsync();

            if(CurrentVersion is null || latestRelease.VersionNumber == "-1")
                return;

            Version latestVersion = new Version(latestRelease.VersionNumber.Remove(0, 1));

            Version currentVersion = new Version(CurrentVersion.ToString(3));

            if(latestVersion > currentVersion)
                NavigationService.ShowUpdateAlert("Update Available", "Would you like to update?");
        }
        catch(Exception ex)
        {
            switch(ex)
            {
                case FailedToFetchLatestRelease:
                    NavigationService.ShowErrorAlert("Error", "Failed to get latest release.");

                    break;

                default:
                    NavigationService.ShowErrorAlert("Unknown Error", ex.Message);

                    break;
            }
        }
    }
}