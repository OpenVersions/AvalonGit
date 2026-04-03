# 🚀 AvalonGit

**AvalonGit** é um cliente Git multiplataforma, focado em performance e simplicidade, desenvolvido com **C#** e **Avalonia UI**. O objetivo é oferecer uma alternativa open-source leve para desenvolvedores que precisam gerir repositórios sem o overhead de ferramentas baseadas em Electron.

> ⚠️ **Status:** Em Desenvolvimento - MVP em construção

---

## 🛠️ Tech Stack

- **UI Framework:** [Avalonia UI](https://avaloniaui.net/) (Cross-platform XAML)
- **Git Engine:** [LibGit2Sharp](https://github.com/libgit2/libgit2sharp) (Native wrapper)
- **Runtime:** .NET 8.0+
- **Pattern:** MVVM (com ReactiveUI)

## ✨ Funcionalidades

### Core (Prioridade Alta)
- [x] Stage e Unstage de arquivos
- [ ] Operações básicas: Commit, Push, Pull, Fetch
  - [x] Commit
  - [x] Push
  - [ ] Pull
  - [ ] Fetch
- [ ] Visualização de histórico de commits
- [ ] Gerenciamento de Branches (Create/Checkout)

### Expansões Futuras
- [ ] Diff viewer integrado
- [ ] Merge e Rebase
- [ ] Visualização de blame
- [ ] Suporte a submodules

---

## 🚀 Começando

### Pré-requisitos

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)

### Instalação e Execução

```bash
# Clone o repositório
git clone https://github.com/OpenVersions/AvalonGit.git
cd AvalonGit

# Execute o projeto
dotnet run --project AvalonGit.Desktop
```

---

## 🤝 Contribuindo

Contribuições são bem-vindas! Para contribuir:

1. Faça um Fork do projeto
2. Crie uma Branch para sua feature (`git checkout -b feature/NomeDaFeature`)
3. Commit suas mudanças
4. Push para a Branch
5. Abra um Pull Request

---

## 📄 Licença

Distribuído sob a licença **MIT**. Veja `LICENSE` para mais informações.

---

*Desenvolvido com foco em praticidade e performance.*
