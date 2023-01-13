using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SCD.Avalonia.Services;

namespace SCD.Avalonia.ViewModels;

public partial class ErrorAlertViewModel : ObservableObject
{
    [ObservableProperty]
    private string _error;

    [ObservableProperty]
    private string _errorMessage;

    public ErrorAlertViewModel()
    {
        _error = string.Empty;
        _errorMessage = string.Empty;
    }

    public ErrorAlertViewModel(string error, string errorMessage)
    {
        _error = error;
        _errorMessage = errorMessage;
    }

    [RelayCommand]
    private void Close() => NavigationService.CloseAlert();
}