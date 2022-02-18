using ReactiveUI;
using SCD.Avalonia.Services;
using System.Reactive;

namespace SCD.Avalonia.ViewModels;

public class AlertViewModel : ReactiveObject
{
    private readonly NavigationService _navigationService;

    public AlertViewModel(NavigationService navigationService, string error, string errorMessage)
    {
        _navigationService = navigationService;

        Error = error;
        ErrorMessage = errorMessage;

        CloseCommand = ReactiveCommand.Create(() => Close());
    }

    public string Error { get; }
    public string ErrorMessage { get; }

    public ReactiveCommand<Unit, Unit> CloseCommand { get; }

    private void Close() => _navigationService.CloseAlert();
}