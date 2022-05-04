using Avalonia.Controls;
using ReactiveUI;
using SCD.Avalonia.Services;
using SCD.Core;
using SCD.Core.DataModels;
using SCD.Core.Exceptions;
using SCD.Core.Extensions;
using SCD.Core.Helpers;
using SCD.Core.Utilities;
using System;
using System.IO;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace SCD.Avalonia.ViewModels;

public class DownloadingViewModel : ReactiveObject
{
    private readonly Window _window;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly Progress<decimal> _progress;

    private double _downloadProgress = 0;
    private string _filename = "Loading..";

    public DownloadingViewModel(Window window, string albumURL, string downloadLocation)
    {
        _window = window;

        _cancellationTokenSource = new CancellationTokenSource();
        _progress = new Progress<decimal>(ProgressChanged);

        AlbumDownloader.FileChanged += AlbumDownloader_FileChanged;
        AlbumDownloader.ErrorOccurred += AlbumDownloader_ErrorOccurred;

        CancelDownloadCommand = ReactiveCommand.Create(CancelDownload);

        Task.Run(async () => await DownloadAsync(albumURL, downloadLocation));
    }

    public string Filename
    {
        get => _filename;
        set => this.RaiseAndSetIfChanged(ref _filename, value);
    }

    public double DownloadProgress
    {
        get => _downloadProgress;
        set => this.RaiseAndSetIfChanged(ref _downloadProgress, value);
    }

    public ReactiveCommand<Unit, Unit> CancelDownloadCommand { get; }

    private void CancelDownload()
    {
        _cancellationTokenSource?.Cancel();
        HttpClientHelper.Cancel();
        GC.Collect();

        NavigationService.NavigateTo(new MainFormViewModel(_window));
    }

    private async Task DownloadAsync(string albumURL, string downloadLocation)
    {
        try
        {
            Album album = await HttpClientHelper.HttpClient.FetchAlbumAsync(albumURL, _cancellationTokenSource.Token);

            album.Title = (string.IsNullOrEmpty(Parser.ParseValidPath(album.Title))) ? "Unknown Album Title" : album.Title;
            downloadLocation = (string.IsNullOrEmpty(Parser.ParseValidPath(downloadLocation))) ? throw new ArgumentException(nameof(downloadLocation)) : downloadLocation;

            string path = Path.Combine(downloadLocation, album.Title);

            await AlbumDownloader.DownloadAlbumAsync(album, path, _progress, _cancellationTokenSource.Token);

            NavigationService.NavigateTo(new DownloadFinishedViewModel(_window, path));
        }
        catch(Exception ex)
        {
            switch(ex)
            {
                case FailedToFetchAlbumException:
                    NavigationService.ShowErrorAlert("Error", ex.Message);
                    break;
            }

            NavigationService.NavigateTo(new MainFormViewModel(_window));
        }
    }

    private void ProgressChanged(decimal progressAmount) => DownloadProgress = (double)Math.Round(progressAmount, MidpointRounding.ToZero);

    private void AlbumDownloader_ErrorOccurred(Exception obj) => NavigationService.ShowErrorAlert("Error", obj.Message);

    private void AlbumDownloader_FileChanged(AlbumFile file)
    {
        Filename = file.Filename;
        DownloadProgress = 0;
    }
}
