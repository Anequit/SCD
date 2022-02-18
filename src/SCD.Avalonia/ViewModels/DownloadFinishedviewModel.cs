using Avalonia.Controls;
using ReactiveUI;
using SCD.Avalonia.Services;
using SCD.Core.Utilities;
using System.Reactive;

namespace SCD.Avalonia.ViewModels;

public class DownloadFinishedViewModel : ReactiveObject
{
    NavigationService _navigationService;

    public DownloadFinishedViewModel(NavigationService navigationService, Window window, string path)
    {
        _navigationService = navigationService;

        HomeCommand = ReactiveCommand.Create(() => Home(window));
        OpenFolderCommand = ReactiveCommand.Create(() => OpenFolder(path));
    }

    public ReactiveCommand<Unit, Unit> HomeCommand { get; set; }
    public ReactiveCommand<Unit, Unit> OpenFolderCommand { get; set; }

    private void Home(Window window) => _navigationService.NavigateTo(new MainFormViewModel(_navigationService, window));
    private void OpenFolder(string path) => PathUtilities.Open(path);
}
