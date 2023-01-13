using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SCD.Avalonia.Services;
using SCD.Core.Utilities;

namespace SCD.Avalonia.ViewModels;

public partial class UpdateAlertViewModel : ObservableObject
{
    [ObservableProperty]
    private string _message;

    [ObservableProperty]
    private string _title;

    public UpdateAlertViewModel(string title, string message)
    {
        _title = title;
        _message = message;
    }

    [RelayCommand]
    private void Yes() => Web.Open(@"https://github.com/Anequit/SCD/releases/latest");

    [RelayCommand]
    private void No() => NavigationService.CloseAlert();
}