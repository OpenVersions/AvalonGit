using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Avalonia.Threading;
using AvalonGit.Core.IServices;
using AvalonGit.Core.Models;

namespace AvalonGit.Desktop.ViewModels;

public class MainWindowViewModel : ViewModelBase, IDisposable
{
    private readonly IGitService _gitService;
    private string _repositoryPath = string.Empty;
    private FileSystemWatcher? _watcher;

    public string Greeting { get; } = "Welcome to AvalonGit!";
    
    public string RepositoryPath
    {
        get => _repositoryPath;
        set => this.RaiseAndSetIfChanged(ref _repositoryPath, value);
    }

    public ObservableCollection<GitFileStatus> UnstagedFiles { get; } = new();
    public ObservableCollection<GitFileStatus> StagedFiles { get; } = new();
    
    public ReactiveCommand<Unit, Unit> OpenRepositoryCommand { get; }
    public ReactiveCommand<Unit, Unit> CloneRepositoryCommand { get; }
    public ReactiveCommand<string, Unit> StageFileCommand { get; }
    public ReactiveCommand<string, Unit> UnstageFileCommand { get; }
    
    public MainWindowViewModel(IGitService gitService)
    {
        _gitService = gitService ?? throw new ArgumentNullException(nameof(gitService));
        
        OpenRepositoryCommand = ReactiveCommand.Create(() => { });
        CloneRepositoryCommand = ReactiveCommand.Create(() => { });
        
        //define se as operações git podem ser executadas
        var canExecuteGitOps = this.WhenAnyValue(
            x => x.RepositoryPath, 
            path => !string.IsNullOrWhiteSpace(path)
        ).ObserveOn(RxApp.MainThreadScheduler);
        
        StageFileCommand = ReactiveCommand.CreateFromTask<string>(ExecuteStageAsync, canExecuteGitOps);
        UnstageFileCommand = ReactiveCommand.CreateFromTask<string>(ExecuteUnstageAsync, canExecuteGitOps);
        
        StageFileCommand.ThrownExceptions.Subscribe(ex => Console.WriteLine($"[Erro Stage] {ex.Message}"));
        UnstageFileCommand.ThrownExceptions.Subscribe(ex => Console.WriteLine($"[Erro Unstage] {ex.Message}"));

        //monitora mudanças no caminho do repositório
        this.WhenAnyValue(x => x.RepositoryPath)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(async path => 
            {
                SetupWatcher(path);
                await RefreshStatusAsync();
            });
    }

    //configura o monitoramento automático de arquivos
    private void SetupWatcher(string path)
    {
        _watcher?.Dispose();
        _watcher = null;

        if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
            return;

        _watcher = new FileSystemWatcher(path)
        {
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
            EnableRaisingEvents = true
        };

        //agrupa eventos de sistema de arquivos para evitar múltiplas atualizações
        Observable.Merge(
            Observable.FromEventPattern<FileSystemEventArgs>(_watcher, nameof(_watcher.Changed)).Select(_ => Unit.Default),
            Observable.FromEventPattern<FileSystemEventArgs>(_watcher, nameof(_watcher.Created)).Select(_ => Unit.Default),
            Observable.FromEventPattern<FileSystemEventArgs>(_watcher, nameof(_watcher.Deleted)).Select(_ => Unit.Default),
            Observable.FromEventPattern<RenamedEventArgs>(_watcher, nameof(_watcher.Renamed)).Select(_ => Unit.Default)
        )
            .Throttle(TimeSpan.FromMilliseconds(500))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(async _ => await RefreshStatusAsync());
    }
    
    private async Task ExecuteStageAsync(string filePath)
    {
        var path = RepositoryPath;
        await Task.Run(() => _gitService.StageFile(path, filePath));
        await RefreshStatusAsync();
    }

    private async Task ExecuteUnstageAsync(string filePath)
    {
        var path = RepositoryPath;
        await Task.Run(() => _gitService.UnstageFile(path, filePath));
        await RefreshStatusAsync();
    }

    private async Task RefreshStatusAsync()
    {
        var path = RepositoryPath;
        if (string.IsNullOrWhiteSpace(path))
        {
            Dispatcher.UIThread.Post(() =>
            {
                UnstagedFiles.Clear();
                StagedFiles.Clear();
            });
            return;
        }

        try
        {
            //executa a operação em segundo plano
            var status = await Task.Run(() => _gitService.GetStatus(path).ToList());
            
            Dispatcher.UIThread.Post(() =>
            {
                UpdateCollection(UnstagedFiles, status.Where(x => x.State == GitFileState.Unstaged));
                UpdateCollection(StagedFiles, status.Where(x => x.State == GitFileState.Staged));
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Erro Refresh] {ex.Message}");
        }
    }

    private static void UpdateCollection(ObservableCollection<GitFileStatus> collection, IEnumerable<GitFileStatus> newItems)
    {
        collection.Clear();
        foreach (var item in newItems)
        {
            collection.Add(item);
        }
    }

    public void Dispose()
    {
        _watcher?.Dispose();
    }
}