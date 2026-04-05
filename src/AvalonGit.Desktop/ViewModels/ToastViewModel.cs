using System;
using System.Reactive;
using Avalonia;
using Avalonia.Media;
using Avalonia.Controls;
using ReactiveUI;

namespace AvalonGit.Desktop.ViewModels;

public class ToastViewModel : ReactiveObject
{
    private bool _isVisible;
    private string _title = string.Empty;
    private string _message = string.Empty;
    private bool _isError;
    private bool _hasDetails;
    private string _details = string.Empty;

    public bool IsVisible
    {
        get => _isVisible;
        set => this.RaiseAndSetIfChanged(ref _isVisible, value);
    }

    public string Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    public string Message
    {
        get => _message;
        set => this.RaiseAndSetIfChanged(ref _message, value);
    }

    public bool IsError
    {
        get => _isError;
        set => this.RaiseAndSetIfChanged(ref _isError, value);
    }

    public bool HasDetails
    {
        get => _hasDetails;
        set => this.RaiseAndSetIfChanged(ref _hasDetails, value);
    }

    public string Details
    {
        get => _details;
        set => this.RaiseAndSetIfChanged(ref _details, value);
    }

    public IBrush IconColor => IsError 
        ? new SolidColorBrush(Color.Parse("#F87171")) 
        : new SolidColorBrush(Color.Parse("#4ADE80"));

    public StreamGeometry? IconData => null;

    public ReactiveCommand<Unit, Unit> ClearCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowDetailsCommand { get; }

    public event Action? DetailsRequested;

    public ToastViewModel()
    {
        ClearCommand = ReactiveCommand.Create(() =>
        {
            IsVisible = false;
            HasDetails = false;
            Details = string.Empty;
        });

        ShowDetailsCommand = ReactiveCommand.Create(() =>
        {
            DetailsRequested?.Invoke();
        });
    }

    public void Show(string title, string message, bool isError, string? details = null)
    {
        Title = title;
        Message = message;
        IsError = isError;
        HasDetails = !string.IsNullOrWhiteSpace(details);
        Details = details ?? string.Empty;
        IsVisible = true;

        this.RaisePropertyChanged(nameof(IconColor));
        this.RaisePropertyChanged(nameof(IconData));
    }

    public void Hide()
    {
        IsVisible = false;
    }
}
