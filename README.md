# Segunda Chamada Acadêmica

Sistema web para solicitação e gestão de requerimentos de segunda chamada, desenvolvido em ASP.NET Core MVC com Entity Framework e SQL Server (LocalDB).

---

## Logins

| Perfil       | Matrícula | Senha |
|--------------|-----------|-------|
| Aluno ENG    | 1         | 123   |
| Aluno SI     | 2         | 123   |
| Coordenador  | 3         | 123   |
| Professor    | 4         | 123   |

---

## O que cada perfil faz

**Aluno** — visualiza seus dados (curso, período, turno), envia requerimentos de segunda chamada com upload de atestado e acompanha o status de cada pedido. Se um pedido for negado, o motivo da recusa aparece na tabela.

**Coordenador** — vê todos os requerimentos do sistema, pode filtrar por status (Pendente / Aprovado / Negado), aprovar ou negar pedidos (com justificativa obrigatória) e criar requerimentos manualmente em nome de um aluno.

**Professor** — visualiza apenas os requerimentos já aprovados pelo coordenador, separados por curso em duas abas: Engenharia e Sistemas de Informação.

---

## Estrutura do projeto

```
Controllers/
  AccountController       — login, logout e redirecionamento por perfil
  AlunoController         — dashboard e endpoints JSON do aluno
  CoordenadorController   — painel, aprovar, negar e criar requerimentos
  ProfessorController     — painel com alunos aprovados por curso
  RequerimentoSegundaChamadaController — endpoint de criação via fetch e busca de matérias

Services/
  AlunoService            — dados do aluno, matérias e histórico de requerimentos
  RequerimentoService     — criação e consulta de requerimentos
  ArquivoService          — upload e validação de arquivos (PDF, JPG, PNG, máx 10 MB)

Models/
  Usuario                 — entidade de usuário com enum de perfil
  RequerimentoSegundaChamada — entidade do requerimento

DTOs/                     — objetos de transferência de dados entre camadas
Data/AppDbContext         — contexto do EF Core com seed dos usuários iniciais
wwwroot/uploads/          — arquivos de atestado enviados pelos usuários
```

---

## Como rodar

1. Certifique-se de ter o .NET 10 SDK e SQL Server LocalDB instalados.
2. Clone o repositório.
3. Execute as migrations:
   ```
   dotnet ef database update
   ```
4. Rode o projeto:
   ```
   dotnet run
   ```
5. Acesse `https://localhost:{porta}` e faça login com um dos usuários acima.
