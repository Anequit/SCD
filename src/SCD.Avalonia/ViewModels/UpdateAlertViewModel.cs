using ReactiveUI;
using SCD.Avalonia.Services;
using SCD.Core.Utilities;
using System.Reactive;

namespace SCD.Avalonia.ViewModels;

public class UpdateAlertViewModel : ReactiveObject
{
    public UpdateAlertViewModel(string title, string message)
    {
        Title = title;
        Message = message;

        YesCommand = ReactiveCommand.Create(Yes);
        NoCommand = ReactiveCommand.Create(No);
    }

    public string Title { get; }
    public string Message { get; }

    public ReactiveCommand<Unit, Unit> YesCommand { get; }
    public ReactiveCommand<Unit, Unit> NoCommand { get; }

    private void Yes() => Web.Open(@"https://github.com/Anequit/SCD/releases/latest");
    private void No() => NavigationService.CloseAlert();
}
