using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SCD.Avalonia.Services;

namespace SCD.Avalonia.ViewModels;

public partial class TitleBarViewModel : ObservableObject
{
    private readonly Window _window;

    [ObservableProperty]
    private string _title;

    public TitleBarViewModel()
    {
        _window = ((IClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime!).MainWindow!;

        string version = "Unknown version";

        if(UpdateService.CurrentVersion is not null)
            version = $"v{UpdateService.CurrentVersion.ToString(3)}";

        _title = $"SCD - {version}";
    }

    [RelayCommand]
    private void Minimize() => _window.WindowState = WindowState.Minimized;

    [RelayCommand]
    private void Exit() => _window.Close();

    [RelayCommand]
    private void Drag(PointerPressedEventArgs eventArgs) => _window.BeginMoveDrag(eventArgs);
}