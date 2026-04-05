# AGENTS.md - Guia resumido para agentes

Duração: ~150 linhas

## 1. Build / Clean / Run

```bash
# Build completo (Debug)
dotnet build

# Build Release (Waive warnings)
dotnet build -c Release

# Build apenas do projeto "AvalonGit.Desktop"
dotnet build --project AvalonGit.Desktop
```

## 2. Testes

```bash
# Todos os testes
dotnet test

# Teste único por nome (contém substring)
dotnet test --filter "FullyQualifiedName~<NomeDoTeste>"

# Teste único por classe/método
dotnet test --filter "FullyQualifiedName~<Classe>.<Metodo>"

# Teste por categoria (Unit, Integration, etc.)
dotnet test --filter "Category=Unit"

# Parâmetros avançados
# Paralelização          : dotnet test --parallel
# Cobertura de código   : dotnet test --collect:"XPlat Code Coverage"
# Excluir testes de integração : dotnet test --filter "FullyQualifiedName!~Integration"
```

## 3. Lint / Formatação

- Use o **dotnet format** para ver e aplicar linting e formatação automática.

```bash
# Situação atual (lint)
dotnet format --check

# Aplicar formatação
# --apply aplica as alterações automaticamente
# --dry-run apenas reporta
# --project opcional, se disponível

dotnet format --apply
```

## 4. Convenções de Código C#

### 4.1 Nomenclatura

| Elemento | Convenção | Exemplo |
|----------|-----------|---------|
| Classes | PascalCase | `GitService` |
| Interfaces | PascalCase com prefixo `I` | `IGitService` |
| Métodos | PascalCase | `GetCommits()` |
| Variáveis locais & parâmetros | camelCase | `repositoryPath` |
| Campos privados | camelCase com prefixo `_` | `_repository` |
| Constantes | PascalCase (não UPPER_SNAKE_CASE) | `DefaultBranchName` |

### 4.2 Imports e Namespaces

Order: System namespaces, third‑party, all project namespaces.

```csharp
// System
using System;
using System.Collections.Generic;
using System.Linq;

// Third‑party
using LibGit2Sharp;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ReactiveUI;

// Projeto
using AvalonGit.Services;
using AvalonGit.ViewModels;
using AvalonGit.Models;

// Alias (evitar conflitos)
using Commit = AvalonGit.Models.GitCommit;
```

### 4.3 Formatação de Código

- `{` na mesma linha.
- 4 espaços de indentação (sem tabs).
- 120 caracteres por linha como limite.
- Um espaço em branco entre membros da classe; duas entre classes.
- Uma instrução por linha.
- Use `#nullable enable` nas fontes.

### 4.4 Tipos e Nullability

- Sempre use tipos explícitos; evite `var` em declaraciones de dados.
- Quando {use ? } em tipos que podem ser nulos.
- `throw new ArgumentNullException(nameof(param));` no início dos métodos.

### 4.5 Tratamento de Exceções

- Exceções específicas (ex.: `RepositoryNotFoundException`).
- Mensagens claras e contextualizadas.
- Loge primeiro (`Log.Error(...)`) antes de propagar.

### 4.6 MVVM & ReactiveUI

- ViewModels herdam de `ReactiveObject`.
- Propriedades reativas usam `RaiseAndSetIfChanged`.
- Comandos usam `ReactiveCommand.CreateFromTask`.
- Evite code‑behind; use eventos de UI gravados no ViewModel.

### 4.7 Git Operations

- Sempre dispose `Repository` e `Commands`.
- Use `using` para operações de curta duração.

## 5. Estrutura de Diretórios

```
AvalonGit/
├── src/
│   ├── AvalonGit.Desktop/  # Aplicação principal (App, Views, ViewModels)
│   └── AvalonGit.Core/     # Lógica de negócio (Models, Services, IServices)
├── tests/
│   └── AvalonGit.Tests/    # Testes unitários
├── AvalonGit.slnx          # Solution file
├── AGENTS.md
├── README.md
└── LICENSE
```

## 6. Assinatura de Commit

Use *Conventional Commits*:
- `feat:` nova funcionalidade
- `fix:` correção
- `refactor:` refatoração
- `docs:` documentação
- `test:` testes
- `chore:` manutenção

## 7. Observações

- Nada de regras de Cursor ou Copilot encontradas.
- `dotnet format` aplicável a todo o workspace.
- Testes são executados com .NET 8.0.

---
