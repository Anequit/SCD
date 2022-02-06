using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SCD.Avalonia.Views;

public partial class DownloadFinishedView : UserControl
{
    public DownloadFinishedView() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
