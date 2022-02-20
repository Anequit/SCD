using ReactiveUI;
using SCD.Avalonia.Services;
using System.Reactive;

namespace SCD.Avalonia.ViewModels;

public class AlertViewModel : ReactiveObject
{
    public AlertViewModel(string error, string errorMessage)
    {
        Error = error;
        ErrorMessage = errorMessage;

        CloseCommand = ReactiveCommand.Create(() => Close());
    }

    public string Error { get; }
    public string ErrorMessage { get; }

    public ReactiveCommand<Unit, Unit> CloseCommand { get; }

    private void Close() => NavigationService.CloseAlert();
}