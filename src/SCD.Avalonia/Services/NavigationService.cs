using ReactiveUI;
using SCD.Avalonia.ViewModels;
using SCD.Core.DataModels;
using System;
using System.Collections.ObjectModel;

namespace SCD.Avalonia.Services;

public static class NavigationService
{
    public static event Action? CurrentViewModelChanged;
    public static event Action? CurrentAlertViewModelChanged;

    // Initialize with an array of 2 objects
    public static ObservableCollection<ReactiveObject?> ViewModels { get; } = new ObservableCollection<ReactiveObject?>(new ReactiveObject[2]);

    // Will always be in ViewModels[0]
    public static ReactiveObject? CurrentViewModel
    {
        get => ViewModels[0];
        private set
        {
            ViewModels[0] = value;
            CurrentViewModelChanged?.Invoke();
        }
    }

    public static ReactiveObject? CurrentAlertViewModel
    {
        get
        {
            return ViewModels[1];
        }
        private set
        {
            ViewModels[1] = value;
            CurrentAlertViewModelChanged?.Invoke();
        }
    }

    public static void NavigateTo(ReactiveObject viewModel) => CurrentViewModel = viewModel;

    public static void ShowErrorAlert(string title, string message) => CurrentAlertViewModel = new ErrorAlertViewModel(title, message);

    public static void ShowUpdateAlert(string title, string message, Release release) => CurrentAlertViewModel = new UpdateAlertViewModel(title, message, release);

    public static void CloseAlert() => CurrentAlertViewModel = null;
}
