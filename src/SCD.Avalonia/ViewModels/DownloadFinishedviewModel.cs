using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SCD.Avalonia.Services;
using SCD.Core.Utilities;

namespace SCD.Avalonia.ViewModels;

public partial class DownloadFinishedViewModel : ObservableObject
{
    private readonly string _path;

    public DownloadFinishedViewModel(string path) => _path = path;

    [RelayCommand]
    private void Home() => NavigationService.NavigateTo(new MainFormViewModel());

    [RelayCommand]
    private void OpenFolder() => Explorer.Open(_path);
}