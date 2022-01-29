using ReactiveUI;
using System;

namespace CyberdropDownloader.Avalonia.Services;

public class Navigator
{
    private ReactiveObject? _currentViewModel;
    private ReactiveObject? _previousViewModel;

    public event Action? CurrentViewModelChanged;

    public ReactiveObject? CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _previousViewModel = CurrentViewModel;

            _currentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }

    public ReactiveObject? PreviousViewModel => _previousViewModel;

    private void OnCurrentViewModelChanged() => CurrentViewModelChanged?.Invoke();
}
