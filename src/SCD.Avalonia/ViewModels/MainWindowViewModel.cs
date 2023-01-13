using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using SCD.Avalonia.Services;

namespace SCD.Avalonia.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableObject? _alertViewModel;

    [ObservableProperty]
    private ObservableObject? _currentViewModel;

    [ObservableProperty]
    private ObservableObject? _titleBarViewModel;

    public MainWindowViewModel(Window window)
    {
        NavigationService.CurrentViewModelChanged += CurrentViewModelChanged;
        NavigationService.CurrentAlertViewModelChanged += CurrentAlertViewModelChanged;

        window.ExtendClientAreaToDecorationsHint = true;
        window.ExtendClientAreaChromeHints = 0;
        window.ExtendClientAreaTitleBarHeightHint = -1;

        _titleBarViewModel = new TitleBarViewModel();

        NavigationService.NavigateTo(new MainFormViewModel());
    }

    private void CurrentViewModelChanged() => CurrentViewModel = NavigationService.CurrentViewModel;

    private void CurrentAlertViewModelChanged() => AlertViewModel = NavigationService.CurrentAlertViewModel;
}