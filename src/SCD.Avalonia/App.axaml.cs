using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SCD.Avalonia.Services;
using SCD.Avalonia.ViewModels;
using SCD.Avalonia.Views;

namespace SCD.Avalonia;

public class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if(ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            desktop.MainWindow.DataContext = new MainWindowViewModel(new Navigator(), desktop.MainWindow);
        }

        base.OnFrameworkInitializationCompleted();
    }
}
