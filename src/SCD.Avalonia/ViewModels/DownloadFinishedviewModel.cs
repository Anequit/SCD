using Avalonia.Controls;
using ReactiveUI;
using SCD.Avalonia.Services;
using SCD.Core.Utilities;
using System.Reactive;

namespace SCD.Avalonia.ViewModels;

public class DownloadFinishedViewModel : ReactiveObject
{
    public DownloadFinishedViewModel(Navigator navigator, Window window, string path)
    {
        HomeCommand = ReactiveCommand.Create(() => Home(navigator, window));
        OpenFolderCommand = ReactiveCommand.Create(() => OpenFolder(path));
    }

    public ReactiveCommand<Unit, Unit> HomeCommand { get; set; }
    public ReactiveCommand<Unit, Unit> OpenFolderCommand { get; set; }

    private void Home(Navigator navigator, Window window) => navigator.CurrentViewModel = new MainFormViewModel(navigator, window);
    private void OpenFolder(string path) => PathUtilities.Open(path);
}
