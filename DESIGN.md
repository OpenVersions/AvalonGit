# Design System: AvalonGit — Modern High-Density Flat

## 1. Philosophy

**High-Density Information Design** focused on legibility and productivity. Replace soft shadows with thin borders (1px) and color-based layer separation. The interface prioritizes data visibility over decorative elements.

---

## 2. Color Palette (Zinc/Slate Dark)

### Base Colors

| Token | Hex Value | Role |
|-------|-----------|------|
| `BackgroundBase` | `#09090b` | Main application background |
| `SurfacePanel` | `#18181b` | Sidebar, panels, elevated surfaces |
| `BorderDivider` | `#27272a` | Borders, dividers, separators |

### Text Colors

| Token | Hex Value | Role |
|-------|-----------|------|
| `TextPrimary` | `#fafafa` | Main text, headings |
| `TextSecondary` | `#a1a1aa` | Secondary labels, descriptions |
| `TextMuted` | `#71717a` | Disabled text, placeholders |

### Accent Colors

| Token | Hex Value | Role |
|-------|-----------|------|
| `AccentPrimary` | `#3b82f6` | Commit, Push, Primary actions |
| `AccentSuccess` | `#22c55e` | Added lines in diff |
| `AccentDanger` | `#ef4444` | Removed lines in diff |
| `AccentWarning` | `#eab308` | Warnings, modified files |

---

## 3. Geometry

- **CornerRadius**: Fixed 4px for all elements (buttons, inputs, panels)
- **BoxShadow**: ZERO - No shadows allowed
- **Borders**: 1px solid, using `BorderDivider` color
- **Elevation**: Achieved through color differences only (Surface vs Background)

---

## 4. Typography

| Role | Font Stack | Size | Weight |
|------|------------|------|--------|
| UI Text | Inter, sans-serif | 13-14px | 400-500 |
| Code/Diff | JetBrains Mono, Cascadia Code, monospace | 12-13px | 400 |
| Section Headers | Inter | 11px | 600 |
| Badges | Inter | 10-11px | 500-600 |

---

## 5. Spacing System

- **Base Unit**: 4px grid
- **Padding**: Minimal to maximize data visibility
- **Panel Padding**: 8-12px
- **Item Spacing**: 4-8px
- **Component Gap**: 4px

---

## 6. Component Specifications

### Layout Structure

```
Window: 1200x800 (default), 800x600 (min)
├── TopBar: 40px height, SurfacePanel background
├── Main Grid: 280px Sidebar | Content Area
│   ├── Sidebar: SurfacePanel, 1px right border
│   └── Content: BackgroundBase
└── StatusBar: 36px height, SurfacePanel background
```

### Buttons

- **Primary (Commit/Push)**: 
  - Background: `#3b82f6`
  - Text: `#fafafa`
  - CornerRadius: 4px
  - Padding: 8px, 6px
  - Border: none

- **Secondary**:
  - Background: `#18181b`
  - Text: `#fafafa`
  - Border: 1px solid `#27272a`
  - CornerRadius: 4px

- **Icon Button**:
  - Size: 28x28px
  - Background: Transparent
  - Border: none
  - CornerRadius: 4px

### Inputs (TextBox)

- Background: `#18181b`
- Border: 1px solid `#27272a`
- CornerRadius: 4px
- Padding: 8px, 6px
- Text: `#fafafa`
- Placeholder: `#71717a`

### File Lists (Unstaged/Staged)

- Background: `#18181b` (recessed appearance via darker bg)
- Border: 1px solid `#27272a`
- CornerRadius: 4px
- Item Padding: 6px, 4px
- Selected: Background `#27272a`

### Diff Viewer

- Background: `#09090b` (base)
- Line Padding: 4px, 8px
- Added Lines: Background `#14532d` (dark green), Text `#22c55e`
- Removed Lines: Background `#450a0a` (dark red), Text `#ef4444`
- Gutter: Background `#18181b`

### Badges

- Status Badge: 4px corner, 4px padding
- Branch Badge: Surface background, inset appearance via border

---

## 7. Visual Rules

### DO
- Use 1px borders for separation
- Use color hierarchy (Surface vs Background) for elevation
- Keep spacing tight (4-8px) for density
- Use JetBrains Mono for code content
- Apply 4px corner radius everywhere

### DON'T
- NO BoxShadow anywhere
- NO neumorphic effects
- NO gradients
- NO large padding that reduces data visibility
- NO rounded corners beyond 4px

---

## 8. Quick Reference

```
Background Base   : #09090b
Surface/Panels    : #18181b
Borders           : #27272a

Text Primary      : #fafafa
Text Secondary    : #a1a1aa
Text Muted        : #71717a

Primary Accent    : #3b82f6 (Commit/Push)
Success          : #22c55e (Added)
Danger           : #ef4444 (Removed)
Warning          : #eab308 (Modified)

CornerRadius     : 4px (everything)
BoxShadow        : NONE
```

---

## 9. Implementation Notes

- Use SolidColorBrush for all colors
- Apply border as 1px solid on panels
- Create depth through Surface (#18181b) on Background (#09090b)
- All interactive elements have 4px radius
- Diff viewer uses monospace fonts exclusively
