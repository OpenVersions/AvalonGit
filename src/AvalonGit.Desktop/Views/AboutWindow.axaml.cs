using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvalonGit.Desktop.Views;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();
    }

    private void CloseWindow(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private async void OpenGitHub(object? sender, RoutedEventArgs e)
    {
        if (!Application.Current!.TryFindResource("ProjectUrl", out var urlResource) ||
            urlResource is not string url) return;
        try
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
        }
        catch
        {
            // ignored
        }
    }
}
