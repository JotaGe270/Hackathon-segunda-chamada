# 🚀 Quick Start - Sistema de Perfil de Coordenador

## 1️⃣ Preparação Inicial

### Verificar se o projeto compila
```bash
# No Visual Studio: Build → Build Solution (Ctrl+Shift+B)
# OU no terminal PowerShell:
dotnet build
```

### Verificar se SQL Server LocalDB está rodando
```powershell
# Verificar status
sqllocaldb info
sqllocaldb start mssqllocaldb
```

---

## 2️⃣ Atualizar Database

```bash
# Se houver novas migrations:
dotnet ef database update
```

---

## 3️⃣ Inserir Dados de Teste

Executar este script no SQL Server Management Studio:

```sql
-- Usuário Coordenador
INSERT INTO Usuarios (Matricula, SenhaHash, Perfil) 
VALUES (999999, 'senha123', 3);

-- Usuários Alunos
INSERT INTO Usuarios (Matricula, SenhaHash, Perfil) VALUES 
  (201234567, 'senha123', 1),
  (201234568, 'senha123', 2);

-- Requerimentos Pendentes
INSERT INTO RequerimentosSegundaChamada 
  (MatriculaAluno, NomeMateria, Motivo, TipoAtestado, URLAtestado, Status, DataCriacao)
VALUES 
  (201234567, 'Cálculo Diferencial e Integral I', 'Estava doente', 'Médico', 
   'https://exemplo.com/doc1', 'Pendente', GETDATE()),
  (201234568, 'Fundamentos de Banco de Dados', 'Óbito familiar', 'Óbito', 
   'https://exemplo.com/doc2', 'Pendente', GETDATE());

-- Requerimentos Aprovados
INSERT INTO RequerimentosSegundaChamada 
  (MatriculaAluno, NomeMateria, Motivo, TipoAtestado, URLAtestado, Status, DataCriacao)
VALUES 
  (201234567, 'Geometria Analítica', 'Internado no hospital', 'Médico', 
   'https://exemplo.com/doc3', 'Aprovado', DATEADD(DAY, -1, GETDATE()));

-- Requerimentos Negados
INSERT INTO RequerimentosSegundaChamada 
  (MatriculaAluno, NomeMateria, Motivo, TipoAtestado, URLAtestado, Status, MotivoRecusa, DataCriacao)
VALUES 
  (201234568, 'Desenvolvimento Web', 'Estava com gripe', 'Médico', 
   'https://exemplo.com/doc4', 'Negado', 'Atestado ilegível', DATEADD(DAY, -5, GETDATE()));
```

---

## 4️⃣ Rodar a Aplicação

### Opção A: Visual Studio
1. Pressione **F5** ou clique em **Start** (botão verde)
2. Browser abre automaticamente

### Opção B: Terminal
```powershell
dotnet run
# Browser abre em: https://localhost:7162
```

---

## 5️⃣ Fazer Login

### Como Coordenador
```
URL: https://localhost:7162/Account/Login
Matrícula: 999999
Senha: senha123
```

### Como Aluno
```
Matrícula: 201234567
Senha: senha123
```

---

## 6️⃣ Acessar as Funcionalidades

### 📊 Perfil do Coordenador
- URL: `/Coordenador/Perfil`
- Após login, clique em **"Meu Perfil"** na navbar

### 📋 Painel de Requerimentos
- URL: `/Coordenador/Painel`
- Após login, clique em **"Painel"** na navbar

---

## 📁 Estrutura de Pastas

```
Controllers/
  └── CoordenadorController.cs ← Nova action Perfil()

DTOs/
  └── PerfilCoordenadorDto.cs ← DTO novo

Views/
  ├── Coordenador/
  │   ├── Perfil.cshtml ← ✨ NOVO
  │   └── Painel.cshtml ← ✨ NOVO
  └── Shared/
      └── _NavbarCoordenador.cshtml ← ✨ NOVO

Documentação/
  ├── LEIA-ME.md ← Você está aqui
  ├── IMPLEMENTACAO_PERFIL_COORDENADOR.md
  ├── ARQUITETURA_SISTEMA.md
  └── GUIA_TESTES.md
```

---

## ✨ Funcionalidades Implementadas

### ✅ Perfil do Coordenador
- Dashboard com estatísticas
- 4 Cards: Pendentes, Aprovados, Negados, Total
- Tabela com últimos 5 requerimentos
- Ações rápidas

### ✅ Painel de Requerimentos
- Tabela com todos os requerimentos
- Filtros avançados
- Modal de detalhes
- Botões: Ver, Aprovar, Negar
- Modal de novo requerimento

### ✅ Gerenciamento de Requerimentos
- Aprovar com confirmação
- Negar com motivo obrigatório
- Visualizar detalhes completos
- Validação de segurança

---

## 🧪 Testar as Funcionalidades

1. **Ir para Perfil**
   - Clique em "Meu Perfil" na navbar
   - Veja as estatísticas
   - Clique em "Ver todos"

2. **Ir para Painel**
   - Clique em "Painel" na navbar
   - Veja todos os requerimentos
   - Teste os filtros

3. **Aprovar um Requerimento**
   - Encontre um com Status "Pendente"
   - Clique no botão "Aprovar"
   - Confirme na janela que abre
   - Status mudará para "Aprovado" (verde)

4. **Negar um Requerimento**
   - Encontre outro com Status "Pendente"
   - Clique no botão "Negar"
   - Digite o motivo
   - Clique em "Negar"
   - Status mudará para "Negado" (vermelho)

5. **Ver Detalhes**
   - Clique no botão "Ver" de qualquer requerimento
   - Modal abre com informações completas

---

## 🔧 Troubleshooting

### Erro: "Migrations not applied"
```bash
# Solução:
dotnet ef database update
```

### Erro: "Database not found"
```bash
# Verifique a connection string em appsettings.json
# Deve estar: "Server=(localdb)\\mssqllocaldb;Database=HackathonSegundaChamada..."

# Se preciso recriar:
dotnet ef database drop --force
dotnet ef database update
```

### Erro ao inserir dados de teste
- Verifique se o database foi criado
- Use SQL Server Management Studio
- Conecte em `(localdb)\mssqllocaldb`

### Página em branco ou erro 404
- Certifique-se que está logado como Coordenador
- Verifique a URL: `/Coordenador/Perfil` ou `/Coordenador/Painel`
- Abra o console (F12) para ver erros

---

## 📚 Documentação Completa

Leia os arquivos para mais informações:

- **IMPLEMENTACAO_PERFIL_COORDENADOR.md** - Tudo que foi implementado
- **ARQUITETURA_SISTEMA.md** - Como funciona internamente
- **GUIA_TESTES.md** - Como testar tudo

---

## ⌨️ Atalhos Úteis

| Ação | Atalho |
|------|--------|
| Compilar | `Ctrl + Shift + B` |
| Rodar | `F5` |
| Parar | `Shift + F5` |
| Abrir Browser | `Ctrl + Alt + B` |
| Developer Tools | `F12` |
| Reload página | `F5` |

---

## 🎯 Próximas Etapas

1. ✅ Código implementado
2. ✅ Projeto compilando
3. ⬜ Inserir dados de teste
4. ⬜ Rodar a aplicação
5. ⬜ Fazer login como coordenador
6. ⬜ Testar as funcionalidades
7. ⬜ Ler documentação completa

---

## 💡 Dicas

- Use Ctrl+Shift+F para buscar no projeto
- F5 no SQL Server para ver dados inseridos em tempo real
- F12 no browser para inspecionar elementos
- Acesse `/Account/Login` para voltar ao login

---

**Bem-vindo ao Sistema de Perfil de Coordenador! 🚀**

Qualquer dúvida, consulte:
- `GUIA_TESTES.md` para testes
- `ARQUITETURA_SISTEMA.md` para entender fluxos
- `IMPLEMENTACAO_PERFIL_COORDENADOR.md` para detalhes técnicos

---

**Status:** ✅ Pronto para usar  
**Última atualização:** 24/05/2025
