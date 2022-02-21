using ReactiveUI;
using SCD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCD.Avalonia.ViewModels;

public class UpdatingViewModel : ReactiveObject
{
    private double _progress;

    public UpdatingViewModel()
    {
    }

    public double Progress
    {
        get => _progress;
        set => this.RaiseAndSetIfChanged(ref _progress, value);
    }
}
