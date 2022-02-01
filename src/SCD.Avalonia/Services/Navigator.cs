using Avalonia.Controls;
using System;
using System.Collections.ObjectModel;

namespace SCD.Avalonia.Services;

public class Navigator
{
    private readonly ObservableCollection<UserControl> _views;

    public event Action? CurrentViewChanged;

    public Navigator() => _views = new ObservableCollection<UserControl>();

    public UserControl CurrentView
    {
        get => _views[-1];
        set
        {
            // Store prvious view incase we want to implement a back button
            PreviousView = _views[-1];

            _views[-1] = value;
            OnCurrentViewChanged();
        }
    }

    public ObservableCollection<UserControl> Views
    {
        get => _views;
    }

    public UserControl? PreviousView { get; private set; }

    private void OnCurrentViewChanged() => CurrentViewChanged?.Invoke();
}
