using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using AvalonGit.Desktop.ViewModels;
using System.Linq;

namespace AvalonGit.Desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }

    private void MinimizeWindow(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void MaximizeWindow(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized 
            ? WindowState.Normal 
            : WindowState.Maximized;
    }

    private void CloseWindow(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private async void OpenRepository(object? sender, RoutedEventArgs e)
    {
        var topLevel = GetTopLevel(this);
        if (topLevel == null) return;

        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Abrir Repositório Git",
            AllowMultiple = false
        });

        if (folders.Any())
        {
            var folderPath = folders.First().Path.LocalPath;
            if (DataContext is MainWindowViewModel vm)
            {
                vm.RepositoryPath = folderPath;
            }
        }
    }

    private async void OpenAboutWindow(object? sender, RoutedEventArgs e)
    {
        var aboutWindow = new AboutWindow
        {
            DataContext = new AboutViewModel()
        };
        await aboutWindow.ShowDialog(this);
    }
}
