using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SCD.Avalonia.Views;

public partial class DownloadingView : UserControl
{
    public DownloadingView() 
    {
        InitializeComponent();

#if !RELEASE
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
