using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SCD.Avalonia.Services;
using SCD.Core;
using SCD.Core.DataModels;
using SCD.Core.Extensions;
using SCD.Core.Helpers;
using SCD.Core.Utilities;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SCD.Avalonia.ViewModels;

public partial class DownloadingViewModel : ObservableObject
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly Progress<decimal> _progress;

    [ObservableProperty]
    private double _downloadProgress;

    [ObservableProperty]
    private string _filename = "Loading..";

    public DownloadingViewModel(string albumUrl, string downloadLocation)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _progress = new Progress<decimal>(ProgressChanged);

        AlbumDownloader.FileChanged += FileChanged;
        AlbumDownloader.ErrorOccurred += ErrorOccurred;

        Task.Run(async () => await DownloadAsync(albumUrl, downloadLocation));
    }

    [RelayCommand]
    private void CancelDownload()
    {
        _cancellationTokenSource?.Cancel();
        HttpClientHelper.Cancel();

        NavigationService.NavigateTo(new MainFormViewModel());
    }

    private async Task DownloadAsync(string albumUrl, string downloadLocation)
    {
        try
        {
            // Fetch album from cyberdrop
            Album album = await HttpClientHelper.HttpClient.FetchAlbumAsync(albumUrl, _cancellationTokenSource.Token);

            // Parse album title
            album.Title = string.IsNullOrEmpty(Parser.ParseValidPath(album.Title)) ? "Unknown Album Title" : album.Title;
            
            // Parse download path
            downloadLocation = string.IsNullOrEmpty(Parser.ParseValidPath(downloadLocation)) ? throw new ArgumentException(null, nameof(downloadLocation)) : downloadLocation;

            // Create folder path
            string path = Path.Combine(downloadLocation, album.Title);

            // Download album files
            await AlbumDownloader.DownloadAlbumAsync(album, path, _progress, _cancellationTokenSource.Token);

            // Navigate to download finished page
            NavigationService.NavigateTo(new DownloadFinishedViewModel(path));
        }
        catch(Exception ex)
        {
            // If task wasn't canceled report error to user
            if(!_cancellationTokenSource.Token.IsCancellationRequested && ex is not (OperationCanceledException or TaskCanceledException))
                ErrorOccurred(ex);

            // Navigate back to main page
            NavigationService.NavigateTo(new MainFormViewModel());
        }
    }

    private void ProgressChanged(decimal progressAmount) => DownloadProgress = (double)Math.Round(progressAmount, MidpointRounding.ToZero);

    private void ErrorOccurred(Exception obj) => NavigationService.ShowErrorAlert("Error", obj.Message);

    private void FileChanged(AlbumFile file)
    {
        Filename = file.Filename;
        DownloadProgress = 0;
    }
}