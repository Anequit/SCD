using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CyberdropDownloader.Avalonia.Services;
using CyberdropDownloader.Avalonia.ViewModels;
using CyberdropDownloader.Avalonia.Views;

namespace CyberdropDownloader.Avalonia;
public class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(new Navigator()),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
