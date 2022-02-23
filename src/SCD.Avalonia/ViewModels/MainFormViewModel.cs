using Avalonia.Controls;
using ReactiveUI;
using SCD.Avalonia.Services;
using SCD.Core.Exceptions;
using SCD.Core.Utilities;
using System;
using System.IO;
using System.Net.Http;
using System.Reactive;
using System.Reflection;
using System.Threading.Tasks;

namespace SCD.Avalonia.ViewModels;

public class MainFormViewModel : ReactiveObject
{
    private readonly Window _window;

    private string _albumURL = string.Empty;
    private string _downloadLocation = string.Empty;

    public MainFormViewModel(Window window)
    {
        _window = window;

        IObservable<bool> ableToDownload = this.WhenAnyValue(
            x => x.AlbumURL,
            x => x.DownloadLocation,
            (URL, DL) => !string.IsNullOrEmpty(URL) && !string.IsNullOrEmpty(DL));

        ReportBugCommand = ReactiveCommand.Create(() => ReportBug());
        DownloadCommand = ReactiveCommand.Create(() => Download(), ableToDownload);
        SelectCommand = ReactiveCommand.CreateFromTask(() => SelectAsync());

        Task.Run(async () => await UpdateService.CheckForUpdateAsync()).ConfigureAwait(false);
    }

    public string AlbumURL
    {
        get => _albumURL;
        set => this.RaiseAndSetIfChanged(ref _albumURL, value);
    }

    public string DownloadLocation
    {
        get => _downloadLocation;
        set => this.RaiseAndSetIfChanged(ref _downloadLocation, value);
    }

    public ReactiveCommand<Unit, Unit> ReportBugCommand { get; }
    public ReactiveCommand<Unit, Unit> DownloadCommand { get; }
    public ReactiveCommand<Unit, Unit> SelectCommand { get; }

    private void ReportBug() => WebUtilities.Open("https://github.com/Anequit/SCD/issues");

    private void Download()
    {
        if(!Directory.Exists(DownloadLocation))
        {
            NavigationService.ShowErrorAlert("Error", "Invalid download location.");
            DownloadLocation = string.Empty;
            return;
        }

        NavigationService.NavigateTo(new DownloadingViewModel(_window, AlbumURL, DownloadLocation));
    }

    private async Task SelectAsync()
    {
        OpenFolderDialog openFolderDialog = new OpenFolderDialog()
        {
            Title = "Download Location",
            Directory = Assembly.GetExecutingAssembly().Location,
        };

        string? path = await openFolderDialog.ShowAsync(_window);

        if(path is null)
            return;

        DownloadLocation = path;
    }
}
