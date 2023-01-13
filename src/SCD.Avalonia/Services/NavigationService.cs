using CommunityToolkit.Mvvm.ComponentModel;
using SCD.Avalonia.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace SCD.Avalonia.Services;

public static class NavigationService
{
    // Initialize with an array of 2 objects
    private static ObservableCollection<ObservableObject?> ViewModels { get; } = new ObservableCollection<ObservableObject?>(new ObservableObject[2]);

    // Will always be in ViewModels[0]
    public static ObservableObject? CurrentViewModel
    {
        get => ViewModels[0];
        private set
        {
            ViewModels[0] = value;
            CurrentViewModelChanged?.Invoke();
        }
    }

    public static ObservableObject? CurrentAlertViewModel
    {
        get => ViewModels[1];
        private set
        {
            ViewModels[1] = value;
            CurrentAlertViewModelChanged?.Invoke();
        }
    }

    public static event Action? CurrentViewModelChanged;
    public static event Action? CurrentAlertViewModelChanged;

    public static void NavigateTo(ObservableObject viewModel) => CurrentViewModel = viewModel;

    public static void ShowErrorAlert(string title, string message) => CurrentAlertViewModel = new ErrorAlertViewModel(title, message);

    public static void ShowUpdateAlert(string title, string message) => CurrentAlertViewModel = new UpdateAlertViewModel(title, message);

    public static void CloseAlert() => CurrentAlertViewModel = null;
}