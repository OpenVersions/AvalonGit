# Design System: AvalonGit — Soft UI (Neomorphism)

## 1. Visual Theme & Atmosphere

AvalonGit adota o **Neomorfismo (Soft UI)** — uma estética onde os elementos parecem extrudados ou esculpidos na superfície, criando profundidade sutil através de sombras duplas (luz + sombra) sobre fundos monocromáticos. O resultado é uma interface que parece "macia" ao toque, com botões que parecem pressionáveis e cards que flutuam organicamente.

O tema é **dark-first**, construído sobre um fundo slate profundo (`#1A1D23`) que evita o preto puro, criando uma base quente e sofisticada. A paleta é predominantemente monocromática com acentos estratégicos em azul (`#3B82F6`) para ações primárias, verde (`#4ADE80`) para adições no diff, e vermelho (`#F87171`) para remoções.

A tipografia usa **Inter** como fonte principal (UI, labels, botões) e **Cascadia Code / JetBrains Mono** para conteúdo de código (diff viewer, logs). Essa dualidade cria hierarquia clara entre interface e conteúdo técnico.

O espaçamento segue um sistema de **8px** com granularidade fina de **4px** para micro-ajustes. Border radius de **12px** para containers principais e **8px** para componentes internos cria uma estética suave e orgânica.

**Key Characteristics:**
- Neomorfismo dark sobre fundo `#1A1D23` (slate aquecido, não preto puro)
- Sombras duplas: luz superior-esquerda (`#22262E`) + sombra inferior-direita (`#121418`)
- Monocromático com acentos funcionais (azul, verde, vermelho)
- Inter para UI, Cascadia Code/JetBrains Mono para código
- Border radius: 12px (containers), 8px (componentes), 6px (inputs)
- Sistema de espaçamento 8px com granularidade 4px
- Transições suaves de 200ms em todas as interações
- Sombras internas (inset) para inputs e áreas "rebaixadas"

---

## 2. Color Palette & Roles

### Primary (Neomorphic Base)

| Token | Hex Value | Role |
|-------|-----------|------|
| `BaseColor` | `#1A1D23` | Fundo principal da aplicação — slate aquecido |
| `SurfaceColor` | `#1E2128` | Superfícies elevadas (sidebar, topbar, footer) — ligeiramente mais claro |
| `SurfaceLightColor` | `#242830` | Superfícies de segundo nível (cards, painéis) |
| `SurfaceDarkColor` | `#16181D` | Superfícies rebaixadas (inputs, áreas inset) |

### Shadows (Neomorphic Engine)

| Token | Hex Value | Role |
|-------|-----------|------|
| `ShadowLight` | `#22262E` | Sombra clara — simula luz vindo de cima-esquerda |
| `ShadowDark` | `#121418` | Sombra escura — simula profundidade embaixo-direita |
| `ShadowLightStrong` | `#282D36` | Sombra clara forte — para elevação maior |
| `ShadowDarkStrong` | `#0E1014` | Sombra escura forte — para elevação maior |

### Accent Colors

| Token | Hex Value | Role |
|-------|-----------|------|
| `PrimaryColor` | `#3B82F6` | Ações primárias, links, destaques — azul vibrante |
| `PrimaryHoverColor` | `#60A5FA` | Hover em elementos primários — azul mais claro |
| `PrimaryActiveColor` | `#2563EB` | Active/pressed em elementos primários — azul mais escuro |
| `SuccessColor` | `#4ADE80` | Linhas adicionadas no diff, estados de sucesso |
| `DangerColor` | `#F87171` | Linhas removidas no diff, estados de erro |
| `WarningColor` | `#FBBF24` | Avisos, estados de atenção |

### Text Colors

| Token | Hex Value | Role |
|-------|-----------|------|
| `TextPrimaryColor` | `#E8EAED` | Texto principal — branco suave, não puro |
| `TextSecondaryColor` | `#9CA3AF` | Texto secundário, labels, placeholders |
| `TextMutedColor` | `#6B7280` | Texto desabilitado, metadados |
| `TextOnAccentColor` | `#FFFFFF` | Texto sobre fundos de acento |

### Diff-Specific Colors

| Token | Hex Value | Role |
|-------|-----------|------|
| `DiffAddedBackground` | `#15291E` | Fundo de linhas adicionadas — verde escuro sutil |
| `DiffAddedForeground` | `#4ADE80` | Texto de linhas adicionadas |
| `DiffRemovedBackground` | `#2C161A` | Fundo de linhas removidas — vermelho escuro sutil |
| `DiffRemovedForeground` | `#F87171` | Texto de linhas removidas |
| `DiffHeaderBackground` | `#1E2128` | Fundo de cabeçalhos de diff |
| `DiffHeaderForeground` | `#3B82F6` | Texto de cabeçalhos de diff |
| `DiffGutterBackground` | `#16181D` | Fundo da gutter (line numbers) |
| `DiffGutterForeground` | `#4B5563` | Texto da gutter |
| `DiffIndicatorForeground` | `#6B7280` | Indicadores +/- |

### Border

| Token | Hex Value | Role |
|-------|-----------|------|
| `BorderSubtleColor` | `rgba(255, 255, 255, 0.04)` | Bordas sutis entre seções |
| `BorderDefaultColor` | `rgba(255, 255, 255, 0.08)` | Bordas padrão de componentes |
| `BorderStrongColor` | `rgba(255, 255, 255, 0.12)` | Bordas fortes, focus rings |

---

## 3. Typography Rules

### Font Families

| Role | Font Stack |
|------|------------|
| **UI Font** | `Inter, -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif` |
| **Code Font** | `"Cascadia Code", "JetBrains Mono", "Fira Code", Consolas, "Courier New", monospace` |

### Hierarchy

| Role | Size | Weight | Line Height | Letter Spacing | Notes |
|------|------|--------|-------------|----------------|-------|
| Page Title | 28px | 700 | 1.30 | -0.5px | Greeting na welcome screen |
| Section Title | 16px | 600 | 1.40 | 0.5px | Headers de seção (UPPERCASE) |
| Body | 14px | 400 | 1.50 | 0 | Texto padrão, descrições |
| Body Small | 13px | 400 | 1.50 | 0 | Texto auxiliar, tooltips |
| Code | 13px | 400 | 1.60 | 0 | Conteúdo de diff, logs |
| Caption | 11px | 500 | 1.40 | 0.8px | Labels, badges, metadados |
| Button | 13px | 500 | 1.00 | 0.3px | Texto de botões |
| Input | 14px | 400 | 1.50 | 0 | Texto de inputs |

### Principles
- **UI vs Code separation**: Inter para toda interface, monospace apenas para conteúdo de código
- **Weight-driven hierarchy**: 700 para títulos, 600 para seções, 500 para interativos, 400 para corpo
- **Tight line-height para interação**: Botões e labels compactos usam 1.00–1.40
- **Generous line-height para leitura**: Corpo de texto e código usam 1.50–1.60
- **Uppercase para seções**: Section titles usam `TextTransform="Uppercase"` com letter-spacing 0.5px

---

## 4. Component Stylings

### Neomorphic Shadow System

O coração do design neomórfico são as sombras duplas. Cada nível de elevação combina uma sombra clara (top-left) e uma sombra escura (bottom-right):

```
Elevated (raised):
  BoxShadow: -4 -4 8 0 #22262E, 4 4 8 0 #121418

Flat (default):
  BoxShadow: -2 -2 4 0 #22262E, 2 2 4 0 #121418

Pressed (inset):
  BoxShadow: inset -2 -2 4 0 #22262E, inset 2 2 4 0 #121418

Input (recessed):
  BoxShadow: inset -3 -3 6 0 #22262E, inset 3 3 6 0 #121418
```

### Buttons

**Primary Button (Commit, Push)**
- Background: `#1E2128` (Surface)
- Text: `#E8EAED` (TextPrimary)
- Padding: `10, 8`
- CornerRadius: `8`
- FontSize: `13`, FontWeight: `500`
- BoxShadow (flat): `-2 -2 4 0 #22262E, 2 2 4 0 #121418`
- BoxShadow (pointerover): `-3 -3 6 0 #282D36, 3 3 6 0 #0E1014`
- BoxShadow (pressed): `inset -2 -2 4 0 #22262E, inset 2 2 4 0 #121418`
- Use: Ações primárias (Commit, Push)

**Accent Button (Primary CTA)**
- Background: `#3B82F6` (Primary)
- Text: `#FFFFFF` (TextOnAccent)
- Padding: `10, 8`
- CornerRadius: `8`
- BoxShadow: `-2 -2 6 0 rgba(59,130,246,0.3), 2 2 8 0 rgba(0,0,0,0.3)`
- BoxShadow (pointerover): `-3 -3 8 0 rgba(96,165,250,0.4), 3 3 10 0 rgba(0,0,0,0.3)`
- BoxShadow (pressed): `inset 0 2 4 0 rgba(0,0,0,0.2)`
- Use: Ação principal em destaque

**Icon Button (Stage/Unstage, Window Controls)**
- Background: `#1E2128` (Surface)
- Padding: `6`
- CornerRadius: `8`
- Width/Height: `32`
- BoxShadow (flat): `-2 -2 4 0 #22262E, 2 2 4 0 #121418`
- BoxShadow (pressed): `inset -1 -1 3 0 #22262E, inset 1 1 3 0 #121418`
- Use: Stage/unstage, window controls, ações secundárias com ícone

**Close Button (Window)**
- Background: `#1E2128`
- Width: `46`, Height: `32`
- BoxShadow (pointerover): fundo `#E81123` (Windows-style red)
- Use: Fechar janela

**Ghost Button (Menu, Links)**
- Background: `Transparent`
- Text: `#E8EAED`
- Padding: `12, 8`
- CornerRadius: `8`
- BoxShadow (pointerover): `#FFFFFFFF` a 8% opacidade
- Use: Menu items, links, ações secundárias

### Inputs

**TextBox (Commit Message, Forms)**
- Background: `#16181D` (SurfaceDark — recessed)
- Text: `#E8EAED`
- Border: `1px solid rgba(255, 255, 255, 0.06)`
- Padding: `12, 10`
- CornerRadius: `8`
- BoxShadow (default): `inset -3 -3 6 0 #22262E, inset 3 3 6 0 #121418`
- BoxShadow (focus): `inset -3 -3 6 0 #22262E, inset 3 3 6 0 #121418` + border `#3B82F6`
- FontSize: `14`
- Watermark: `#6B7280`
- Use: Commit message, formulários

### Cards / Panels

**Sidebar Panel**
- Background: `#1E2128` (Surface)
- Border: `1px solid rgba(255, 255, 255, 0.04)` à direita
- BoxShadow: `-4 -4 8 0 #22262E, 4 4 8 0 #121418`
- CornerRadius: `0` (full height, flush to edges)
- Use: Sidebar de mudanças

**Content Panel**
- Background: `#1A1D23` (Base)
- Use: Área principal de conteúdo

**Toast / Notification**
- Background: `#1E2128` (Surface)
- Border: `1px solid rgba(255, 255, 255, 0.06)`
- Padding: `16`
- CornerRadius: `12`
- BoxShadow: `-6 -6 12 0 #22262E, 6 6 12 0 #121418`
- Use: Notificações toast

**Modal / Error Panel**
- Background: `#1E2128` (Surface)
- Border: `1px solid rgba(255, 255, 255, 0.08)`
- Padding: `20`
- CornerRadius: `12`
- BoxShadow: `-8 -8 16 0 #22262E, 8 8 16 0 #121418`
- Use: Painéis modais, detalhes de erro

### ListBox Items

**File Item (Unstaged/Staged)**
- Background: `Transparent`
- Padding: `8, 6`
- CornerRadius: `6`
- BoxShadow (pointerover): `-1 -1 3 0 #22262E, 1 1 3 0 #121418`
- BoxShadow (selected): `inset -2 -2 4 0 #22262E, inset 2 2 4 0 #121418`
- Use: Items de lista de arquivos

**Diff Line**
- Padding: `0, 2`
- Classes `.Addition`, `.Deletion`, `.Header`, `.Info` com backgrounds específicos
- Use: Linhas do diff viewer

### Badges / Tags

**Version Badge**
- Background: `#3B82F6` (Primary)
- Text: `#FFFFFF`
- Padding: `4, 2`
- CornerRadius: `4`
- FontSize: `11`, FontWeight: `600`
- Use: Versão do app, labels

**Branch Badge**
- Background: `#16181D` (SurfaceDark)
- Text: `#E8EAED`
- Padding: `4, 6`
- CornerRadius: `6`
- FontSize: `12`, FontWeight: `500`
- BoxShadow: `inset -2 -2 4 0 #22262E, inset 2 2 4 0 #121418`
- Use: Nome do branch na status bar

### Menu Flyout

**MenuFlyoutPresenter**
- Background: `#1E2128` (Surface)
- Border: `1px solid rgba(255, 255, 255, 0.06)`
- CornerRadius: `12`
- Padding: `6`
- BoxShadow: `-6 -6 12 0 #22262E, 6 6 12 0 #121418`
- Use: Menu dropdown do hamburger

**MenuItem**
- Padding: `10, 8`
- CornerRadius: `8`
- BoxShadow (pointerover): `-1 -1 3 0 #22262E, 1 1 3 0 #121418`
- Use: Items do menu

### Separator
- Background: `rgba(255, 255, 255, 0.06)`
- Height: `1px`
- Margin: `8, 4`
- Use: Divisores no menu e entre seções

---

## 5. Layout Principles

### Spacing System

| Scale | Value | Use |
|-------|-------|-----|
| `xs` | 4px | Micro gaps, icon padding |
| `sm` | 8px | Gaps entre items, padding interno |
| `md` | 16px | Padding de containers, gaps entre seções |
| `lg` | 24px | Margens de layout, espaçamento de cards |
| `xl` | 32px | Espaçamento entre áreas principais |
| `2xl` | 48px | Espaçamento de seções grandes |

### Grid & Container

| Area | Width | Notes |
|------|-------|-------|
| Window Default | 1200x800 | Tamanho padrão |
| Window Min | 800x600 | Tamanho mínimo |
| Sidebar | 300px | Largura fixa |
| Content | Flexible | Preenche espaço restante |
| Top Bar | 48px height | Menu + window controls |
| Status Bar | 36px height | Branch info + push |

### Whitespace Philosophy
- **Respiração entre elementos**: Neomorfismo depende de espaço para que as sombras funcionem. Elementos muito próximos perdem o efeito 3D.
- **Separação por profundidade, não por bordas**: Use sombras e elevação para diferenciar seções, não bordas sólidas.
- **Padding generoso**: Containers internos têm padding mínimo de 16px para permitir que as sombras se espalhem.

### Border Radius Scale
- `4px`: Badges, tags, elementos pequenos
- `6px`: ListBox items, elementos de lista
- `8px`: Botões, inputs, cards internos (padrão)
- `12px`: Containers principais, modais, toast, menu flyout
- `0`: Sidebar full-height (flush às bordas)

---

## 6. Depth & Elevation

| Level | Name | BoxShadow | Use |
|-------|------|-----------|-----|
| 0 | Flat | `-2 -2 4 0 #22262E, 2 2 4 0 #121418` | Botões em repouso, ícones |
| 1 | Raised | `-4 -4 8 0 #22262E, 4 4 8 0 #121418` | Cards, sidebar, painéis |
| 2 | Elevated | `-6 -6 12 0 #22262E, 6 6 12 0 #121418` | Toast, menu flyout |
| 3 | Floating | `-8 -8 16 0 #22262E, 8 8 16 0 #121418` | Modais, error panels |
| -1 | Recessed (inset) | `inset -3 -3 6 0 #22262E, inset 3 3 6 0 #121418` | Inputs, áreas de digitação |
| -2 | Pressed (inset) | `inset -2 -2 4 0 #22262E, inset 2 2 4 0 #121418` | Botões pressionados, items selecionados |

**Shadow Philosophy:**
- Luz vem de **cima-esquerda** (135°): sombra clara no topo-esquerda, sombra escura embaixo-direita
- Distância da luz: 4–8px (sutil, não dramática)
- Blur: 4–16px (suave, não nítido)
- Spread: 0 (sem expansão, apenas blur)
- Opacidade implícita via cor: as sombras já são cores escuras/claras, não preto/branco transparente

**Depth Transitions:**
- Hover: +2px em distância e blur (ex: 4→6, 8→10)
- Pressed: muda para inset (elemento "afunda")
- Focus: adiciona border color `#3B82F6` (azul)

---

## 7. Interaction & Motion

### Hover States

| Element | Default | Hover |
|---------|---------|-------|
| Primary Button | shadow flat | shadow raised + background `#242830` |
| Accent Button | shadow com glow azul | glow mais intenso + background `#60A5FA` |
| Icon Button | shadow flat | shadow raised |
| Ghost Button | transparente | `rgba(255,255,255,0.08)` |
| Close Button | transparente | `#E81123` |
| ListBox Item | transparente | shadow flat sutil |
| Link | `#E8EAED` | `#3B82F6` + underline |

### Focus States
- Border: `1px solid #3B82F6` (azul primário)
- BoxShadow: adicional `0 0 0 3px rgba(59, 130, 246, 0.2)` (glow azul sutil)
- Transição: 150ms ease-out

### Pressed States
- BoxShadow muda para **inset** (elemento "afunda" na superfície)
- Background escurece levemente (`#1E2128` → `#16181D`)
- Transição: 100ms ease-out

### Transitions
- **Duration**: 200ms para todas as transições visuais
- **Easing**: `cubic-bezier(0.4, 0, 0.2, 1)` (Material Design standard)
- **Properties**: `Background`, `BoxShadow`, `BorderBrush`, `Opacity`, `Foreground`
- **Transform**: nenhum scale ou translate — neomorfismo é sobre profundidade, não movimento

### Disabled States
- Opacity: `0.5`
- BoxShadow: removido (elemento "plano")
- Cursor: padrão (não hand)

---

## 8. Responsive Behavior

### Breakpoints

| Name | Width | Key Changes |
|------|-------|-------------|
| Compact | <900px | Sidebar colapsa para ícones (60px) |
| Standard | 900-1200px | Layout padrão (300px sidebar) |
| Expanded | >1200px | Sidebar expande para 340px |

### Touch Targets
- Botões: mínimo 32x32px (icon buttons)
- Botões com texto: mínimo 44px de altura
- Inputs: padding mínimo de 10px vertical

### Collapsing Strategy
- Sidebar: 300px → 60px (só ícones) em telas <900px
- Welcome greeting: 28px → 22px
- Status bar: empilha verticalmente em telas muito estreitas
- Toast: largura máxima de 350px → 100% - 32px em mobile

---

## 9. Accessibility Considerations

### Contrast
- Neomorfismo tem contraste inherentemente baixo entre elementos e fundo
- **Texto sobre superfície**: mínimo 4.5:1 (WCAG AA) — `#E8EAED` sobre `#1E2128` = ~12:1 ✓
- **Texto secundário**: `#9CA3AF` sobre `#1E2128` = ~5.5:1 ✓
- **Placeholders**: `#6B7280` sobre `#16181D` = ~4.2:1 ( borderline — considerar `#7C8390` )

### Focus Indicators
- Sempre visíveis, nunca removidos
- Border azul `#3B82F6` + glow externo
- Não depender apenas de sombra para indicar foco

### Reduced Motion
- Respeitar `prefers-reduced-motion` (quando aplicável)
- Transições de 200ms são suficientemente curtas para não causar desconforto

---

## 10. Agent Prompt Guide

### Quick Color Reference
```
Base (page bg)       : #1A1D23
Surface (panels)     : #1E2128
Surface Light (cards): #242830
Surface Dark (inputs): #16181D

Shadow Light         : #22262E
Shadow Dark          : #121418

Text Primary         : #E8EAED
Text Secondary       : #9CA3AF
Text Muted           : #6B7280

Primary (accent)     : #3B82F6
Success              : #4ADE80
Danger               : #F87171
Warning              : #FBBF24

Border Subtle        : rgba(255,255,255,0.04)
Border Default       : rgba(255,255,255,0.08)
Border Strong        : rgba(255,255,255,0.12)
```

### Neomorphic Shadow Recipes

**Elevated Button (flat → raised → pressed):**
```xml
<!-- Flat (default) -->
BoxShadow="-2 -2 4 0 #22262E, 2 2 4 0 #121418"

<!-- Raised (hover) -->
BoxShadow="-4 -4 8 0 #22262E, 4 4 8 0 #121418"

<!-- Pressed (active) -->
BoxShadow="inset -2 -2 4 0 #22262E, inset 2 2 4 0 #121418"
```

**Recessed Input:**
```xml
BoxShadow="inset -3 -3 6 0 #22262E, inset 3 3 6 0 #121418"
```

**Card / Panel:**
```xml
BoxShadow="-4 -4 8 0 #22262E, 4 4 8 0 #121418"
```

### Example Component Prompts

**"Create a neomorphic primary button:"**
```
Background: #1E2128, text: #E8EAED at 13px weight 500.
Padding: 10,8. CornerRadius: 8.
Flat shadow: -2 -2 4 0 #22262E, 2 2 4 0 #121418.
Hover shadow: -4 -4 8 0 #22262E, 4 4 8 0 #121418 + bg #242830.
Pressed shadow: inset -2 -2 4 0 #22262E, inset 2 2 4 0 #121418.
Transition: 200ms cubic-bezier(0.4, 0, 0.2, 1).
```

**"Create a recessed text input:"**
```
Background: #16181D. Text: #E8EAED at 14px. Watermark: #6B7280.
Border: 1px solid rgba(255,255,255,0.06). Padding: 12,10. CornerRadius: 8.
Shadow: inset -3 -3 6 0 #22262E, inset 3 3 6 0 #121418.
Focus: border #3B82F6 + outer glow 0 0 0 3px rgba(59,130,246,0.2).
```

**"Create a sidebar panel:"**
```
Background: #1E2128. Full height, flush left.
Border-right: 1px solid rgba(255,255,255,0.04).
Shadow: -4 -4 8 0 #22262E, 4 4 8 0 #121418.
Section headers: 11px weight 600, uppercase, letter-spacing 0.5px, color #9CA3AF.
```

**"Create a file list item:"**
```
Transparent bg, padding 8,6. CornerRadius: 6.
Hover: shadow -1 -1 3 0 #22262E, 1 1 3 0 #121418.
Selected: inset shadow -2 -2 4 0 #22262E, inset 2 2 4 0 #121418.
Text: #E8EAED at 14px. Action button: 32x32 icon button with flat shadow.
```

### Iteration Guide

1. **Sombras primeiro**: No neomorfismo, a sombra É o design. Defina sombras antes de tudo.
2. **Luz consistente**: Sempre topo-esquerda (sombra clara) + baixo-direita (sombra escura).
3. **Espaço é essencial**: Elementos precisam de respiro para as sombras funcionarem. Mínimo 8px entre componentes.
4. **Dois estados de sombra**: flat (repouso) e raised (hover). Pressed usa inset.
5. **Inputs são sempre inset**: Diferente de botões, inputs são "rebaixados" por padrão.
6. **Cores monocromáticas**: Fundo e superfícies são variações do mesmo tom. Acentos são apenas para ação.
7. **Border radius suave**: 8px padrão, 12px para containers grandes. Nada de pills (radius infinito).
8. **Transições sutis**: 200ms, sem transforms. Apenas background, shadow, border, opacity.
9. **Contraste do texto**: Nunca sacrificar legibilidade pelo efeito visual. Texto sempre >= 4.5:1.
10. **Testar sem sombras**: Se a interface não funciona sem sombras, o layout está errado. Sombras são enhancement, não estrutura.

---

## 11. Migration Plan

### Phase 1: Foundation (Design Tokens)
- [ ] Atualizar `AvalonGitTheme.axaml` com nova paleta neomórfica
- [ ] Adicionar tokens de sombra (não existem atualmente)
- [ ] Adicionar tokens de spacing e border radius
- [ ] Consolidar `PrimaryColor` e `AccentColor` (atualmente duplicados)
- [ ] Adicionar tokens de estado (hover, active, disabled)

### Phase 2: Component Styles
- [ ] Reescrever `AvalonGitStyles.axaml` com estilos neomórficos
- [ ] Criar classe `.neoButton` (primary neomorphic button)
- [ ] Criar classe `.neoIconButton` (icon button com sombras)
- [ ] Criar classe `.neoInput` (input rebaixado)
- [ ] Criar classe `.neoCard` (card elevado)
- [ ] Criar classe `.neoPanel` (panel com sombra)
- [ ] Atualizar `.titleBarButton` com sombras neomórficas
- [ ] Atualizar `.menuButton` com sombras neomórficas
- [ ] Adicionar estilos para `.neoListBoxItem`

### Phase 3: MainWindow
- [ ] Aplicar sombras neomórficas na sidebar
- [ ] Aplicar sombras neomórficas nos botões de stage/unstage
- [ ] Aplicar sombras neomórficas no input de commit
- [ ] Aplicar sombras neomórficas no botão commit
- [ ] Aplicar sombras neomórficas no botão push
- [ ] Aplicar sombras neomórficas no menu flyout
- [ ] Aplicar sombras neomórficas no toast
- [ ] Aplicar sombras neomórficas no error panel
- [ ] Atualizar tipografia (Inter para UI, monospace para código)
- [ ] Adicionar empty states para listas de arquivos
- [ ] Corrigir posicionamento do toast e error panel (estão com Grid attached properties erradas)

### Phase 4: Secondary Windows
- [ ] Atualizar `AboutWindow.axaml` com estilo neomórfico
- [ ] Atualizar `AddRemoteWindow.axaml` com estilo neomórfico
- [ ] Corrigir MVVM do `AddRemoteWindow` (atualmente usa code-behind)

### Phase 5: Polish
- [ ] Adicionar transições suaves (200ms) em todas as interações
- [ ] Adicionar focus indicators azuis
- [ ] Adicionar loading state visual no botão push
- [ ] Adicionar ícone ao toast (atualmente null)
- [ ] Verificar contraste e acessibilidade
- [ ] Testar em diferentes tamanhos de janela

---

## 12. File Structure (Post-Migration)

```
src/AvalonGit.Desktop/
├── Themes/
│   ├── AvalonGitTheme.axaml      # Design tokens (colors, shadows, spacing, radius)
│   └── AvalonGitStyles.axaml     # Component styles (neoButton, neoInput, etc.)
├── Views/
│   ├── MainWindow.axaml          # Main window com neomorfismo aplicado
│   ├── ToastPanel.axaml          # Toast com sombras neomórficas
│   ├── AboutWindow.axaml         # About dialog atualizado
│   └── AddRemoteWindow.axaml     # Dialog atualizado + MVVM fix
└── ...
```
