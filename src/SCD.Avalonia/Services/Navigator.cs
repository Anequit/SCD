using ReactiveUI;
using System;
using System.Collections.ObjectModel;

namespace SCD.Avalonia.Services;

public class Navigator
{
    private readonly ObservableCollection<ReactiveObject> _viewModels;

    public event Action? CurrentViewModelChanged;

    public Navigator() => _viewModels = new ObservableCollection<ReactiveObject>();

    public ReactiveObject CurrentViewModel
    {
        get => _viewModels[_viewModels.Count - 1];
        set
        {
            PreviousViewModel = CurrentViewModel;

            _viewModels[_viewModels.Count - 1] = value;
            OnCurrentViewModelChanged();
        }
    }

    public ObservableCollection<ReactiveObject> ViewModels
    {
        get => _viewModels;
    }

    public ReactiveObject? PreviousViewModel { get; private set; }

    private void OnCurrentViewModelChanged() => CurrentViewModelChanged?.Invoke();
}
