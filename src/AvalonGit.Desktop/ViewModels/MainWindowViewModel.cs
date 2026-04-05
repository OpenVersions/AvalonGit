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
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using AvalonGit.Core.IServices;
using AvalonGit.Core.Models;
using AvalonGit.Desktop.Views;

namespace AvalonGit.Desktop.ViewModels;

public class MainWindowViewModel : ViewModelBase, IDisposable
{
    private readonly IGitService _gitService;
    private string _repositoryPath = string.Empty;
    private FileSystemWatcher? _watcher;
    private GitFileStatus? _selectedUnstagedFile;
    private GitFileStatus? _selectedStagedFile;
    private bool _isLoadingDiff;
    private string _commitMessage = string.Empty;
    private string _currentBranch = string.Empty;
    private string _selectedRemote = string.Empty;
    private bool _showErrorDetails;
    private string _errorLog = string.Empty;
    private bool _isPushing;
    private ToastViewModel _toastViewModel = new();

    public string Greeting { get; } = "Welcome to AvalonGit!";
    
    public string RepositoryPath
    {
        get => _repositoryPath;
        set => this.RaiseAndSetIfChanged(ref _repositoryPath, value);
    }

    public ObservableCollection<GitFileStatus> UnstagedFiles { get; } = new();
    public ObservableCollection<GitFileStatus> StagedFiles { get; } = new();
    public ObservableCollection<DiffLine> DiffLines { get; } = new();

    public GitFileStatus? SelectedUnstagedFile
    {
        get => _selectedUnstagedFile;
        set => this.RaiseAndSetIfChanged(ref _selectedUnstagedFile, value);
    }

    public GitFileStatus? SelectedStagedFile
    {
        get => _selectedStagedFile;
        set => this.RaiseAndSetIfChanged(ref _selectedStagedFile, value);
    }

    public bool IsLoadingDiff
    {
        get => _isLoadingDiff;
        set => this.RaiseAndSetIfChanged(ref _isLoadingDiff, value);
    }

    public string CommitMessage
    {
        get => _commitMessage;
        set => this.RaiseAndSetIfChanged(ref _commitMessage, value);
    }

    public string CurrentBranch
    {
        get => _currentBranch;
        set => this.RaiseAndSetIfChanged(ref _currentBranch, value);
    }

    public string SelectedRemote
    {
        get => _selectedRemote;
        set => this.RaiseAndSetIfChanged(ref _selectedRemote, value);
    }

    public bool ShowErrorDetails
    {
        get => _showErrorDetails;
        set => this.RaiseAndSetIfChanged(ref _showErrorDetails, value);
    }

    public string ErrorLog
    {
        get => _errorLog;
        set => this.RaiseAndSetIfChanged(ref _errorLog, value);
    }

    public bool IsPushing
    {
        get => _isPushing;
        set => this.RaiseAndSetIfChanged(ref _isPushing, value);
    }

    public ToastViewModel Toast => _toastViewModel;

    public ObservableCollection<GitRemote> Remotes { get; } = new();
    public ObservableCollection<GitBranch> Branches { get; } = new();
    
    public ReactiveCommand<Unit, Unit> OpenRepositoryCommand { get; }
    public ReactiveCommand<Unit, Unit> CloneRepositoryCommand { get; }
    public ReactiveCommand<string, Unit> StageFileCommand { get; }
    public ReactiveCommand<string, Unit> UnstageFileCommand { get; }
    public ReactiveCommand<Unit, Unit> CommitCommand { get; }
    public ReactiveCommand<Unit, Unit> PushCommand { get; }
    public ReactiveCommand<string, Unit> PushToRemoteCommand { get; }
    public ReactiveCommand<Unit, Unit> AddRemoteCommand { get; }
    
    public MainWindowViewModel(IGitService gitService)
    {
        _gitService = gitService ?? throw new ArgumentNullException(nameof(gitService));
        
        OpenRepositoryCommand = ReactiveCommand.Create(() => { });
        CloneRepositoryCommand = ReactiveCommand.Create(() => { });
        
        //define se as operações git podem ser executadas
        var canExecuteGitOps = this.WhenAnyValue(
            x => x.RepositoryPath, 
            path => !string.IsNullOrWhiteSpace(path)
        );
        
        StageFileCommand = ReactiveCommand.CreateFromTask<string>(
            ExecuteStageAsync, 
            canExecuteGitOps,
            outputScheduler: RxApp.MainThreadScheduler);
            
        UnstageFileCommand = ReactiveCommand.CreateFromTask<string>(
            ExecuteUnstageAsync, 
            canExecuteGitOps,
            outputScheduler: RxApp.MainThreadScheduler);
        
        StageFileCommand.ThrownExceptions.Subscribe(ex => Console.WriteLine($"[Erro Stage] {ex.Message}"));
        UnstageFileCommand.ThrownExceptions.Subscribe(ex => Console.WriteLine($"[Erro Unstage] {ex.Message}"));

        var canCommit = this.WhenAnyValue(
            x => x.RepositoryPath,
            x => x.StagedFiles.Count,
            x => x.CommitMessage,
            (path, stagedCount, message) =>
                !string.IsNullOrWhiteSpace(path) &&
                stagedCount > 0 &&
                !string.IsNullOrWhiteSpace(message)
        );

        CommitCommand = ReactiveCommand.CreateFromTask(
            ExecuteCommitAsync,
            canCommit,
            outputScheduler: RxApp.MainThreadScheduler);

        CommitCommand.ThrownExceptions.Subscribe(ex => Console.WriteLine($"[Erro Commit] {ex.Message}"));

        var canPush = this.WhenAnyValue(
            x => x.RepositoryPath,
            x => x.Remotes.Count,
            x => x.CurrentBranch,
            x => x.IsPushing,
            (path, remoteCount, branch, isPushing) =>
                !string.IsNullOrWhiteSpace(path) &&
                remoteCount > 0 &&
                !string.IsNullOrWhiteSpace(branch) &&
                !isPushing
        );

        PushCommand = ReactiveCommand.CreateFromTask(
            ExecutePushAsync,
            canPush,
            outputScheduler: RxApp.MainThreadScheduler);

        PushToRemoteCommand = ReactiveCommand.CreateFromTask<string>(
            ExecutePushToRemoteAsync,
            canPush,
            outputScheduler: RxApp.MainThreadScheduler);

        AddRemoteCommand = ReactiveCommand.CreateFromTask(
            ExecuteAddRemoteAsync,
            canExecuteGitOps,
            outputScheduler: RxApp.MainThreadScheduler);

        PushCommand.ThrownExceptions.Subscribe(ex => Console.WriteLine($"[Erro Push] {ex.Message}"));
        PushToRemoteCommand.ThrownExceptions.Subscribe(ex => Console.WriteLine($"[Erro Push] {ex.Message}"));
        AddRemoteCommand.ThrownExceptions.Subscribe(ex => Console.WriteLine($"[Erro AddRemote] {ex.Message}"));

        // Garante que apenas um arquivo esteja selecionado por vez e busca o diff
        this.WhenAnyValue(x => x.SelectedUnstagedFile)
            .Where(x => x != null)
            .Subscribe(_ => SelectedStagedFile = null);

        this.WhenAnyValue(x => x.SelectedStagedFile)
            .Where(x => x != null)
            .Subscribe(_ => SelectedUnstagedFile = null);

        this.WhenAnyValue(x => x.SelectedUnstagedFile, x => x.SelectedStagedFile)
            .Throttle(TimeSpan.FromMilliseconds(100))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(async tuple =>
            {
                var (unstaged, staged) = tuple;
                var selected = unstaged ?? staged;
                
                if (selected == null)
                {
                    ClearDiff();
                    return;
                }

                IsLoadingDiff = true;
                var path = RepositoryPath;
                var fileName = selected.FilePath;
                var isStaged = staged != null;

                try
                {
                    var lines = await Task.Run(() => _gitService.GetFileDiff(path, fileName, isStaged));
                    
                    DiffLines.Clear();
                    foreach (var line in lines)
                    {
                        DiffLines.Add(line);
                    }

                    if (!DiffLines.Any())
                    {
                        DiffLines.Add(new DiffLine("Nenhuma alteração de texto detectada.", DiffLineType.Context));
                    }
                }
                catch (Exception ex)
                {
                    DiffLines.Clear();
                    DiffLines.Add(new DiffLine($"Erro ao carregar diff: {ex.Message}", DiffLineType.Context));
                }
                finally
                {
                    IsLoadingDiff = false;
                }
            });

        //monitora mudanças no caminho do repositório
        this.WhenAnyValue(x => x.RepositoryPath)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(async path => 
            {
                SetupWatcher(path);
                await RefreshStatusAsync();
                await RefreshBranchAndRemoteAsync();
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
    
    private void ClearDiff()
    {
        DiffLines.Clear();
        IsLoadingDiff = false;
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

    private async Task ExecuteCommitAsync()
    {
        var path = RepositoryPath;
        var message = CommitMessage;
        
        await Task.Run(() => _gitService.Commit(path, message));
        
        CommitMessage = string.Empty;
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
                ClearDiff();
                SelectedUnstagedFile = null;
                SelectedStagedFile = null;
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
                
                // Limpa seleção se os arquivos não existem mais
                if (SelectedUnstagedFile != null && !UnstagedFiles.Any(x => x.FilePath == SelectedUnstagedFile.FilePath))
                    SelectedUnstagedFile = null;
                if (SelectedStagedFile != null && !StagedFiles.Any(x => x.FilePath == SelectedStagedFile.FilePath))
                    SelectedStagedFile = null;
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

    private async Task ExecutePushAsync()
    {
        var path = RepositoryPath;
        var remoteName = Remotes.Any() ? Remotes.First().Name : "origin";
        var branchName = CurrentBranch;

        if (string.IsNullOrWhiteSpace(remoteName))
        {
            Toast.Show("Erro", "Nenhum remote configurado. Clique em + para adicionar um remote.", true);
            return;
        }

        await ExecutePushToRemoteAsync(remoteName);
    }

    private async Task ExecutePushToRemoteAsync(string remoteName)
    {
        var path = RepositoryPath;
        var branchName = CurrentBranch;

        IsPushing = true;
        ShowErrorDetails = false;
        ErrorLog = string.Empty;

        try
        {
            await Task.Run(() => _gitService.Push(path, remoteName, branchName));
            
            Toast.Show("Sucesso", $"Push realizado para {remoteName}", false);
        }
        catch (InvalidOperationException ex)
        {
            ErrorLog = ex.ToString();
            ShowErrorDetails = true;
            Toast.Show("Erro ao fazer push", ex.Message, true, ex.ToString());
        }
        finally
        {
            IsPushing = false;
        }
    }

    private async Task ExecuteAddRemoteAsync()
    {
        var addRemoteWindow = new AddRemoteWindow();
        var result = await addRemoteWindow.ShowDialog<dynamic>(GetMainWindow());
        
        if (result != null)
        {
            try
            {
                await Task.Run(() => _gitService.AddRemote(RepositoryPath, result.Name, result.Url));
                await RefreshBranchAndRemoteAsync();
                Toast.Show("Sucesso", $"Remote '{result.Name}' adicionado com sucesso.", false);
            }
            catch (InvalidOperationException ex)
            {
                Toast.Show("Erro ao adicionar remote", ex.Message, true, ex.ToString());
            }
        }
    }

    private Avalonia.Controls.Window? GetMainWindow()
    {
        if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow;
        }
        return null;
    }

    private async Task RefreshBranchAndRemoteAsync()
    {
        var path = RepositoryPath;
        if (string.IsNullOrWhiteSpace(path))
            return;

        try
        {
            var branches = await Task.Run(() => _gitService.GetBranches(path).ToList());
            var remotes = await Task.Run(() => _gitService.GetRemotes(path).ToList());
            
            Dispatcher.UIThread.Post(() =>
            {
                Branches.Clear();
                foreach (var branch in branches.Where(b => !b.IsRemote))
                {
                    Branches.Add(branch);
                    if (branch.IsCurrentBranch)
                        CurrentBranch = branch.FriendlyName;
                }

                Remotes.Clear();
                foreach (var remote in remotes)
                {
                    Remotes.Add(remote);
                }

                if (!string.IsNullOrWhiteSpace(SelectedRemote) && Remotes.Any(r => r.Name == SelectedRemote))
                {
                    SelectedRemote = Remotes.First(r => r.Name == SelectedRemote).Name;
                }
                else if (Remotes.Any())
                {
                    SelectedRemote = Remotes.First().Name;
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Erro RefreshBranchAndRemote] {ex.Message}");
        }
    }

    public void Dispose()
    {
        _watcher?.Dispose();
    }
}