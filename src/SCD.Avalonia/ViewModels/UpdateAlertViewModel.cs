using ReactiveUI;
using SCD.Avalonia.Services;
using SCD.Core.DataModels;
using SCD.Core.Utilities;
using System.Reactive;

namespace SCD.Avalonia.ViewModels;

public class UpdateAlertViewModel : ReactiveObject
{
    private readonly Release _release;

    public UpdateAlertViewModel(string title, string message, Release release)
    {
        _release = release;

        Title = title;
        Message = message;

        YesCommand = ReactiveCommand.Create(() => Yes());
        NoCommand = ReactiveCommand.Create(() => No());
    }

    public string Title { get; }
    public string Message { get; }

    public ReactiveCommand<Unit, Unit> YesCommand { get; }
    public ReactiveCommand<Unit, Unit> NoCommand { get; }

    private void Yes() => WebUtilities.Open(_release.Url);

    private void No() => NavigationService.CloseAlert();
}
