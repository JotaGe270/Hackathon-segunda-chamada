# 📋 Sistema de Perfil de Coordenador - Resumo de Implementação

## 🎯 Objetivo
Criar uma tela completa de perfil de coordenação que integre todas as funcionalidades existentes no código e forneça uma interface intuitiva para gerenciamento de requerimentos de segunda chamada.

---

## 📦 Componentes Implementados

### 1. **DTO - PerfilCoordenadorDto.cs**
Novo modelo de dados para transportar informações do perfil do coordenador:
```csharp
public record PerfilCoordenadorDto(
    int Matricula,
    string NomeCompleto,
    string Departamento,
    int TotalRequerimentosPendentes,
    int TotalRequerimentosAprovados,
    int TotalRequerimentosNegados,
    List<RequerimentoResumoDto> RequerimentosRecentes
);
```

**Responsabilidades:**
- Encapsula dados do coordenador
- Fornece estatísticas de requerimentos
- Lista requerimentos recentes

---

### 2. **View - _NavbarCoordenador.cshtml**
Componente de navegação específico para coordenadores

**Funcionalidades:**
- Link para Painel de requerimentos
- Link para Meu Perfil
- Exibição da matrícula do usuário logado
- Botão de Logout

---

### 3. **View - Perfil.cshtml**
Página principal do perfil do coordenador com:

#### 📊 Seção de Dados Pessoais
- Matrícula
- Nome completo
- Departamento
- Cargo
- Botão para editar perfil

#### 📈 Dashboard com Estatísticas
Cards interativos mostrando:
- **⏳ Requerimentos Pendentes** - Total de requisições aguardando análise
- **✓ Requerimentos Aprovados** - Total de requisições aprovadas
- **✕ Requerimentos Negados** - Total de requisições negadas
- **📋 Total Geral** - Soma de todos os requerimentos

#### 📝 Requerimentos Recentes
Tabela mostrando os 5 últimos requerimentos com:
- ID do requerimento
- Nome da matéria
- Tipo de atestado
- Status com badge colorida
- Data de criação
- Link para ver todos os requerimentos

#### ⚡ Ações Rápidas
Botões para acesso direto a:
- Gerenciar Requerimentos (vai para Painel)
- Novo Requerimento (criar manualmente)
- Relatório (modal para gerar relatórios)
- Ajuda

#### 📑 Modal de Relatório
Permite gerar relatórios customizados com filtros por:
- Data inicial e final
- Status do requerimento

---

### 4. **View - Painel.cshtml**
Painel completo de gerenciamento de requerimentos

#### 🔍 Filtros Avançados
- Status (Todos, Pendente, Aprovado, Negado)
- Busca por matéria
- Filtro por data

#### 📊 Tabela Principal com Colunas
- ID do requerimento
- Matrícula do aluno
- Nome da matéria
- Tipo de atestado
- Status (com badges coloridas)
- Data de criação
- Ações (Ver detalhes, Aprovar, Negar)

#### 👁️ Modal de Detalhes
Visualiza informações completas do requerimento:
- Matrícula do aluno
- Matéria
- Tipo de atestado
- Status
- Motivo da ausência
- Motivo da recusa (se houver)
- Link para documento de atestado
- Data de criação

#### ✅ Modal de Aprovação
Confirmação antes de aprovar requerimento com:
- Resumo do requerimento
- Botão de confirmação

#### ❌ Modal de Negação
Rejeição de requerimento com campo obrigatório para:
- Motivo da recusa
- Validação de campo obrigatório

#### ➕ Modal de Novo Requerimento
Permite ao coordenador criar requerimentos manualmente:
- Campo de matrícula do aluno (carrega matérias automaticamente via AJAX)
- Seleção de matéria
- Motivo da ausência
- Tipo de atestado
- URL do documento

---

### 5. **Controller - CoordenadorController.cs**
Nova action no controller:

```csharp
public async Task<IActionResult> Perfil()
```

**Funcionalidades:**
- Valida se o usuário está autenticado
- Busca dados do coordenador no banco
- Calcula estatísticas (total pendentes, aprovados, negados)
- Busca os 5 requerimentos mais recentes
- Monta o DTO com todas as informações
- Retorna a view Perfil.cshtml

---

## 🔌 Integração com Código Existente

### Funcionalidades Utilizadas:

1. **Autenticação**
   - Utiliza sistema de claims (ClaimTypes)
   - Restrição por role [Authorize(Roles = "Coordenador")]

2. **Requerimentos**
   - Lista de requerimentos com `RequerimentosSegundaChamada`
   - Status: Pendente, Aprovado, Negado
   - Motivos de recusa

3. **Ações Existentes**
   - `Aprovar(int id)` - marca como aprovado
   - `Negar(int id, string motivo)` - marca como negado com justificativa
   - `Painel()` - lista todos os requerimentos
   - `BuscarMateriasPorMatricula(int matricula)` - retorna matérias em JSON

---

## 🎨 Design e UX

- **Navbar customizada** para coordenadores
- **Cards com bordas coloridas** para status
- **Badges dinâmicas** com emojis e cores
- **Modals reutilizáveis** para ações
- **Layout responsivo** (mobile-friendly)
- **Feedback visual** com mensagens de sucesso/erro
- **Ícones informativos** para melhor compreensão

---

## 🔐 Segurança

- ✅ Autorização por role (Coordenador)
- ✅ Anti-CSRF tokens em formulários
- ✅ Validação de matrícula logada
- ✅ Validação de campo obrigatório (motivo recusa)
- ✅ Redirecionar para login se não autenticado

---

## 📱 Rotas de Acesso

| Rota | Ação | Descrição |
|------|------|-----------|
| `/Coordenador/Perfil` | GET | Exibe perfil do coordenador |
| `/Coordenador/Painel` | GET | Exibe painel com todos os requerimentos |
| `/Coordenador/Aprovar` | POST | Aprova um requerimento |
| `/Coordenador/Negar` | POST | Nega um requerimento com motivo |

---

## 🚀 Como Usar

### 1. Login como Coordenador
- Use matrícula e senha cadastradas como perfil "Coordenador"

### 2. Navegar para o Perfil
- Clique em "Meu Perfil" na navbar

### 3. Visualizar Estatísticas
- O dashboard mostra resumo de requerimentos

### 4. Acessar Painel
- Clique em "Painel" ou em "Ver todos" em qualquer card

### 5. Gerenciar Requerimentos
- Use os botões de ação (Ver, Aprovar, Negar)
- Preencha o motivo ao negar

### 6. Gerar Relatório
- Clique no botão "Relatório" para customizar filtros

---

## ✨ Funcionalidades Extras

- **JavaScript para carregamento dinâmico** de matérias
- **Modal reutilizável** para cada requerimento
- **Validação no frontend** e backend
- **Feedback em tempo real** com TempData
- **Filtros para busca avançada**

---

## 📋 Modelos Usados

- `Usuario` - dados do coordenador
- `RequerimentoSegundaChamada` - requerimentos
- `PerfilUsuario` enum (AlunoEng, AlunoSI, Coordenador, Professor)

---

## 🔗 Dependências

- Entity Framework Core (para consultas ao BD)
- ASP.NET Core Identity (para autenticação)
- Bootstrap 5 (para estilo)

---

**Status:** ✅ Implementação Concluída e Testada
**Compatibilidade:** .NET 10 | Razor Pages
