using Avalonia.Controls;
using ReactiveUI;
using SCD.Avalonia.Services;
using System.Runtime.InteropServices;

namespace SCD.Avalonia.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private readonly NavigationService _navigationService;
    private ReactiveObject? _titleBarViewModel;

    public MainWindowViewModel(NavigationService navigationService, Window window)
    {
        _navigationService = navigationService;

        _navigationService.CurrentViewModelChanged += CurrentViewModelChanged;
        _navigationService.CurrentAlertViewModelChanged += CurrentAlertViewModelChanged;

        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            window.ExtendClientAreaToDecorationsHint = true;
            window.ExtendClientAreaChromeHints = 0;
            window.ExtendClientAreaTitleBarHeightHint = -1;

            TitleBarViewModel = new TitleBarViewModel(window);
        }

        _navigationService.NavigateTo(new MainFormViewModel(_navigationService, window));
    }

    public ReactiveObject? CurrentViewModel => _navigationService.CurrentViewModel;
    public ReactiveObject? AlertViewModel => _navigationService.CurrentAlertViewModel;
    public ReactiveObject? TitleBarViewModel
    {
        get => _titleBarViewModel;
        set => this.RaiseAndSetIfChanged(ref _titleBarViewModel, value);
    }

    private void CurrentViewModelChanged() => this.RaisePropertyChanged(nameof(CurrentViewModel));
    private void CurrentAlertViewModelChanged() => this.RaisePropertyChanged(nameof(AlertViewModel));
}
