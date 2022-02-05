using ReactiveUI;
using SCD.Avalonia.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace SCD.Avalonia.Services;

public class Navigator
{
    public event Action? CurrentViewModelChanged;
    public event Action? CurrentAlertViewModelChanged;

    public Navigator() => ViewModels = new ObservableCollection<ReactiveObject?>();

    public ObservableCollection<ReactiveObject?> ViewModels { get; }

    public ReactiveObject? CurrentViewModel
    {
        get => ViewModels[0];
        set
        {
            if(ViewModels.Count == 0)
            {
                ViewModels.Add(value);
            }

            ViewModels[0] = value;
            CurrentViewModelChanged?.Invoke();
        }
    }

    public ReactiveObject? CurrentAlertViewModel
    {
        get
        {
            if(ViewModels[ViewModels.Count - 1] is not AlertViewModel)
            {
                return null;
            }

            return ViewModels[ViewModels.Count - 1];
        }
        set
        {
            if(ViewModels.Count == 1)
            {
                ViewModels.Add(value);
            }

            ViewModels[ViewModels.Count - 1] = value;

            CurrentAlertViewModelChanged?.Invoke();
        }
    }
}
