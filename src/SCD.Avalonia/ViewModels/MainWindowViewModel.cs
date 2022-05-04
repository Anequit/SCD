using Avalonia.Controls;
using ReactiveUI;
using SCD.Avalonia.Services;

namespace SCD.Avalonia.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private ReactiveObject? _titleBarViewModel;

    public MainWindowViewModel(Window window)
    {
        NavigationService.CurrentViewModelChanged += CurrentViewModelChanged;
        NavigationService.CurrentAlertViewModelChanged += CurrentAlertViewModelChanged;

        window.ExtendClientAreaToDecorationsHint = true;
        window.ExtendClientAreaChromeHints = 0;
        window.ExtendClientAreaTitleBarHeightHint = -1;

        TitleBarViewModel = new TitleBarViewModel(window);

        NavigationService.NavigateTo(new MainFormViewModel(window));
    }

    public ReactiveObject? CurrentViewModel => NavigationService.CurrentViewModel;
    public ReactiveObject? AlertViewModel => NavigationService.CurrentAlertViewModel;
    public ReactiveObject? TitleBarViewModel
    {
        get => _titleBarViewModel;
        set => this.RaiseAndSetIfChanged(ref _titleBarViewModel, value);
    }

    private void CurrentViewModelChanged() => this.RaisePropertyChanged(nameof(CurrentViewModel));
    private void CurrentAlertViewModelChanged() => this.RaisePropertyChanged(nameof(AlertViewModel));
}
