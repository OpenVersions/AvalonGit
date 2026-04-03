using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvalonGit.Core.IServices;

namespace AvalonGit.Desktop.Views;

public partial class AddRemoteWindow : Window
{
    public string RemoteName { get; set; } = "origin";
    public string RemoteUrl { get; set; } = string.Empty;

    public AddRemoteWindow()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void Cancel(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private async void AddRemote(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(RemoteName) || string.IsNullOrWhiteSpace(RemoteUrl))
        {
            return;
        }

        Close(new { Name = RemoteName, Url = RemoteUrl });
    }
}
