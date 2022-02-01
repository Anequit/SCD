using Avalonia.Controls;
using CyberdropDownloader.Avalonia.Services;
using CyberdropDownloader.Avalonia.Views;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace CyberdropDownloader.Avalonia.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private readonly Navigator _navigator;

    public MainWindowViewModel(Navigator navigator, Window window)
    {
        _navigator = navigator;

        _navigator.CurrentViewChanged += CurrentViewModelChanged;

        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            window.ExtendClientAreaToDecorationsHint = true;
            window.SystemDecorations = SystemDecorations.BorderOnly;
            window.ExtendClientAreaChromeHints = 0;

            _navigator.Views.Add(new TitleBarView());
        }

        _navigator.Views.Add(new MainFormView());
        CurrentViewModelChanged();
    }

    public ObservableCollection<UserControl> Views => _navigator.Views;

    private void CurrentViewModelChanged() => this.RaisePropertyChanged(nameof(Views));
}
