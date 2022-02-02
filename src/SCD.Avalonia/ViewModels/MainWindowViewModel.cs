using Avalonia.Controls;
using SCD.Avalonia.Services;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace SCD.Avalonia.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private readonly Navigator _navigator;

    public MainWindowViewModel(Navigator navigator, Window window)
    {
        _navigator = navigator;

        _navigator.CurrentViewModelChanged += CurrentViewModelChanged;

        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            window.ExtendClientAreaToDecorationsHint = true;
            window.SystemDecorations = SystemDecorations.BorderOnly;
            window.ExtendClientAreaChromeHints = 0;

            _navigator.ViewModels.Add(new TitleBarViewModel());
        }

        _navigator.ViewModels.Add(new MainFormViewModel(_navigator));
        CurrentViewModelChanged();
    }

    public ObservableCollection<ReactiveObject> ViewModels => _navigator.ViewModels;

    private void CurrentViewModelChanged() => this.RaisePropertyChanged(nameof(ViewModels));
}
