using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SCD.Avalonia.Views;
public partial class AlertView : UserControl
{
    public AlertView()
    {
        InitializeComponent();

#if !RELEASE
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
