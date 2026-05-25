# 🎨 Preview Visual - Sistema de Perfil de Coordenador

## 📊 Página de Perfil - `/Coordenador/Perfil`

### Navbar
```
┌─────────────────────────────────────────────────────────────┐
│ 📋 Sistema de Segunda Chamada - Coordenador    Painel Meu P...│
│                                        Matrícula: 999999 | Sair │
└─────────────────────────────────────────────────────────────┘
```

### Dados do Coordenador
```
┌────────────────────────────────────────────────────────────────┐
│ Dados do Coordenador                              ✏️ Editar   │
├────────────────────────────────────────────────────────────────┤
│ Matrícula: 999999          Nome: Coordenador #999999         │
│ Departamento: Coordenação  Cargo: Coordenador Acadêmico      │
└────────────────────────────────────────────────────────────────┘
```

### Dashboard de Estatísticas
```
┌──────────────┐  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐
│ ⏳ PENDENTES  │  │ ✓ APROVADOS  │  │ ✕ NEGADOS    │  │ 📋 TOTAL     │
│      5       │  │     12       │  │      3       │  │     20       │
│ Ver todos →  │  │              │  │              │  │              │
└──────────────┘  └──────────────┘  └──────────────┘  └──────────────┘
```

### Requerimentos Recentes
```
┌────────────────────────────────────────────────────────────────┐
│ Requerimentos Recentes                         Ver Todos       │
├────────────────────────────────────────────────────────────────┤
│ ID   │ Matéria                    │ Atestado │ Status    │ Data │
├──────┼────────────────────────────┼──────────┼───────────┼──────┤
│ #1   │ Cálculo Diferencial I      │ Médico   │ ⏳ Pend.  │ 24/05│
│ #2   │ Física para Engenharia     │ Médico   │ ✓ Aprov. │ 24/05│
│ #3   │ Geometria Analítica        │ Trabalho │ ✕ Negado │ 23/05│
│ #4   │ Fundamentos Banco Dados    │ Óbito    │ ⏳ Pend.  │ 22/05│
│ #5   │ Redes de Computadores      │ Médico   │ ✓ Aprov. │ 21/05│
└────────────────────────────────────────────────────────────────┘
```

### Ações Rápidas
```
┌────────────────────────────────────────────────────────────────┐
│ Ações Rápidas                                                  │
├────────────────────────────────────────────────────────────────┤
│ [📋 Gerenciar] [➕ Novo] [📊 Relatório] [❓ Ajuda]            │
└────────────────────────────────────────────────────────────────┘
```

---

## 📋 Página de Painel - `/Coordenador/Painel`

### Header
```
┌────────────────────────────────────────────────────────────────┐
│ Painel de Requerimentos de Segunda Chamada        ➕ Novo      │
└────────────────────────────────────────────────────────────────┘
```

### Filtros
```
┌────────────────────────────────────────────────────────────────┐
│ Status: [Todos ▼]  Matéria: [_________]  Data: [__/__] [🔍 Fi]│
└────────────────────────────────────────────────────────────────┘
```

### Tabela Principal
```
┌────────────────────────────────────────────────────────────────┐
│ ID   │ Matrícula│ Matéria      │ Atestado │ Status  │ Data   │A │
├──────┼──────────┼──────────────┼──────────┼─────────┼────────┼──┤
│ #5   │ 201234567│ Cálculo I    │ Médico   │ Pendente│ 24/05  │👁👍❌│
│ #4   │ 201234568│ Física       │ Médico   │ Aprovad│ 24/05  │👁  │
│ #3   │ 201234569│ Geometria    │ Trabalho │ Aprovad│ 24/05  │👁  │
│ #2   │ 201234570│ Banco Dados  │ Óbito    │ Negado │ 22/05  │👁  │
│ #1   │ 201234567│ Redes        │ Médico   │ Pendente│ 21/05  │👁👍❌│
└────────────────────────────────────────────────────────────────┘
```

---

## 🔲 Modals

### Modal: Ver Detalhes
```
┌──────────────────────────────────────────────────────────────────┐
│ Detalhes do Requerimento #5                               [X]   │
├──────────────────────────────────────────────────────────────────┤
│                                                                  │
│ Matrícula do Aluno: 201234567    Matéria: Cálculo I            │
│ Tipo de Atestado: Médico         Status: Pendente              │
│                                                                  │
│ Motivo da Ausência:                                            │
│ Estava com febre alta e precisa de repouso médico              │
│                                                                  │
│ Atestado: [📎 Visualizar Documento]                            │
│ Data de Criação: 24/05/2025 10:30:45                           │
│                                                                  │
├──────────────────────────────────────────────────────────────────┤
│                                                [OK]               │
└──────────────────────────────────────────────────────────────────┘
```

### Modal: Aprovar Requerimento
```
┌──────────────────────────────────────────────────────────────────┐
│ Aprovar Requerimento                                      [X]    │
├──────────────────────────────────────────────────────────────────┤
│                                                                  │
│ Tem certeza que deseja APROVAR este requerimento?              │
│                                                                  │
│ ┌────────────────────────────────────────────────────────────┐ │
│ │ Requerimento #5                                            │ │
│ │ Aluno: 201234567                                           │ │
│ │ Matéria: Cálculo Diferencial e Integral I                │ │
│ └────────────────────────────────────────────────────────────┘ │
│                                                                  │
├──────────────────────────────────────────────────────────────────┤
│                                    [Cancelar] [✓ Aprovar]         │
└──────────────────────────────────────────────────────────────────┘
```

### Modal: Negar Requerimento
```
┌──────────────────────────────────────────────────────────────────┐
│ Negar Requerimento                                        [X]    │
├──────────────────────────────────────────────────────────────────┤
│                                                                  │
│ ┌────────────────────────────────────────────────────────────┐ │
│ │ ⚠️ AVISO: Requerimento #5                                 │ │
│ │ Aluno: 201234567                                           │ │
│ │ Matéria: Cálculo Diferencial e Integral I                │ │
│ └────────────────────────────────────────────────────────────┘ │
│                                                                  │
│ Motivo da Recusa * (obrigatório)                               │
│ ┌──────────────────────────────────────────────────────────┐   │
│ │ Explique o motivo da recusa...                           │   │
│ │                                                            │   │
│ │                                                            │   │
│ └──────────────────────────────────────────────────────────┘   │
│                                                                  │
├──────────────────────────────────────────────────────────────────┤
│                                    [Cancelar] [✕ Negar]          │
└──────────────────────────────────────────────────────────────────┘
```

### Modal: Novo Requerimento
```
┌──────────────────────────────────────────────────────────────────┐
│ Nova Solicitação de Segunda Chamada                       [X]    │
├──────────────────────────────────────────────────────────────────┤
│                                                                  │
│ Matrícula do Aluno *                                           │
│ [_______________________] 👉 Carrega matérias automaticamente   │
│                                                                  │
│ Matéria *                                                       │
│ [Selecione uma matéria ▼]                                       │
│                                                                  │
│ Motivo da Ausência *                                            │
│ ┌──────────────────────────────────────────────────────────┐   │
│ │ Descreva o motivo da ausência...                         │   │
│ │                                                            │   │
│ │                                                            │   │
│ └──────────────────────────────────────────────────────────┘   │
│                                                                  │
│ Tipo de Atestado *                                              │
│ [Selecione... ▼]  (Médico, Trabalho, Óbito, Outro)            │
│                                                                  │
│ URL do Atestado *                                               │
│ [_______________________________]                               │
│                                                                  │
├──────────────────────────────────────────────────────────────────┤
│                        [Cancelar] [➕ Cadastrar]                  │
└──────────────────────────────────────────────────────────────────┘
```

---

## 🎨 Componentes de Design

### Badges de Status
```
Pendente:  ⏳ [badge amarelo]
Aprovado:  ✓  [badge verde]
Negado:    ✕  [badge vermelho]
```

### Cards de Estatísticas
```
┌──────────┐
│ ⏳       │  Cor Amarela (#FFC107)
│ PENDENTES│  Texto Preto
│    5     │  Border-left: 4px
│          │
└──────────┘

┌──────────┐
│ ✓        │  Cor Verde (#28A745)
│ APROVADOS│  Texto Branco
│    12    │  Border-left: 4px
│          │
└──────────┘

┌──────────┐
│ ✕        │  Cor Vermelha (#DC3545)
│ NEGADOS  │  Texto Branco
│    3     │  Border-left: 4px
│          │
└──────────┘

┌──────────┐
│ 📋       │  Cor Azul (#17A2B8)
│ TOTAL    │  Texto Branco
│    20    │  Border-left: 4px
│          │
└──────────┘
```

### Botões de Ação
```
Primária:     [Botão Azul] - Ações principais
Sucesso:      [Botão Verde] - Aprovar
Perigo:       [Botão Vermelho] - Negar
Info:         [Botão Azul Claro] - Relatório
Secundária:   [Botão Cinza] - Outras ações
Outline:      [Botão Contorno] - Links secundários
```

---

## 📱 Responsividade

### Desktop (1200px+)
```
┌────────────────────────────────────────────────────────┐
│ Cards em 4 colunas (lado a lado)                       │
│ Tabela com scroll horizontal se necessário              │
└────────────────────────────────────────────────────────┘
```

### Tablet (768px - 1199px)
```
┌──────────────────────────┐
│ Cards em 2 colunas       │
│ Tabela com scroll        │
└──────────────────────────┘
```

### Mobile (<768px)
```
┌────────┐
│ Cards  │
│ em 1   │
│ coluna │
│        │
│ Tabela │
│ scroll │
└────────┘
```

---

## 🎯 Fluxo de Interação Esperado

### 1️⃣ Login
```
Login Page → [Digite: 999999 / senha123] → Perfil
```

### 2️⃣ Visualizar Perfil
```
Perfil → Dashboard com 4 cards → Requerimentos recentes
```

### 3️⃣ Ir para Painel
```
Perfil → "Ver Todos" → Painel com tabela completa
```

### 4️⃣ Gerenciar Requerimento
```
Painel → [Clique em Linha] → Modal Detalhes
         ↓
      [Ver] → Modal com dados completos
      [Aprovar] → Modal confirmação → Status muda para ✓
      [Negar] → Modal com textarea → Motivo obrigatório → Status muda para ✕
```

### 5️⃣ Logout
```
Navbar → "Sair" → Login Page
```

---

## 🌈 Paleta de Cores

```
Primária:       #1A3A6B (Azul Escuro - Navbar)
Sucesso:        #28A745 (Verde)
Perigo:         #DC3545 (Vermelho)
Aviso:          #FFC107 (Amarelo)
Info:           #17A2B8 (Azul Claro)
Fundo:          #F4F6F9 (Cinza Claro)
Texto Principal:#1C1C1E (Preto)
Texto Secundário:#6B7280 (Cinza)
Border:         #D1D5DB (Cinza Claro)
```

---

## 📐 Espaçamento

```
Padding Containers: 1rem
Padding Cards:      1.5rem
Margin Bottom:      1rem
Gap Cards:          1rem (g-3)
Borda Arredondada:  0.75rem
Shadow:             0 4px 16px rgba(0,0,0,0.08)
```

---

## ✨ Efeitos

- Hover nos cards: Sombra mais forte
- Hover nos botões: Cor mais clara
- Modals: Fade in (0.3s)
- Badges: Sem efeito hover
- Tabelas: Hover na linha

---

**Versão:** 1.0  
**Status:** ✅ Design Finalizado  
**Compatibilidade:** Todos os navegadores modernos
