using ReactiveUI;
using SCD.Avalonia.Services;
using System.Reactive;

namespace SCD.Avalonia.ViewModels;

public class AlertViewModel : ReactiveObject
{
    private readonly Navigator _navigator;

    public AlertViewModel(Navigator navigator, string error, string errorMessage)
    {
        _navigator = navigator;

        Error = error;
        ErrorMessage = errorMessage;

        CloseCommand = ReactiveCommand.Create(() => Close());
    }

    public string Error { get; }
    public string ErrorMessage { get; }

    public ReactiveCommand<Unit, Unit> CloseCommand { get; }

    private void Close() => _navigator.CurrentAlertViewModel = null;
}