using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SCD.Avalonia.Views;

public class TitleBarView : UserControl
{
    public TitleBarView() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}