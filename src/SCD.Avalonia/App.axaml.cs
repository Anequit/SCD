using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SCD.Avalonia.ViewModels;
using SCD.Avalonia.Views;

namespace SCD.Avalonia;

public class App : Application
{
    public static Window MainWindow { get; private set; } = null!;

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if(ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            MainWindow = new MainWindow();

            desktop.MainWindow = MainWindow;
            desktop.MainWindow.DataContext = new MainWindowViewModel(desktop.MainWindow);
        }

        base.OnFrameworkInitializationCompleted();
    }
}