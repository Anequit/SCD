using Avalonia.Controls;
using ReactiveUI;
using SCD.Avalonia.Services;
using SCD.Core.DataModels;
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
    private readonly NavigationService _navigationService;
    private readonly Window _window;

    private string _albumURL = string.Empty;
    private string _downloadLocation = string.Empty;

    public MainFormViewModel(NavigationService navigationService, Window window)
    {
        _navigationService = navigationService;
        _window = window;

        IObservable<bool> ableToDownload = this.WhenAnyValue(
            x => x.AlbumURL,
            x => x.DownloadLocation,
            (URL, DL) => !string.IsNullOrEmpty(URL) && !string.IsNullOrEmpty(DL));

        ReportBugCommand = ReactiveCommand.Create(() => ReportBug());
        DownloadCommand = ReactiveCommand.CreateFromTask(() => Download(), ableToDownload);
        SelectCommand = ReactiveCommand.CreateFromTask(() => SelectAsync());
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

    private async Task Download()
    {
        try
        {
            if(!Directory.Exists(DownloadLocation))
                throw new InvalidPathException(DownloadLocation);

            Album album = await WebUtilities.FetchAlbumAsync(AlbumURL);

            _navigationService.NavigateTo(new DownloadingViewModel(_navigationService, _window, album, DownloadLocation));
        }
        catch(Exception exception)
        {
            switch(exception)
            {
                case ArgumentOutOfRangeException:
                    _navigationService.ShowAlert("Error", "Invalid album URL.");
                    AlbumURL = string.Empty;
                    break;

                case UnsuccessfulAlbumException:
                    _navigationService.ShowAlert("Error", exception.Message);
                    AlbumURL = string.Empty;
                    break;

                case InvalidPathException:
                    _navigationService.ShowAlert("Error", "Invalid download location.");
                    DownloadLocation = string.Empty;
                    break;

                case HttpRequestException:
                    _navigationService.ShowAlert("Error", "Failed to get response from server.");
                    break;

                default:
                    _navigationService.ShowAlert("Unknown Error", exception.Message);
                    break;
            }
        }
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
