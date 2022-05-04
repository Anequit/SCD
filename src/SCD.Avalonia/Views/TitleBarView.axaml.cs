using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SCD.Avalonia.Views;

public partial class TitleBarView : UserControl
{
    public TitleBarView()
    {
        InitializeComponent();
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
