using ReactiveUI;
using SCD.Avalonia.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace SCD.Avalonia.Services;

public class NavigationService
{
    public event Action? CurrentViewModelChanged;
    public event Action? CurrentAlertViewModelChanged;

    public NavigationService() => ViewModels = new ObservableCollection<ReactiveObject?>();

    public ObservableCollection<ReactiveObject?> ViewModels { get; }

    public ReactiveObject? CurrentViewModel
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

    public ReactiveObject? CurrentAlertViewModel
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

    public void NavigateTo(ReactiveObject viewModel)
    {
        CurrentViewModel = viewModel;
    }

    public void ShowAlert(string error, string errorMessage)
    {
        CurrentAlertViewModel = new AlertViewModel(this, error, errorMessage);
    }

    public void CloseAlert()
    {
        CurrentAlertViewModel = null;
    }
}
