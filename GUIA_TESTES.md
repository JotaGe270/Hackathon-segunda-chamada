# 🧪 Guia de Testes - Sistema de Perfil de Coordenador

## 🎬 Como Testar o Sistema

### Pré-requisitos
- SQL Server LocalDB rodando
- Projeto compilado com sucesso
- Migrations executadas

---

## 📋 Dados de Teste para Inserir no Banco

### 1. Inserir Usuário Coordenador
Execute no SQL Server Management Studio:

```sql
INSERT INTO Usuarios (Matricula, SenhaHash, Perfil)
VALUES (999999, 'senha123', 3);  -- 3 = Coordenador
```

### 2. Inserir Usuários Alunos (para gerar requerimentos)
```sql
INSERT INTO Usuarios (Matricula, SenhaHash, Perfil)
VALUES 
  (201234567, 'senha123', 1),  -- 1 = AlunoEng
  (201234568, 'senha123', 2),  -- 2 = AlunoSI
  (201234569, 'senha123', 1),  -- 1 = AlunoEng
  (201234570, 'senha123', 2);  -- 2 = AlunoSI
```

### 3. Inserir Requerimentos de Teste (Pendentes)
```sql
INSERT INTO RequerimentosSegundaChamada 
  (MatriculaAluno, NomeMateria, Motivo, TipoAtestado, URLAtestado, Status, DataCriacao)
VALUES 
  (201234567, 'Cálculo Diferencial e Integral I', 'Estava doente com febre alta', 'Médico', 
   'https://drive.google.com/file/d/exemplo1', 'Pendente', GETDATE()),
  
  (201234568, 'Fundamentos de Banco de Dados', 'Obito familiar - tio faleceu', 'Óbito', 
   'https://drive.google.com/file/d/exemplo2', 'Pendente', GETDATE()),
  
  (201234569, 'Física para Engenharia', 'Viagem para trabalho', 'Trabalho', 
   'https://drive.google.com/file/d/exemplo3', 'Pendente', GETDATE());
```

### 4. Inserir Requerimentos Aprovados
```sql
INSERT INTO RequerimentosSegundaChamada 
  (MatriculaAluno, NomeMateria, Motivo, TipoAtestado, URLAtestado, Status, MotivoRecusa, DataCriacao)
VALUES 
  (201234567, 'Geometria Analítica', 'Internado no hospital', 'Médico', 
   'https://drive.google.com/file/d/exemplo4', 'Aprovado', NULL, DATEADD(DAY, -1, GETDATE())),
  
  (201234568, 'Redes de Computadores', 'Estava com covid', 'Médico', 
   'https://drive.google.com/file/d/exemplo5', 'Aprovado', NULL, DATEADD(DAY, -2, GETDATE())),
  
  (201234569, 'Algoritmos e Lógica de Programação', 'Viagem para congresso', 'Trabalho', 
   'https://drive.google.com/file/d/exemplo6', 'Aprovado', NULL, DATEADD(DAY, -3, GETDATE()));
```

### 5. Inserir Requerimentos Negados
```sql
INSERT INTO RequerimentosSegundaChamada 
  (MatriculaAluno, NomeMateria, Motivo, TipoAtestado, URLAtestado, Status, MotivoRecusa, DataCriacao)
VALUES 
  (201234570, 'Desenvolvimento Web', 'Estava com gripe', 'Médico', 
   'https://drive.google.com/file/d/exemplo7', 'Negado', 'Atestado não apresenta período de repouso recomendado', DATEADD(DAY, -5, GETDATE())),
  
  (201234568, 'Engenharia de Requisitos', 'Razões pessoais', 'Outro', 
   'https://drive.google.com/file/d/exemplo8', 'Negado', 'Não há documentação comprovando o motivo da ausência', DATEADD(DAY, -4, GETDATE()));
```

---

## 🔓 Credenciais de Teste

### Login como Coordenador
```
Matrícula: 999999
Senha: senha123
```

### Login como Aluno (para criar requerimentos)
```
Matrícula: 201234567
Senha: senha123
```

---

## 🧪 Casos de Teste Recomendados

### 1. ✅ Acessar Perfil do Coordenador
- [ ] Login com coordenador (999999)
- [ ] Verificar se redireciona para Painel
- [ ] Clicar em "Meu Perfil" na navbar
- [ ] Validar:
  - Exibição correta da matrícula
  - Cards de estatísticas mostram valores corretos
  - Tabela de requerimentos recentes está preenchida
  - Links de ação funcionam

### 2. ✅ Visualizar Painel
- [ ] No perfil, clicar em "Painel" ou "Ver todos"
- [ ] Validar:
  - Todos os requerimentos são exibidos
  - Filtros funcionam (status, matéria, data)
  - Botões de ação aparecem apenas para pendentes
  - Tabela é responsiva no mobile

### 3. ✅ Ver Detalhes de um Requerimento
- [ ] No painel, clicar no botão "Ver" de um requerimento
- [ ] Validar:
  - Modal abre corretamente
  - Todos os dados são exibidos (matrícula, matéria, motivo, etc)
  - Link do documento está acessível
  - Motivo de recusa aparece (se tiver sido negado)

### 4. ✅ Aprovar um Requerimento Pendente
- [ ] No painel, clicar no botão "Aprovar"
- [ ] Validar:
  - Modal de confirmação abre
  - Clique em "Aprovar"
  - Mensagem de sucesso aparece
  - Requerimento muda status para "Aprovado" (verde)
  - Botões de ação desaparecem

### 5. ✅ Negar um Requerimento Pendente
- [ ] No painel, clicar no botão "Negar"
- [ ] Validar:
  - Modal de negação abre com textarea
  - Textarea é obrigatório (tentar enviar vazio = erro)
  - Preencher com motivo e confirmar
  - Mensagem de sucesso aparece
  - Requerimento muda status para "Negado" (vermelho)
  - Motivo aparece em "Ver Detalhes"

### 6. ✅ Criar Novo Requerimento (como Coordenador)
- [ ] No painel, clicar em "Novo Requerimento"
- [ ] Validar:
  - Modal abre
  - Preencher matrícula de um aluno (201234567)
  - Matérias carregam automaticamente via AJAX
  - Preencher todos os campos
  - Confirmar
  - Requerimento aparece no painel

### 7. ✅ Gerar Relatório
- [ ] No perfil, clicar em "Relatório"
- [ ] Validar:
  - Modal abre com filtros
  - Campos de data e status estão disponíveis
  - Botão de gerar funciona (implementar backend se necessário)

### 8. ✅ Logout
- [ ] Clicar em "Sair" na navbar
- [ ] Validar:
  - Redireciona para login
  - Cookie é destruído
  - Não consegue acessar /Coordenador/Perfil sem logar novamente

### 9. ✅ Segurança - Acesso Não Autorizado
- [ ] Tentar acessar `/Coordenador/Perfil` sem estar logado
  - [ ] Deve redirecionar para login
- [ ] Logar como Aluno e tentar acessar `/Coordenador/Perfil`
  - [ ] Deve exibir "Access Denied" ou redirecionar

### 10. ✅ Responsividade
- [ ] Abrir perfil no celular (DevTools F12)
- [ ] Validar:
  - Navbar colapsa corretamente
  - Cards estão em uma coluna
  - Tabela fica scrollável horizontalmente
  - Botões de ação ficam acessíveis

---

## 🐛 Testes de Erro

### 1. Campo Obrigatório Vazio
- [ ] Modal de negar vazio → erro
- [ ] Matrícula aluno vazia → erro
- [ ] URL atestado inválida → erro HTML5

### 2. Validação Anti-CSRF
- [ ] Tentar manipular formulário sem token → erro 400

### 3. Matrícula Inválida
- [ ] Buscar matérias com matrícula inexistente → JSON erro
- [ ] Buscar matérias com matrícula negativa → JSON erro

---

## 📊 Esperados para Teste Manual

Com os dados inseridos acima, o perfil deve mostrar:

```
┌─────────────────────────────────────────┐
│           ESTATÍSTICAS ESPERADAS        │
├─────────────────────────────────────────┤
│ Pendentes: 3                            │
│ Aprovados: 3                            │
│ Negados: 2                              │
│ Total: 8                                │
└─────────────────────────────────────────┘

┌─────────────────────────────────────────┐
│        ÚLTIMOS 5 REQUERIMENTOS          │
├─────────────────────────────────────────┤
│ 1. Negar 2 (Eng. Req.) - Negado        │
│ 2. Negar 1 (Desenvolv. Web) - Negado   │
│ 3. Pendente 3 (Física) - Pendente      │
│ 4. Pendente 2 (Banco Dados) - Pendente │
│ 5. Pendente 1 (Cálculo I) - Pendente   │
└─────────────────────────────────────────┘
```

---

## 🔍 Checklist Final

- [ ] Projeto compila sem erros
- [ ] Migrations executadas com sucesso
- [ ] Dados de teste inseridos no BD
- [ ] Login funciona com coordenador
- [ ] Perfil exibe corretamente
- [ ] Painel lista todos os requerimentos
- [ ] Estatísticas estão corretas
- [ ] Aprovar requerimento funciona
- [ ] Negar requerimento funciona (com motivo obrigatório)
- [ ] Ver detalhes funciona
- [ ] Filtros funcionam
- [ ] Responsivo em mobile
- [ ] Logout funciona
- [ ] Segurança por role funciona

---

## 📝 SQL Script Completo

Para executar tudo de uma vez:

```sql
-- Limpar dados anteriores (opcional)
DELETE FROM RequerimentosSegundaChamada;
DELETE FROM Usuarios;

-- Inserir coordenador
INSERT INTO Usuarios (Matricula, SenhaHash, Perfil) VALUES (999999, 'senha123', 3);

-- Inserir alunos
INSERT INTO Usuarios (Matricula, SenhaHash, Perfil) VALUES 
  (201234567, 'senha123', 1),
  (201234568, 'senha123', 2),
  (201234569, 'senha123', 1),
  (201234570, 'senha123', 2);

-- Inserir requerimentos pendentes
INSERT INTO RequerimentosSegundaChamada 
  (MatriculaAluno, NomeMateria, Motivo, TipoAtestado, URLAtestado, Status, DataCriacao)
VALUES 
  (201234567, 'Cálculo Diferencial e Integral I', 'Estava doente com febre alta', 'Médico', 'https://exemplo.com/doc1', 'Pendente', GETDATE()),
  (201234568, 'Fundamentos de Banco de Dados', 'Óbito familiar - tio faleceu', 'Óbito', 'https://exemplo.com/doc2', 'Pendente', GETDATE()),
  (201234569, 'Física para Engenharia', 'Viagem para trabalho', 'Trabalho', 'https://exemplo.com/doc3', 'Pendente', GETDATE());

-- Inserir requerimentos aprovados
INSERT INTO RequerimentosSegundaChamada 
  (MatriculaAluno, NomeMateria, Motivo, TipoAtestado, URLAtestado, Status, DataCriacao)
VALUES 
  (201234567, 'Geometria Analítica', 'Internado no hospital', 'Médico', 'https://exemplo.com/doc4', 'Aprovado', DATEADD(DAY, -1, GETDATE())),
  (201234568, 'Redes de Computadores', 'Estava com covid', 'Médico', 'https://exemplo.com/doc5', 'Aprovado', DATEADD(DAY, -2, GETDATE())),
  (201234569, 'Algoritmos e Lógica de Programação', 'Viagem para congresso', 'Trabalho', 'https://exemplo.com/doc6', 'Aprovado', DATEADD(DAY, -3, GETDATE()));

-- Inserir requerimentos negados
INSERT INTO RequerimentosSegundaChamada 
  (MatriculaAluno, NomeMateria, Motivo, TipoAtestado, URLAtestado, Status, MotivoRecusa, DataCriacao)
VALUES 
  (201234570, 'Desenvolvimento Web', 'Estava com gripe', 'Médico', 'https://exemplo.com/doc7', 'Negado', 'Atestado não apresenta período de repouso recomendado', DATEADD(DAY, -5, GETDATE())),
  (201234568, 'Engenharia de Requisitos', 'Razões pessoais', 'Outro', 'https://exemplo.com/doc8', 'Negado', 'Não há documentação comprovando o motivo da ausência', DATEADD(DAY, -4, GETDATE()));

-- Verificar dados inseridos
SELECT COUNT(*) as TotalRequerimentos FROM RequerimentosSegundaChamada;
SELECT COUNT(*) as TotalUsuarios FROM Usuarios;
SELECT * FROM RequerimentosSegundaChamada ORDER BY DataCriacao DESC;
```

---

**Versão:** 1.0
**Data:** 24/05/2025
**Status:** Pronto para testes
