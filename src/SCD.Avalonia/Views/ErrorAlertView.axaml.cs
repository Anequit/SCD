using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SCD.Avalonia.Views;

public partial class ErrorAlertView : UserControl
{
    public ErrorAlertView()
    {
        InitializeComponent();
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
