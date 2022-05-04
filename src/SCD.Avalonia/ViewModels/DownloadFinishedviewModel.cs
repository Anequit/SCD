using Avalonia.Controls;
using ReactiveUI;
using SCD.Avalonia.Services;
using SCD.Core.Utilities;
using System.Reactive;

namespace SCD.Avalonia.ViewModels;

public class DownloadFinishedViewModel : ReactiveObject
{
    public DownloadFinishedViewModel(Window window, string path)
    {
        HomeCommand = ReactiveCommand.Create(() => Home(window));
        OpenFolderCommand = ReactiveCommand.Create(() => OpenFolder(path));
    }

    public ReactiveCommand<Unit, Unit> HomeCommand { get; set; }
    public ReactiveCommand<Unit, Unit> OpenFolderCommand { get; set; }

    private void Home(Window window) => NavigationService.NavigateTo(new MainFormViewModel(window));
    private void OpenFolder(string path) => Explorer.Open(path);
}
