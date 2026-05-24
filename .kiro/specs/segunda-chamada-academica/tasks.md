# Plano de Implementação: Sistema de Segunda Chamada Acadêmica

## Visão Geral

Implementação incremental do sistema de segunda chamada acadêmica sobre ASP.NET Core MVC (.NET 10), Entity Framework Core, SQL Server, Bootstrap 5 e jQuery. As tarefas estão ordenadas por dependência: infraestrutura → modelos/DTOs → serviços → controllers → views → JavaScript → testes.

## Tarefas

- [x] 1. Configurar infraestrutura base (Session, DI, pasta de uploads)
  - Adicionar `builder.Services.AddSession(...)` e `builder.Services.AddDistributedMemoryCache()` em `Program.cs`
  - Adicionar `app.UseSession()` no pipeline HTTP de `Program.cs`, antes de `app.UseAuthorization()`
  - Criar o diretório `wwwroot/uploads/` e adicionar entrada `wwwroot/uploads/` no `.gitignore`
  - Configurar limite de tamanho de requisição para 10 MB via `builder.WebHost.ConfigureKestrel` ou `[RequestSizeLimit]`
  - _Requisitos: 4.1, 5.1_

- [x] 2. Adicionar campo `Motivo` ao modelo e criar migration
  - [x] 2.1 Adicionar propriedade `public string Motivo { get; set; }` com `[Required]` ao modelo `Models/RequerimentoSegundaChamada.cs`
    - _Requisitos: 3.3, 6.4_
  - [x] 2.2 Criar nova migration via `dotnet ef migrations add AdicionarMotivoRequerimento` e aplicar com `dotnet ef database update`
    - _Requisitos: 3.3_

- [x] 3. Criar ViewModels e DTOs
  - [x] 3.1 Criar `Models/ViewModels/LoginViewModel.cs` com propriedades `Matricula` (int, Required) e `Senha` (string, Required)
    - _Requisitos: 1.1_
  - [x] 3.2 Criar `Models/ViewModels/CriarRequerimentoViewModel.cs` com propriedades `NomeMateria`, `Motivo`, `TipoAtestado` (strings, Required) e `Arquivo` (IFormFile, Required)
    - _Requisitos: 3.3, 4.1_
  - [x] 3.3 Criar `DTOs/UsuarioAutenticadoDto.cs` como `record` com campos `Id`, `Matricula`, `Perfil`
    - _Requisitos: 1.1, 1.2_
  - [x] 3.4 Criar `DTOs/DadosAlunoDto.cs` como `record` com campos `Matricula`, `NomeCompleto`, `Curso`, `Periodo`, `Turno`
    - _Requisitos: 2.1_
  - [x] 3.5 Criar `DTOs/MateriaDto.cs` como `record` com campos `Codigo` e `Nome`
    - _Requisitos: 3.1, 3.2_
  - [x] 3.6 Criar `DTOs/RequerimentoResumoDto.cs` como `record` com campos `Id`, `NomeMateria`, `TipoAtestado`, `Status`, `DataCriacao`
    - _Requisitos: 2.2, 2.3_
  - [x] 3.7 Criar `DTOs/RequerimentoDetalheDto.cs` como `record` com campos `Id`, `MatriculaAluno`, `NomeMateria`, `Motivo`, `TipoAtestado`, `URLAtestado`, `Status`, `DataCriacao`
    - _Requisitos: 6.1, 6.4_
  - [x] 3.8 Criar `DTOs/CriarRequerimentoResultDto.cs` como `record` com campos `Sucesso` (bool), `Id` (int?), `Mensagem` (string)
    - _Requisitos: 3.4, 3.5_

- [x] 4. Implementar filtro de autorização `AutorizarPerfilAttribute`
  - [x] 4.1 Criar `Filters/AutorizarPerfilAttribute.cs` herdando de `ActionFilterAttribute`
    - Construtor recebe `params PerfilUsuario[] perfis`
    - Sobrescrever `OnActionExecuting`: ler `HttpContext.Session.GetString("Perfil")`, verificar se o perfil está na lista de permitidos
    - Se sessão inválida ou perfil não autorizado: `context.Result = new RedirectToActionResult("Login", "Auth", null)`
    - _Requisitos: 5.1, 5.2, 5.3, 5.4, 5.5_
  - [ ]* 4.2 Escrever testes unitários para `AutorizarPerfilAttribute`
    - Testar redirecionamento sem sessão (Propriedade 8)
    - Testar redirecionamento com perfil não autorizado (Propriedade 8)
    - Testar passagem com perfil autorizado
    - **Propriedade 8: Acesso sem sessão válida sempre redireciona para login**
    - **Valida: Requisitos 5.1, 5.2, 5.5**

- [x] 5. Implementar `AuthService`
  - [x] 5.1 Criar interface `Services/IAuthService.cs` com método `Task<UsuarioAutenticadoDto?> ValidarCredenciais(int matricula, string senha)`
    - _Requisitos: 1.1, 1.6_
  - [x] 5.2 Criar implementação `Services/AuthService.cs`
    - Injetar `AppDbContext` via construtor
    - `ValidarCredenciais`: rejeitar `matricula <= 0` retornando `null` sem consultar o banco
    - Buscar usuário com `FirstOrDefaultAsync(u => u.Matricula == matricula)`
    - Comparar senha (texto claro para dev; preparar para hash em produção)
    - Retornar `UsuarioAutenticadoDto` se válido, `null` caso contrário
    - _Requisitos: 1.1, 1.4, 1.6_
  - [x] 5.3 Registrar `IAuthService` → `AuthService` como `Scoped` em `Program.cs`
    - _Requisitos: 1.1_
  - [ ]* 5.4 Escrever testes de propriedade para `AuthService`
    - **Propriedade 1: Autenticação redireciona para o dashboard correto por perfil**
    - **Valida: Requisitos 1.1, 1.2, 1.3**
    - **Propriedade 2: Credenciais inválidas nunca resultam em autenticação**
    - **Valida: Requisitos 1.4, 1.6**

- [x] 6. Implementar `ArquivoService`
  - [x] 6.1 Criar interface `Services/IArquivoService.cs` com método `Task<string> SalvarArquivo(IFormFile arquivo)`
    - _Requisitos: 4.1, 4.2, 4.3, 4.4, 4.6_
  - [x] 6.2 Criar implementação `Services/ArquivoService.cs`
    - Injetar `IWebHostEnvironment` via construtor para obter `WebRootPath`
    - Definir extensões permitidas: `{".pdf", ".jpg", ".jpeg", ".png"}`
    - Definir MIME types permitidos: `{"application/pdf", "image/jpeg", "image/png"}`
    - Validar extensão e MIME type; lançar exceção ou retornar erro se inválido
    - Validar tamanho ≤ 10 MB; rejeitar se exceder
    - Gerar nome com `Guid.NewGuid() + extensão` — nunca usar nome original
    - Salvar em `wwwroot/uploads/{guid}{extensao}` via `FileStream`
    - Retornar URL relativa `/uploads/{guid}{extensao}`
    - _Requisitos: 4.1, 4.2, 4.3, 4.4, 4.6, 8.4_
  - [x] 6.3 Registrar `IArquivoService` → `ArquivoService` como `Scoped` em `Program.cs`
    - _Requisitos: 4.1_
  - [ ]* 6.4 Escrever testes de propriedade para `ArquivoService`
    - **Propriedade 6: Upload com extensão inválida sempre é rejeitado**
    - **Valida: Requisitos 4.3, 4.6**
    - **Propriedade 7: Nome do arquivo salvo nunca contém o nome original**
    - **Valida: Requisitos 4.2, 8.4**

- [x] 7. Implementar `RequerimentoService`
  - [x] 7.1 Criar interface `Services/IRequerimentoService.cs` com métodos:
    - `Task<RequerimentoDto> CriarRequerimento(CriarRequerimentoDto dto)`
    - `Task<RequerimentoDetalheDto?> ObterPorId(int id)`
    - _Requisitos: 3.3, 3.6, 6.1, 6.2, 6.3_
  - [x] 7.2 Criar `DTOs/CriarRequerimentoDto.cs` como `record` com campos `MatriculaAluno`, `NomeMateria`, `Motivo`, `TipoAtestado`, `URLAtestado`
    - _Requisitos: 3.3_
  - [x] 7.3 Criar implementação `Services/RequerimentoService.cs`
    - Injetar `AppDbContext` via construtor
    - `CriarRequerimento`: validar `TipoAtestado ∈ {"medico","trabalho","obito"}`; lançar `ArgumentException` se inválido
    - Criar entidade com `Status = "Pendente"` e `DataCriacao = DateTime.UtcNow`
    - Persistir via `DbContext.Add` + `SaveChangesAsync`
    - `ObterPorId`: rejeitar `id <= 0` sem consultar banco; retornar `null` se não encontrado
    - Mapear entidade para `RequerimentoDetalheDto`
    - _Requisitos: 3.3, 3.4, 3.6, 6.1, 6.2, 6.3, 6.4_
  - [x] 7.4 Registrar `IRequerimentoService` → `RequerimentoService` como `Scoped` em `Program.cs`
    - _Requisitos: 3.3_
  - [ ]* 7.5 Escrever testes de propriedade para `RequerimentoService`
    - **Propriedade 3: Requerimento criado sempre inicia com status Pendente**
    - **Valida: Requisitos 3.3, 3.4**
    - **Propriedade 4: Round-trip de requerimento preserva todos os dados**
    - **Valida: Requisitos 3.3, 6.1, 6.4**
    - **Propriedade 5: Tipo de atestado inválido sempre é rejeitado**
    - **Valida: Requisito 3.6**

- [x] 8. Implementar `AlunoService`
  - [x] 8.1 Criar interface `Services/IAlunoService.cs` com métodos:
    - `Task<DadosAlunoDto> ObterDadosAluno(int matricula)`
    - `Task<List<MateriaDto>> ObterMaterias(int matricula)`
    - `Task<List<RequerimentoResumoDto>> ObterRequerimentos(int matricula)`
    - _Requisitos: 2.1, 2.2, 2.3, 3.1_
  - [x] 8.2 Criar implementação `Services/AlunoService.cs`
    - Injetar `AppDbContext` via construtor
    - `ObterDadosAluno`: buscar usuário por matrícula; derivar `Curso` do enum (`AlunoEng` → "Engenharia", `AlunoSI` → "Sistemas de Informação"); retornar `DadosAlunoDto` com valores placeholder para Período e Turno
    - `ObterMaterias`: retornar lista estática de matérias por curso (hardcoded até tabela de matérias existir)
    - `ObterRequerimentos`: buscar requerimentos do aluno ordenados por `DataCriacao DESC` via LINQ; mapear para `RequerimentoResumoDto`
    - _Requisitos: 2.1, 2.2, 2.3, 3.1, 3.2_
  - [x] 8.3 Registrar `IAlunoService` → `AlunoService` como `Scoped` em `Program.cs`
    - _Requisitos: 2.1_
  - [ ]* 8.4 Escrever testes de propriedade para `AlunoService`
    - **Propriedade 10: Histórico de requerimentos sempre ordenado por data decrescente**
    - **Valida: Requisito 2.3**

- [ ] 9. Checkpoint — Compilar e verificar registro de serviços
  - Garantir que o projeto compila sem erros (`dotnet build`)
  - Verificar que todos os serviços estão registrados em `Program.cs`
  - Perguntar ao usuário se há dúvidas antes de prosseguir para os controllers

- [ ] 10. Implementar `AuthController`
  - [ ] 10.1 Criar `Controllers/AuthController.cs` com rota `[Route("auth")]`
    - Injetar `IAuthService` via construtor
    - `GET /auth/login` → retornar `View()`
    - `POST /auth/login` com `[ValidateAntiForgeryToken]`: chamar `ValidarCredenciais`; se válido, gravar `"UsuarioId"` e `"Perfil"` na Session e redirecionar por perfil; se inválido, adicionar `ModelError` e retornar `View(model)`
    - `POST /auth/logout`: chamar `HttpContext.Session.Clear()` e redirecionar para `/auth/login`
    - _Requisitos: 1.1, 1.2, 1.3, 1.4, 1.5_
  - [ ] 10.2 Atualizar `HomeController.cs` para redirecionar `Index` para `/auth/login`
    - _Requisitos: 1.1_

- [ ] 11. Implementar `AlunoController`
  - Criar `Controllers/AlunoController.cs` com rota `[Route("aluno")]` e `[AutorizarPerfil(PerfilUsuario.AlunoEng, PerfilUsuario.AlunoSI)]`
  - Injetar `IAlunoService` via construtor
  - `GET /aluno/dashboard`: ler matrícula da Session; chamar `ObterDadosAluno`; passar `DadosAlunoDto` para a View
  - `GET /aluno/materias`: ler matrícula da Session; chamar `ObterMaterias`; retornar `Json(materias)`
  - `GET /aluno/requerimentos`: ler matrícula da Session; chamar `ObterRequerimentos`; retornar `Json(requerimentos)`
  - _Requisitos: 2.1, 2.2, 2.3, 3.1, 3.2, 5.3_

- [ ] 12. Implementar `RequerimentoController`
  - Criar `Controllers/RequerimentoController.cs` com rota `[Route("requerimento")]` e `[AutorizarPerfil(PerfilUsuario.AlunoEng, PerfilUsuario.AlunoSI)]`
  - Injetar `IRequerimentoService` e `IArquivoService` via construtor
  - `POST /requerimento/criar` com `[ValidateAntiForgeryToken]`:
    - Verificar `ModelState.IsValid`; retornar HTTP 400 se inválido
    - Ler matrícula da Session; retornar HTTP 401 se sessão expirada
    - Chamar `ArquivoService.SalvarArquivo`; tratar exceção de arquivo inválido com HTTP 400
    - Montar `CriarRequerimentoDto` e chamar `RequerimentoService.CriarRequerimento`
    - Retornar `Json(new CriarRequerimentoResultDto(true, id, "Requerimento criado com sucesso."))`
  - `GET /requerimento/{id:int}`: chamar `ObterPorId`; retornar HTTP 404 se null; retornar `Json(detalhe)` se encontrado
  - _Requisitos: 3.3, 3.4, 3.5, 3.10, 4.1, 5.4, 6.1, 6.2, 6.3, 8.1, 8.2_

- [ ] 13. Implementar `ArquivoController`
  - Criar `Controllers/ArquivoController.cs` com rota `[Route("arquivo")]`
  - Injetar `IArquivoService` via construtor
  - `POST /arquivo/upload`: chamar `SalvarArquivo`; retornar `Json(new { url, nomeArquivo })` em sucesso; retornar HTTP 400 com mensagem em caso de exceção
  - _Requisitos: 4.1, 4.3, 4.4_

- [ ] 14. Checkpoint — Testar endpoints via compilação e rotas
  - Garantir que o projeto compila sem erros (`dotnet build`)
  - Verificar que todas as rotas estão corretamente mapeadas
  - Perguntar ao usuário se há dúvidas antes de prosseguir para as Views

- [ ] 15. Criar CSS customizado com paleta institucional
  - Editar `wwwroot/css/site.css` adicionando variáveis CSS:
    - `--color-primary: #1A3A6B`, `--color-primary-light: #2E5FA3`, `--color-accent: #4A90D9`
    - `--color-success: #2E7D32`, `--color-danger: #C62828`, `--color-warning: #F57F17`
    - `--color-bg: #F4F6F9`, `--color-surface: #FFFFFF`
    - `--color-text-primary: #1C1C1E`, `--color-text-secondary: #6B7280`, `--color-border: #D1D5DB`
  - Adicionar import da fonte Inter (Google Fonts) no `_Layout.cshtml`
  - Estilizar `.btn-primary` com `--color-primary`, `.badge-pendente` com `--color-warning`, etc.
  - _Requisitos: 2.5, 2.6, 2.7_

- [ ] 16. Atualizar `_Layout.cshtml` base
  - Adicionar link para Google Fonts (Inter) no `<head>`
  - Adicionar referência ao `site.css` atualizado
  - Configurar `<body>` com `background-color: var(--color-bg)`
  - Remover conteúdo de navegação padrão do template (será substituído por partials por perfil)
  - _Requisitos: 2.8_

- [ ] 17. Criar partial `_NavbarAluno.cshtml` e `_StatusBadge.cshtml`
  - [ ] 17.1 Criar `Views/Shared/_NavbarAluno.cshtml`
    - Navbar Bootstrap com logo institucional, nome do aluno (via `ViewBag` ou `ViewData`) e botão de logout (form POST para `/auth/logout` com token CSRF)
    - Usar `--color-primary` como cor de fundo da navbar
    - _Requisitos: 2.8, 8.1_
  - [x] 17.2 Criar `Views/Shared/_StatusBadge.cshtml`
    - Partial que recebe `string status` via `@model` ou `ViewData`
    - Renderiza `<span class="badge bg-{classe}">` onde classe é derivada do status
    - _Requisitos: 2.5, 2.6, 2.7_

- [x] 18. Criar View de Login (`Views/Auth/Login.cshtml`)
  - Criar pasta `Views/Auth/` e arquivo `Login.cshtml`
  - Layout: página centralizada com card Bootstrap, fundo `--color-bg`
  - Card contém: logo institucional (placeholder `<img>`), título "Segunda Chamada Acadêmica", campo Matrícula (`<input type="number">`), campo Senha (`<input type="password">`), botão "Entrar" (full-width, `--color-primary`), div de alerta para erros de ModelState
  - Usar `asp-for`, `asp-validation-for` e `asp-antiforgery="true"` no formulário
  - _Requisitos: 1.1, 1.4, 8.1_

- [x] 19. Criar View do Dashboard do Aluno (`Views/Aluno/Dashboard.cshtml`)
  - Criar pasta `Views/Aluno/` e arquivo `Dashboard.cshtml`
  - Incluir `@Html.Partial("_NavbarAluno")`
  - Card de dados do aluno: exibir Matrícula, Curso, Período, Turno do `DadosAlunoDto` passado pelo controller
  - Seção de requerimentos: título "Meus Requerimentos", botão "＋ Nova Solicitação" (`id="btn-nova-solicitacao"`, alinhado à direita)
  - Tabela `<table id="tabela-requerimentos">` com colunas: Matéria, Tipo de Atestado, Status, Data — corpo vazio (preenchido via AJAX)
  - Mensagem de lista vazia `<div id="msg-sem-requerimentos" class="d-none">` exibida via JS quando lista estiver vazia
  - Modal Bootstrap `<div id="modal-solicitacao" class="modal fade">` contendo o formulário de solicitação (ver tarefa 20)
  - Incluir `@section Scripts` com referência a `aluno-dashboard.js`
  - _Requisitos: 2.1, 2.2, 2.3, 2.4, 2.8, 3.1_

- [x] 20. Criar formulário de solicitação dentro do Modal (parte da Dashboard.cshtml)
  - Dentro do `#modal-solicitacao`, adicionar:
    - `<select id="select-materia">` (populado via AJAX)
    - `<textarea id="motivo" maxlength="500">` com contador de caracteres `<span id="contador-motivo">`
    - Grupo de `<input type="radio" name="tipoAtestado">` com opções "medico", "trabalho", "obito"
    - `<input id="arquivo" type="file" accept=".pdf,.jpg,.jpeg,.png">`
    - `<div id="preview-arquivo">` para exibir ícone PDF ou thumbnail
    - Botão "Enviar" (`id="btn-enviar"`) e botão "Cancelar"
    - `<div class="spinner-border d-none" id="spinner-envio">`
    - `<div id="alerta-resultado" class="d-none">`
    - Token CSRF via `@Html.AntiForgeryToken()`
  - _Requisitos: 3.1, 3.2, 3.8, 3.9, 4.5, 7.1, 7.2, 7.3, 7.4, 7.5, 7.6, 8.1_

- [x] 21. Implementar JavaScript do Dashboard (`wwwroot/js/aluno-dashboard.js`)
  - [x] 21.1 Criar `wwwroot/js/aluno-dashboard.js` com as funções base:
    - `inicializarDashboard()`: chamada em `DOMContentLoaded`; chama `carregarRequerimentos()` e configura event listeners
    - `obterClasseStatus(status)`: retorna `"warning"` para "Pendente", `"success"` para "Aprovado", `"danger"` para "Negado"
    - `renderizarLinhaRequerimento(requerimento)`: retorna string HTML `<tr>` com badge de status
    - _Requisitos: 2.2, 2.3, 2.4, 2.5, 2.6, 2.7_
  - [x] 21.2 Implementar funções de carregamento de dados:
    - `carregarRequerimentos()`: fetch `GET /aluno/requerimentos`; renderizar linhas na tabela; exibir/ocultar `#msg-sem-requerimentos`
    - `carregarMaterias()`: fetch `GET /aluno/materias`; popular `#select-materia` com `<option>` para cada matéria
    - _Requisitos: 2.2, 2.3, 2.4, 3.1, 3.2_
  - [x] 21.3 Implementar funções do Modal:
    - `abrirModalSolicitacao()`: abrir modal Bootstrap; chamar `carregarMaterias()`
    - `fecharModalSolicitacao()`: fechar modal; limpar todos os campos do formulário
    - Vincular `#btn-nova-solicitacao` ao `abrirModalSolicitacao()`
    - _Requisitos: 3.1, 3.8_
  - [x] 21.4 Implementar validação e envio do formulário:
    - `validarFormulario()`: verificar que matéria, motivo, tipo de atestado e arquivo estão preenchidos; retornar `false` se qualquer campo ausente
    - `exibirPreviewArquivo(arquivo)`: exibir ícone PDF para `.pdf` ou `<img>` thumbnail para imagens
    - `submeterSolicitacao(evento)`: chamar `validarFormulario()`; exibir spinner; montar `FormData`; fetch `POST /requerimento/criar`; chamar `exibirResultado`; se sucesso, chamar `carregarRequerimentos()` e fechar modal após delay
    - `exibirResultado(sucesso, mensagem)`: exibir `#alerta-resultado` com classe `alert-success` ou `alert-danger`
    - _Requisitos: 3.4, 3.5, 3.7, 3.8, 3.9, 7.1, 7.2, 7.3, 7.4, 7.5, 7.6_
  - [ ]* 21.5 Escrever testes para funções JavaScript puras
    - **Propriedade 9: Badge de status sempre exibe a classe CSS correta**
    - **Valida: Requisitos 2.5, 2.6, 2.7**
    - **Propriedade 11: Validação de formulário rejeita qualquer campo obrigatório ausente**
    - **Valida: Requisitos 7.1, 7.2, 7.3, 7.4**

- [ ] 22. Checkpoint final — Garantir que tudo está integrado
  - Garantir que o projeto compila sem erros (`dotnet build`)
  - Verificar que todas as rotas estão acessíveis e os controllers retornam as Views corretas
  - Verificar que a migration foi aplicada e o banco contém o campo `Motivo`
  - Garantir que todos os testes passam, perguntar ao usuário se há dúvidas

## Notas

- Tarefas marcadas com `*` são opcionais e podem ser puladas para um MVP mais rápido
- Cada tarefa referencia requisitos específicos para rastreabilidade
- Os checkpoints garantem validação incremental antes de avançar para a próxima camada
- As propriedades de corretude validam comportamentos universais do sistema
- Os testes unitários validam casos específicos e condições de borda
- A ordem das tarefas respeita as dependências: infraestrutura → modelos → serviços → controllers → views → JavaScript
