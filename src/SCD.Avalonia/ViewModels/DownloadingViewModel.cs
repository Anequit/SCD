using Avalonia.Controls;
using ReactiveUI;
using SCD.Avalonia.Services;
using SCD.Core;
using SCD.Core.DataModels;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace SCD.Avalonia.ViewModels;

public class DownloadingViewModel : ReactiveObject
{
    private readonly NavigationService _navigationService;
    private readonly Window _window;
    private readonly CancellationTokenSource _cancellationTokenSource;

    private double _progress = 0;
    private string _filename = "Loading..";

    public DownloadingViewModel(NavigationService navigationService, Window window, string albumURL, string downloadLocation)
    {
        _navigationService = navigationService;
        _window = window;

        _cancellationTokenSource = new CancellationTokenSource();

        CancelDownloadCommand = ReactiveCommand.Create(() => CancelDownload());

        AlbumDownloader.DownloadFinished += AlbumDownloader_DownloadFinished;
        AlbumDownloader.FileChanged += AlbumDownloader_FileChanged;
        AlbumDownloader.ProgressChanged += AlbumDownloader_ProgressChanged;
        AlbumDownloader.ErrorOccurred += AlbumDownloader_ErrorOccurred;

        Task.Run(async () => await AlbumDownloader.DownloadAsync(await WebUtilities.FetchAlbumAsync(albumURL), downloadLocation, _cancellationTokenSource.Token));
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
        _navigationService.NavigateTo(new MainFormViewModel(_navigationService, _window));
    }

    private void AlbumDownloader_ProgressChanged(double e) => Progress = e;
    private void AlbumDownloader_FileChanged(AlbumFile e) => Filename = e.Name;
    private void AlbumDownloader_DownloadFinished(string e) => _navigationService.NavigateTo(new DownloadFinishedViewModel(_navigationService, _window, e));
    private void AlbumDownloader_ErrorOccurred(string e) => _navigationService.ShowAlert("Error", e);
}
