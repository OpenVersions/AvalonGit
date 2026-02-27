using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using AvalonGit.Desktop.ViewModels;

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

    private async void OpenAboutWindow(object? sender, RoutedEventArgs e)
    {
        var aboutWindow = new AboutWindow
        {
            DataContext = new AboutViewModel()
        };
        await aboutWindow.ShowDialog(this);
    }
}
