using Avalonia.Controls;
using ReactiveUI;
using System.Reactive;

namespace SCD.Avalonia.ViewModels;

public class TitleBarViewModel : ReactiveObject
{
    private readonly Window _window;

    public TitleBarViewModel(Window window)
    {
        _window = window;

        MinimizeCommand = ReactiveCommand.Create(() => Minimize());
        ExitCommand = ReactiveCommand.Create(() => Exit());
    }

    public ReactiveCommand<Unit, Unit> MinimizeCommand { get; }
    public ReactiveCommand<Unit, Unit> ExitCommand { get; }

    private void Minimize() => _window.WindowState = WindowState.Minimized;

    private void Exit() => _window.Close();
}