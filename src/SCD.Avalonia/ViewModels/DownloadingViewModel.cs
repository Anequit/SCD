using Avalonia.Controls;
using ReactiveUI;
using SCD.Avalonia.Services;
using SCD.Core;
using SCD.Core.DataModels;
using SCD.Core.Utilities;
using SCD.Helpers;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace SCD.Avalonia.ViewModels;

public class DownloadingViewModel : ReactiveObject
{
    private readonly Window _window;
    private readonly CancellationTokenSource _cancellationTokenSource;

    private double _progress = 0;
    private string _filename = "Loading..";

    public DownloadingViewModel(Window window, string albumURL, string downloadLocation)
    {
        _window = window;

        _cancellationTokenSource = new CancellationTokenSource();

        CancelDownloadCommand = ReactiveCommand.Create(() => CancelDownload());

        Downloader.DownloadFinished += AlbumDownloader_DownloadFinished;
        Downloader.FileChanged += AlbumDownloader_FileChanged;
        Downloader.ProgressChanged += AlbumDownloader_ProgressChanged;
        Downloader.ErrorOccurred += AlbumDownloader_ErrorOccurred;

        Task.Run(async () => await Downloader.DownloadAlbumAsync(await WebUtilities.FetchAlbumAsync(albumURL), downloadLocation, _cancellationTokenSource.Token));
    }

    public string Filename
    {
        get => _filename;
        set => this.RaiseAndSetIfChanged(ref _filename, value);
    }

    public double Progress
    {
        get => _progress;
        set => this.RaiseAndSetIfChanged(ref _progress, value);
    }

    public ReactiveCommand<Unit, Unit> CancelDownloadCommand { get; }

    private void CancelDownload()
    {
        HttpClientHelper.Cancel();
        _cancellationTokenSource?.Cancel();
        NavigationService.NavigateTo(new MainFormViewModel(_window));
    }

    private void AlbumDownloader_ProgressChanged(double e) => Progress = e;
    private void AlbumDownloader_FileChanged(AlbumFile e) => Filename = e.Name;
    private void AlbumDownloader_DownloadFinished(string e) => NavigationService.NavigateTo(new DownloadFinishedViewModel(_window, e));
    private void AlbumDownloader_ErrorOccurred(string e) => NavigationService.ShowAlert("Error", e);
}
