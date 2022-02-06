using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SCD.Avalonia.Views;
public partial class MainFormView : UserControl
{
    public MainFormView() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
