using Avalonia.Controls;
using ReactiveUI;
using SCD.Avalonia.Services;
using SCD.Core;
using SCD.Core.DataModels;
using System.Reactive;

namespace SCD.Avalonia.ViewModels;

public class DownloadingViewModel : ReactiveObject
{
    private readonly Navigator _navigator;
    private readonly Window _window;

    private int _progress = 0;
    private string _filename = "";

    public DownloadingViewModel(Navigator navigator, Window window, Album album)
    {
        _navigator = navigator;
        _window = window;

        CancelDownloadCommand = ReactiveCommand.Create(() => CancelDownload());

        Downloader.FileChanged += Downloader_FileChanged;
        Downloader.ProgressChanged += Downloader_ProgressChanged;
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
        HttpClientHandler.Cancel();

        _navigator.CurrentViewModel = new MainFormViewModel(_navigator, _window);
    }

    private void Downloader_ProgressChanged(object? sender, int e) => Progress = e;
    private void Downloader_FileChanged(object? sender, AlbumFile e) => Filename = e.Name;
}
