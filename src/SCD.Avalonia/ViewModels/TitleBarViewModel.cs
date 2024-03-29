﻿using Avalonia.Controls;
using Avalonia.Input;
using ReactiveUI;
using SCD.Avalonia.Services;
using System.Reactive;

namespace SCD.Avalonia.ViewModels;

public class TitleBarViewModel : ReactiveObject
{
    private readonly Window _window;
    private string _title;

    public TitleBarViewModel(Window window)
    {
        _window = window;

        MinimizeCommand = ReactiveCommand.Create(Minimize);
        ExitCommand = ReactiveCommand.Create(Exit);
        DragCommand = ReactiveCommand.Create<PointerPressedEventArgs>(x => Drag(x));

        string version = "Unknown version";

        if(UpdateService.CurrentVersion is not null)
            version = $"v{UpdateService.CurrentVersion.ToString(3)}";

        _title = $"SCD - {version}";
    }

    public string Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    public ReactiveCommand<Unit, Unit> MinimizeCommand { get; }
    public ReactiveCommand<Unit, Unit> ExitCommand { get; }
    public ReactiveCommand<PointerPressedEventArgs, Unit> DragCommand { get; }

    private void Minimize() => _window.WindowState = WindowState.Minimized;
    private void Exit() => _window.Close();
    private void Drag(PointerPressedEventArgs eventArgs) => _window.BeginMoveDrag(eventArgs);
}