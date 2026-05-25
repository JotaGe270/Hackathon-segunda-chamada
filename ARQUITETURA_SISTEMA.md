# 🏗️ Arquitetura do Sistema de Perfil de Coordenador

## 📐 Fluxo de Dados

```
┌─────────────────────────────────────────────────────────────┐
│                        USUÁRIO NAVEGADOR                     │
└────────────────────────────┬────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────┐
│                   VIEWS (Razor Pages)                        │
├─────────────────────────────────────────────────────────────┤
│ • Perfil.cshtml         - Dashboard do perfil               │
│ • Painel.cshtml         - Gerenciamento de requerimentos    │
│ • _NavbarCoordenador.cshtml - Componente de navegação       │
└────────────────────────────┬────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────┐
│              CONTROLLER (CoordenadorController)              │
├─────────────────────────────────────────────────────────────┤
│ • Perfil()   - Busca dados e monta DTO                      │
│ • Painel()   - Lista requerimentos                          │
│ • Aprovar()  - Atualiza status para Aprovado               │
│ • Negar()    - Atualiza status para Negado + motivo        │
└────────────────────────────┬────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────┐
│                    SERVICES (DTOs)                           │
├─────────────────────────────────────────────────────────────┤
│ • PerfilCoordenadorDto - Encapsula dados do perfil         │
│ • RequerimentoResumoDto - Resumo de requerimentos          │
│ • RequerimentoDetalheDto - Detalhes completos              │
└────────────────────────────┬────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────┐
│                  DATA LAYER (Entity Framework)               │
├─────────────────────────────────────────────────────────────┤
│ • AppDbContext.Usuarios - Dados dos coordenadores          │
│ • AppDbContext.RequerimentosSegundaChamada - Requerimentos │
└────────────────────────────┬────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────┐
│                    SQL SERVER (BD)                           │
├─────────────────────────────────────────────────────────────┤
│ • Tabela: Usuarios                                          │
│ • Tabela: RequerimentosSegundaChamada                       │
└─────────────────────────────────────────────────────────────┘
```

---

## 📁 Estrutura de Arquivos Criada

```
Hackathon-segunda-chamada/
│
├── DTOs/
│   └── PerfilCoordenadorDto.cs ✨ [NOVO]
│
├── Controllers/
│   └── CoordenadorController.cs (modificado para adicionar action Perfil)
│
├── Views/
│   ├── Coordenador/
│   │   ├── Perfil.cshtml ✨ [NOVO]
│   │   └── Painel.cshtml ✨ [NOVO]
│   │
│   └── Shared/
│       └── _NavbarCoordenador.cshtml ✨ [NOVO]
│
└── IMPLEMENTACAO_PERFIL_COORDENADOR.md ✨ [DOCUMENTAÇÃO]
```

---

## 🔄 Fluxo de Interação - Caso de Uso: Visualizar Perfil

```
1. Usuário acessa /Coordenador/Perfil
   │
   ├─→ CoordenadorController.Perfil() 
   │   ├─→ Valida autenticação (Claims)
   │   ├─→ Busca Usuario no BD
   │   ├─→ Calcula Total Pendentes (COUNT WHERE Status="Pendente")
   │   ├─→ Calcula Total Aprovados (COUNT WHERE Status="Aprovado")
   │   ├─→ Calcula Total Negados (COUNT WHERE Status="Negado")
   │   ├─→ Busca 5 Requerimentos Recentes (ORDER BY DESC, TAKE(5))
   │   └─→ Monta PerfilCoordenadorDto
   │
   └─→ Retorna View("Perfil", perfilDto)
       │
       ├─→ Perfil.cshtml renderiza:
       │   ├─→ Dados do Coordenador
       │   ├─→ Cards com Estatísticas
       │   ├─→ Tabela de Requerimentos Recentes
       │   └─→ Ações Rápidas
       │
       └─→ Browser exibe página HTML
```

---

## 🔄 Fluxo de Interação - Caso de Uso: Aprovar Requerimento

```
1. Usuário clica no botão "Aprovar" do Painel
   │
   ├─→ Modal de Confirmação abre
   │
   ├─→ Usuário confirma aprovação
   │   │
   │   ├─→ Formulário POST é enviado
   │   │   ├─→ Dados: id, AntiForgeryToken
   │   │   └─→ Para: CoordenadorController.Aprovar(id)
   │   │
   │   ├─→ CoordenadorController.Aprovar(id)
   │   │   ├─→ Busca Requerimento por ID
   │   │   ├─→ Atualiza: Status = "Aprovado"
   │   │   ├─→ Limpa: MotivoRecusa = null
   │   │   ├─→ SaveChanges()
   │   │   ├─→ Define TempData["MensagemSucesso"]
   │   │   └─→ RedirectToAction("Painel")
   │   │
   │   └─→ Retorna Painel atualizado
   │       └─→ Badge do requerimento muda para "✓ Aprovado" (verde)
   │
   └─→ Mensagem de sucesso exibida ao usuário
```

---

## 🔄 Fluxo de Interação - Caso de Uso: Negar Requerimento

```
1. Usuário clica no botão "Negar" do Painel
   │
   ├─→ Modal de Negação abre (com textarea obrigatório)
   │
   ├─→ Usuário digita motivo da recusa
   │
   ├─→ Usuário clica em "Negar"
   │   │
   │   ├─→ Validação Frontend (textarea obrigatório)
   │   │
   │   ├─→ Formulário POST é enviado
   │   │   ├─→ Dados: id, motivo, AntiForgeryToken
   │   │   └─→ Para: CoordenadorController.Negar(id, motivo)
   │   │
   │   ├─→ CoordenadorController.Negar(id, motivo)
   │   │   ├─→ Valida: if (string.IsNullOrWhiteSpace(motivo))
   │   │   │       ├─→ SetTempData["MensagemErro"]
   │   │   │       └─→ RedirectToAction("Painel")
   │   │   │
   │   │   ├─→ Busca Requerimento por ID
   │   │   ├─→ Atualiza: Status = "Negado"
   │   │   ├─→ Grava: MotivoRecusa = motivo
   │   │   ├─→ SaveChanges()
   │   │   ├─→ Define TempData["MensagemSucesso"]
   │   │   └─→ RedirectToAction("Painel")
   │   │
   │   └─→ Retorna Painel atualizado
   │       ├─→ Badge muda para "✕ Negado" (vermelho)
   │       ├─→ Botões de ação desaparecem
   │       └─→ Modal de Detalhes mostra "Motivo da Recusa"
   │
   └─→ Mensagem de sucesso exibida ao usuário
```

---

## 📊 Modelos de Dados

### PerfilCoordenadorDto
```csharp
public record PerfilCoordenadorDto
{
    public int Matricula                         // 123456
    public string NomeCompleto                   // "Coordenador #123456"
    public string Departamento                   // "Coordenação Acadêmica"
    public int TotalRequerimentosPendentes       // 5
    public int TotalRequerimentosAprovados       // 12
    public int TotalRequerimentosNegados         // 3
    public List<RequerimentoResumoDto> RequerimentosRecentes
    // [
    //   {Id: 1, NomeMateria: "Cálculo I", TipoAtestado: "Médico", 
    //    Status: "Pendente", DataCriacao: "24/05/2025 10:30"}
    // ]
}
```

### RequerimentoSegundaChamada (modelo existente)
```csharp
public class RequerimentoSegundaChamada
{
    public int Id                          // 1
    public int MatriculaAluno              // 201234567
    public string NomeMateria              // "Cálculo Diferencial I"
    public string Motivo                   // "Estava doente"
    public string TipoAtestado             // "Médico"
    public string URLAtestado              // "https://drive.google.com/..."
    public string Status                   // "Pendente" | "Aprovado" | "Negado"
    public string? MotivoRecusa            // "Atestado ilegível"
    public DateTime DataCriacao            // 24/05/2025 10:30
}
```

---

## 🎯 Endpoints Disponíveis

| HTTP | Path | Controller | Action | Descrição |
|------|------|-----------|--------|-----------|
| GET | `/Coordenador/Perfil` | CoordenadorController | Perfil | Exibe perfil do coordenador |
| GET | `/Coordenador/Painel` | CoordenadorController | Painel | Lista todos os requerimentos |
| POST | `/Coordenador/Aprovar` | CoordenadorController | Aprovar | Aprova um requerimento |
| POST | `/Coordenador/Negar` | CoordenadorController | Negar | Nega um requerimento |
| GET | `/RequerimentoSegundaChamada/BuscarMateriasPorMatricula?matricula=123` | RequerimentoSegundaChamadaController | BuscarMateriasPorMatricula | Retorna matérias em JSON |
| POST | `/RequerimentoSegundaChamada/Novo` | RequerimentoSegundaChamadaController | Novo | Cria novo requerimento |

---

## 🔐 Autenticação e Autorização

```csharp
[Authorize(Roles = "Coordenador")]  // Apenas Coordenadores podem acessar
public class CoordenadorController : Controller
{
    public async Task<IActionResult> Perfil()
    {
        var matriculaLogada = User.Claims.FirstOrDefault(
            c => c.Type == ClaimTypes.Name)?.Value;
        
        // Valida se usuário está autenticado
        if (matriculaLogada == null || !int.TryParse(matriculaLogada, out var matricula))
            return RedirectToAction("Login", "Account");
    }
}
```

---

## 🎨 Componentes UI/UX

### Cards de Estatísticas
- **Pendentes** (Amarelo) - ⏳ Requerimentos aguardando análise
- **Aprovados** (Verde) - ✓ Requerimentos aprovados
- **Negados** (Vermelho) - ✕ Requerimentos rejeitados
- **Total** (Azul) - 📋 Soma total

### Badges de Status
- `<span class="badge bg-warning">⏳ Pendente</span>`
- `<span class="badge bg-success">✓ Aprovado</span>`
- `<span class="badge bg-danger">✕ Negado</span>`

### Modals Reutilizáveis
Cada requerimento gera dinamicamente 3 modals:
1. `#modal-detalhes-{id}` - Visualizar completo
2. `#modal-aprovar-{id}` - Confirmação de aprovação
3. `#modal-negar-{id}` - Formulário de negação

---

## 📈 Queries ao Banco de Dados

```csharp
// Query 1: Buscar coordenador
await _context.Usuarios
    .FirstOrDefaultAsync(u => u.Matricula == matricula)

// Query 2: Contar requerimentos pendentes
_context.RequerimentosSegundaChamada
    .CountAsync(r => r.Status == "Pendente")

// Query 3: Contar requerimentos aprovados
_context.RequerimentosSegundaChamada
    .CountAsync(r => r.Status == "Aprovado")

// Query 4: Buscar últimos 5 requerimentos
_context.RequerimentosSegundaChamada
    .OrderByDescending(r => r.DataCriacao)
    .Take(5)
    .Select(r => new RequerimentoResumoDto(...))
    .ToListAsync()

// Query 5: Buscar todos os requerimentos (ordenados)
_context.RequerimentosSegundaChamada
    .OrderByDescending(r => r.DataCriacao)
    .ToListAsync()

// Query 6: Atualizar status de requerimento
_context.RequerimentosSegundaChamada.FindAsync(id)
_context.SaveChangesAsync()
```

---

## ✅ Validações Implementadas

| Validação | Local | Tipo |
|-----------|-------|------|
| Autenticação | Controller | Backend |
| Role "Coordenador" | Controller | Backend |
| Matrícula válida | Controller | Backend |
| Motivo obrigatório | Controller + View | Backend + Frontend |
| Anti-CSRF Token | Form | Backend |
| URL válida | HTML5 | Frontend |
| Textarea obrigatório | Modal Negar | Frontend |

---

## 🚀 Performance

- ✅ Queries otimizadas com `ToListAsync()` e `FirstOrDefaultAsync()`
- ✅ Apenas 5 requerimentos recentes carregados no perfil
- ✅ Contagem com `.CountAsync()` (eficiente)
- ✅ Paginação possível no painel (extensível)

---

## 📝 Convenções Seguidas

- ✅ **Action Results**: `RedirectToAction()` para POST
- ✅ **TempData**: Para mensagens transitórias de sucesso/erro
- ✅ **Async/Await**: Todas as operações assíncronas
- ✅ **DTOs**: Transferência de dados entre camadas
- ✅ **Authorization**: Filtros de autorização por role
- ✅ **Validation**: Validação no backend (segurança)

---

**Versão:** 1.0
**Status:** ✅ Produção
**Última atualização:** 24/05/2025
