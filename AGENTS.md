# AGENTS.md - Guia para Agentes de Código

Este documento fornece orientações para agentes que trabalham no projeto AvalonGit.

---

## Visão Geral do Projeto

- **Tipo:** Aplicativo desktop multiplataforma
- **Tech Stack:** C# / .NET 8.0+ / Avalonia UI / LibGit2Sharp
- **Pattern:** MVVM com ReactiveUI
- **Status:** Em desenvolvimento inicial

---

## Comandos de Build e Execução

### Build do Projeto

```bash
# Build completo (Debug)
dotnet build

# Build Release
dotnet build -c Release

# Build de projeto específico
dotnet build --project AvalonGit.Desktop
```

### Execução

```bash
# Run padrão (Debug)
dotnet run --project AvalonGit.Desktop

# Run Release
dotnet run --project AvalonGit.Desktop -c Release
```

### Limpeza

```bash
# Limpar build anterior
dotnet clean

# Limpar e rebuild
dotnet clean && dotnet build
```

---

## Comandos de Testes

### Executar Todos os Testes

```bash
# Executar todos os testes
dotnet test

# Comverbosidade detalhada
dotnet test -v detailed
```

### Executar Teste Único

```bash
# Por nome de teste (conains)
dotnet test --filter "FullyQualifiedName~NomeDoTeste"

# Por nome de método específico
dotnet test --filter "FullyQualifiedName~NomeDaClasse.NomeDoMetodo"

# Por trait/categoria
dotnet test --filter "Category=Unit"
```

### Opções Adicionais de Teste

```bash
# Executar em paralelo
dotnet test --parallel

# Coverage
dotnet test --collect:"XPlat Code Coverage"

# Executar testes que correspondem ao padrão
dotnet test --filter "FullyQualifiedName!~Integration"
```

---

## Code Style Guidelines

### Convenções de Nomenclatura

#### Classes e Structs
- PascalCase
- Substantivos ou frases substantivas
- Exemplos: `GitService`, `CommitViewModel`, `MainWindow`

#### Interfaces
- PascalCase com prefixo `I`
- Exemplos: `IGitService`, `ICommitRepository`

#### Métodos
- PascalCase
- Verbos ou frases verbais
- Exemplos: `GetCommits()`, `CreateBranch()`, `PushChanges()`

#### Propriedades
- PascalCase
- Exemplos: `CurrentBranch`, `RepositoryPath`, `IsLoading`

#### Campos Privados
- camelCase com prefixo `_`
- Exemplos: `_repository`, `_currentBranch`, `_commitList`

#### Parâmetros e Variáveis Locais
- camelCase
- Exemplos: `repositoryPath`, `branchName`, `commit`

#### Constantes
- PascalCase (não UPPER_SNAKE_CASE)
- Exemplos: `DefaultBranchName`, `MaxCommitHistory`

### Imports e Namespaces

```csharp
// Ordenação recomendada (nessa ordem):
// 1. System namespaces
using System;
using System.Collections.Generic;
using System.Linq;

// 2. Third-party (LibGit2Sharp, Avalonia, ReactiveUI)
using LibGit2Sharp;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ReactiveUI;

// 3. Projetos internos
using AvalonGit.Services;
using AvalonGit.ViewModels;
using AvalonGit.Models;

// 4. Alias para evitar conflitos
using Commit = AvalonGit.Models.GitCommit;
```

### Formatação de Código

#### Blocos de Código
- Usar `{` na mesma linha
- Indentação: 4 espaços (não tabs)
- Máximo de 120 caracteres por linha

```csharp
public void ProcessCommit(GitCommit commit, Repository repository)
{
    if (commit == null)
        throw new ArgumentNullException(nameof(commit));

    var branch = repository.Head;
    // código...
}
```

#### Linhas em Branco
- Uma linha em branco entre membros de classe
- Duas linhas em branco entre classes

#### Declarções
- Uma variável por linha
- Inicializar propriedades no construtor ou com inicializador de objeto

### Tipos e Annotations

#### Tipos Explícitos
- Sempre usar tipo explícito (não `var`) exceto quando o tipo for óbvio
- exceptions: `var result = repository.Lookup<Commit>(sha);`

#### Nullability
- Habilitar nullable reference types (`#nullable enable`)
- Usar `?` para tipos que podem ser null
- Preferir `string.Empty` a `null` para strings vazias

#### Propriedades
- Preferir auto-properties quando não houver lógica
- Usar init-only setters quando apropriado

```csharp
// Bom
public string RepositoryPath { get; init; }

// Quando precisa de lógica
private string _repositoryPath;
public string RepositoryPath
{
    get => _repositoryPath;
    set => this.RaiseAndSetIfChanged(ref _repositoryPath, value);
}
```

### Tratamento de Erros

#### Exceções
- Usar exceções para condições excepcionais
- Nomes de exceção descritivos
- Incluir mensagem clara e útil

```csharp
throw new InvalidOperationException(
    $"Não foi possível inicializar o repositório em: {path}");
```

#### Try-Catch
- Catch específicos primeiro, genérico por último
- Logar exceptions antes de propagar
- Não capturar exceptions silenciosamente

```csharp
try
{
    repository = new Repository(repoPath);
}
catch (RepositoryNotFoundException ex)
{
    Log.Error(ex, "Repositório não encontrado em {Path}", repoPath);
    throw;
}
catch (Exception ex)
{
    Log.Error(ex, "Erro ao abrir repositório");
    throw;
}
```

#### Validação
- Validar parâmetros no início dos métodos
- Usar `ArgumentNullException` para parâmetros obrigatórios
- Usar `ArgumentException` para valores inválidos

### Padrões MVVM com ReactiveUI

#### ViewModels
- Herdar de `ReactiveObject`
- Usar `RaiseAndSetIfChanged` para propriedades reativas
- Implementar `IViewFor<T>` para binding com Views

```csharp
public class MainWindowViewModel : ReactiveObject
{
    private string _repositoryPath;
    public string RepositoryPath
    {
        get => _repositoryPath;
        set => this.RaiseAndSetIfChanged(ref _repositoryPath, value);
    }
}
```

#### Commands
- Usar `ReactiveCommand` para comandos reativos
- Criar comandos no construtor do ViewModel

```csharp
public class MainWindowViewModel : ReactiveObject
{
    public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

    public MainWindowViewModel()
    {
        RefreshCommand = ReactiveCommand.CreateFromTask(RefreshAsync);
    }
}
```

### Boas Práticas

#### Git Operations
- Sempre dispose `Repository` e `Commands` objetos
- Usar `using` para operações de curta duração

```csharp
using (var repo = new Repository(path))
{
    var commit = repo.Head.Tip;
    // operações...
}
```

#### UI/Avalonia
- Separar Views (.axaml) de ViewModels
- Usar binding para comunicação
- Evitar code-behind exceto para eventos de UI

#### performance
- Lazy loading para listas grandes
- Virtualização de lists em UI
- Async/await para operações de I/O

---

## Estrutura de Diretórios Sugerida

```
AvalonGit/
├── src/
│   ├── AvalonGit.Desktop/      # Projeto principal (App, Views)
│   ├── AvalonGit.Core/         # Lógica de negócio (Services, Models)
│   └── AvalonGit.Tests/        # Testes unitários
├── AGENTS.md
├── README.md
└── LICENSE
```

---

## Git Rules

### Commits

- UsarConventional Commits:
  - `feat:` nova funcionalidade
  - `fix:` bug fix
  - `refactor:` refatoração
  - `docs:` documentação
  - `test:` testes
  - `chore:` manutenção

- Manter commits pequenos e focados

### Branches

- `feature/` para novas funcionalidades
- `fix/` para correções
- `refactor/` para refatorações

### Pull Requests

- PRs pequenos e focados
- Descrição clara do problema e solução
- Verificarbuild e testes antes de abrir

---

## Notas Importantes

1. **Este projeto está em desenvolvimento inicial** - muitas convenções podem evoluir conforme o projeto cresce.

2. **Sem regras de Cursor/Copilot** - não foram encontradas configurações específicas.

3. **dotnet SDK 8.0+** é necessário para build e execução.

4. **LibGit2Sharp** é usado para todas operações Git - evitar chamar git.exe diretamente.
