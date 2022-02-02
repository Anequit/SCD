using ReactiveUI;
using SCD.Avalonia.Services;
using SCD.Core.Utilities;
using System.Reactive;

namespace SCD.Avalonia.ViewModels;

public class MainFormViewModel : ReactiveObject
{
    private string _albumURL = string.Empty;
    private string _downloadLocation = string.Empty;

    public MainFormViewModel(Navigator navigator)
    {
        ReportBugCommand = ReactiveCommand.Create(() => ReportBug());
        DownloadCommand = ReactiveCommand.Create(() => Download());
        SelectCommand = ReactiveCommand.Create(() => Select());
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

    private void ReportBug() => Web.Open("https://github.com/Anequit/SCD/issues");

    private void Download() { }

    private void Select() { }
}
