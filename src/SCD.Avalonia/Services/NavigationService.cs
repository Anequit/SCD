using ReactiveUI;
using SCD.Avalonia.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace SCD.Avalonia.Services;

public static class NavigationService
{
    public static event Action? CurrentViewModelChanged;
    public static event Action? CurrentAlertViewModelChanged;

    public static ObservableCollection<ReactiveObject?> ViewModels { get; } = new ObservableCollection<ReactiveObject?>();

    public static ReactiveObject? CurrentViewModel
    {
        get => ViewModels[0];
        private set
        {
            if(ViewModels.Count == 0)
                ViewModels.Add(value);

            ViewModels[0] = value;
            CurrentViewModelChanged?.Invoke();
        }
    }

    public static ReactiveObject? CurrentAlertViewModel
    {
        get
        {
            if(ViewModels[ViewModels.Count - 1] is not AlertViewModel)
                return null;

            return ViewModels[ViewModels.Count - 1];
        }
        private set
        {
            if(ViewModels.Count == 1)
                ViewModels.Add(value);

            ViewModels[ViewModels.Count - 1] = value;

            CurrentAlertViewModelChanged?.Invoke();
        }
    }

    public static void NavigateTo(ReactiveObject viewModel) => CurrentViewModel = viewModel;

    public static void ShowAlert(string error, string errorMessage) => CurrentAlertViewModel = new AlertViewModel(error, errorMessage);

    public static void CloseAlert() => CurrentAlertViewModel = null;
}
