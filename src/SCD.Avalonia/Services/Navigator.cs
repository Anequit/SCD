using ReactiveUI;
using SCD.Avalonia.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace SCD.Avalonia.Services;

public class Navigator
{
    private readonly ObservableCollection<ReactiveObject?> _viewModels;

    public event Action? CurrentViewModelChanged;
    public event Action? CurrentAlertViewModelChanged;

    public Navigator() => _viewModels = new ObservableCollection<ReactiveObject?>();

    public ObservableCollection<ReactiveObject?> ViewModels => _viewModels;

    public ReactiveObject? CurrentViewModel
    {
        get => _viewModels[0];
        set
        {
            if(_viewModels.Count == 0)
            {
                _viewModels.Add(value);
            }

            _viewModels[0] = value;
            OnCurrentViewModelChanged();
        }
    }

    public ReactiveObject? CurrentAlertViewModel
    {
        get
        {
            if(_viewModels[_viewModels.Count - 1] is not AlertViewModel)
            {
                return null;
            }

            return _viewModels[_viewModels.Count - 1];
        }
        set
        {
            if(_viewModels.Count == 1)
            {
                _viewModels.Add(value);
            }

            _viewModels[_viewModels.Count - 1] = value;

            OnCurrentAlertViewModelChanged();
        }
    }

    private void OnCurrentViewModelChanged() => CurrentViewModelChanged?.Invoke();
    private void OnCurrentAlertViewModelChanged() => CurrentAlertViewModelChanged?.Invoke();
}
