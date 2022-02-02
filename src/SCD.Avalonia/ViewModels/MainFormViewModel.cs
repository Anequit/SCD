using ReactiveUI;
using SCD.Avalonia.Services;
using System.Diagnostics;
using System.Reactive;
using System.Runtime.InteropServices;

namespace SCD.Avalonia.ViewModels;

public class MainFormViewModel : ReactiveObject
{
    private string _albumURL = string.Empty;
    private string _downloadLocation = string.Empty;

    public MainFormViewModel(Navigator navigator)
    {
        ReportBugCommand = ReactiveCommand.Create(() => ReportBug());
        DownloadCommand = ReactiveCommand.Create(() => { });
        SelectCommand = ReactiveCommand.Create(() => { });
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

    private void Download() { }

    private void ReportBug(string url = "https://github.com/Anequit/SCD/issues")
    {
        // https://stackoverflow.com/a/43232486
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            url = url.Replace("&", "^&");
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
        else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", url);
        }
        else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", url);
        }
    }

    private void Select() { }
}
