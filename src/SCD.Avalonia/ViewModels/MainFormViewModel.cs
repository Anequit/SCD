using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Platform.Storage.FileIO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SCD.Avalonia.Services;
using SCD.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SCD.Avalonia.ViewModels;

public partial class MainFormViewModel : ObservableValidator
{
    private readonly Window _window;

    [Required]
    [ObservableProperty]
    private string _albumUrl = string.Empty;

    [Required]
    [ObservableProperty]
    private string _downloadLocation = string.Empty;

    public MainFormViewModel()
    {
        _window = App.MainWindow;

        Task.Run(UpdateService.CheckForUpdateAsync);
    }

    [RelayCommand]
    private void ReportBug() => Web.Open("https://github.com/Anequit/SCD/issues");

    [RelayCommand]
    private void Download() => NavigationService.NavigateTo(new DownloadingViewModel(AlbumUrl, DownloadLocation));

    [RelayCommand]
    private async Task SelectAsync()
    {
        IReadOnlyList<IStorageFolder> result = await _window.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Download Location",
            AllowMultiple = false,
            SuggestedStartLocation = new BclStorageFolder(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
        });

        if(result.Count == 0)
            return;

        result[0].TryGetUri(out Uri? path);

        if(path is null)
            return;

        DownloadLocation = path.LocalPath;
    }
}