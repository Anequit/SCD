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
    private readonly Navigator _navigator;
    private readonly Window _window;
    private readonly CancellationTokenSource _cancellationTokenSource;

    private int _progress = 0;
    private string _filename = "";

    public DownloadingViewModel(Navigator navigator, Window window, Album album, string downloadLocation)
    {
        _navigator = navigator;
        _window = window;

        _cancellationTokenSource = new CancellationTokenSource();

        CancelDownloadCommand = ReactiveCommand.Create(() => CancelDownload());

        AlbumDownloader.DownloadFinished += AlbumDownloader_DownloadFinished;
        AlbumDownloader.FileChanged += AlbumDownloader_FileChanged;
        AlbumDownloader.ProgressChanged += AlbumDownloader_ProgressChanged;

        Task.Run(async () => await AlbumDownloader.Download(album, downloadLocation, _cancellationTokenSource.Token));
    }

    public string Filename
    {
        get => _filename;
        set => this.RaiseAndSetIfChanged(ref _filename, value);
    }

    public int Progress
    {
        get => _progress;
        set => this.RaiseAndSetIfChanged(ref _progress, value);
    }

    public ReactiveCommand<Unit, Unit> CancelDownloadCommand { get; }

    private void CancelDownload()
    {
        HttpClientHelper.Cancel();
        _cancellationTokenSource?.Cancel();
        _navigator.CurrentViewModel = new MainFormViewModel(_navigator, _window);
    }

    private void AlbumDownloader_ProgressChanged(int e) => Progress = e;
    private void AlbumDownloader_FileChanged(AlbumFile e) => Filename = e.Name;
    private void AlbumDownloader_DownloadFinished(string e) => _navigator.CurrentViewModel = new DownloadFinishedViewModel(_navigator, _window, e);
}
