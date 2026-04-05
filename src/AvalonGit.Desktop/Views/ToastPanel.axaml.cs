using System;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using AvalonGit.Desktop.ViewModels;

namespace AvalonGit.Desktop.Views;

public partial class ToastPanel : UserControl
{
    private DispatcherTimer? _autoCloseTimer;

    public ToastPanel()
    {
        InitializeComponent();
    }

    private void CloseToast(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ToastViewModel vm)
        {
            vm.IsVisible = false;
            vm.ClearCommand.Execute(Unit.Default);
        }
    }

    private void ShowDetails(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ToastViewModel vm)
        {
            vm.ShowDetailsCommand.Execute(Unit.Default);
        }
    }

    public void ShowToast(string title, string message, bool isError, string? details = null, int autoCloseSeconds = 5)
    {
        if (DataContext is ToastViewModel vm)
        {
            vm.Show(title, message, isError, details);
            
            if (!isError && autoCloseSeconds > 0)
            {
                _autoCloseTimer?.Stop();
                _autoCloseTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(autoCloseSeconds)
                };
                _autoCloseTimer.Tick += (s, e) =>
                {
                    _autoCloseTimer.Stop();
                    vm.IsVisible = false;
                };
                _autoCloseTimer.Start();
            }
        }
    }
}
