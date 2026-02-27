using System.Reactive;
using ReactiveUI;

namespace AvalonGit.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";

    public ReactiveCommand<Unit, Unit> OpenRepositoryCommand { get; }
    public ReactiveCommand<Unit, Unit> CloneRepositoryCommand { get; }

    public MainWindowViewModel()
    {
        OpenRepositoryCommand = ReactiveCommand.Create(() => { });
        CloneRepositoryCommand = ReactiveCommand.Create(() => { });
    }
}
