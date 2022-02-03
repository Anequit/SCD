using Avalonia.Controls;
using ReactiveUI;
using SCD.Avalonia.Services;
using System.Runtime.InteropServices;

namespace SCD.Avalonia.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private readonly Navigator _navigator;
    private ReactiveObject? _titleBarViewModel;

    public MainWindowViewModel(Navigator navigator, Window window)
    {
        _navigator = navigator;

        _navigator.CurrentViewModelChanged += CurrentViewModelChanged;
        _navigator.CurrentAlertViewModelChanged += CurrentAlertViewModelChanged;

        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            window.ExtendClientAreaToDecorationsHint = true;
            window.ExtendClientAreaChromeHints = 0;
            window.ExtendClientAreaTitleBarHeightHint = -1;

            TitleBarViewModel = new TitleBarViewModel(window);
        }

        _navigator.CurrentViewModel = new MainFormViewModel(_navigator, window);
    }

    public ReactiveObject? CurrentViewModel => _navigator.CurrentViewModel;
    public ReactiveObject? AlertViewModel => _navigator.CurrentAlertViewModel;
    public ReactiveObject? TitleBarViewModel
    {
        get => _titleBarViewModel;
        set => this.RaiseAndSetIfChanged(ref _titleBarViewModel, value);
    }

    private void CurrentViewModelChanged() => this.RaisePropertyChanged(nameof(CurrentViewModel));
    private void CurrentAlertViewModelChanged() => this.RaisePropertyChanged(nameof(AlertViewModel));
}
