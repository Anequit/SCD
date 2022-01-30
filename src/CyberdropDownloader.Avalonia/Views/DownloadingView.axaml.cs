using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CyberdropDownloader.Avalonia.Views;

public partial class DownloadingView : UserControl
{
    public DownloadingView() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
