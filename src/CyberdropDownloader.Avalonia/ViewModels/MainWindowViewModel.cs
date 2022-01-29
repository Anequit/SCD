using CyberdropDownloader.Avalonia.Services;
using ReactiveUI;

namespace CyberdropDownloader.Avalonia.ViewModels;
public class MainWindowViewModel : ReactiveObject
{
    private readonly Navigator _navigator;

    public MainWindowViewModel(Navigator navigator)
    {
        _navigator = navigator;

        _navigator.CurrentViewModelChanged += CurrentViewModelChanged;
    }

    public ReactiveObject? CurrentViewModel => _navigator.CurrentViewModel;

    private void CurrentViewModelChanged() => this.RaisePropertyChanged(nameof(CurrentViewModel));
}
