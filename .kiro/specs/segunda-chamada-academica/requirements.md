# Documento de Requisitos: Sistema de Segunda Chamada Acadêmica

## Introdução

Este documento descreve os requisitos funcionais e não-funcionais do **Sistema de Segunda Chamada Acadêmica**, derivados do design técnico aprovado. O sistema permite que alunos submetam requerimentos de segunda chamada de provas com atestados digitais, enquanto professores e a coordenação gerenciam o fluxo de aprovação. A aplicação é construída sobre ASP.NET Core MVC com Entity Framework Core, SQL Server, Bootstrap 5 e jQuery.

---

## Glossário

- **Sistema**: A aplicação web de Segunda Chamada Acadêmica (ASP.NET Core MVC).
- **Aluno**: Usuário com perfil `AlunoEng` ou `AlunoSI`, que submete requerimentos.
- **Coordenador**: Usuário com perfil `Coordenador`, responsável por aprovar ou negar requerimentos.
- **Professor**: Usuário com perfil `Professor`, que pode visualizar requerimentos relacionados às suas matérias.
- **Requerimento**: Solicitação de segunda chamada criada por um aluno, contendo matéria, motivo, tipo de atestado e arquivo comprobatório.
- **Atestado**: Arquivo digital (PDF, JPG, JPEG ou PNG) que comprova o motivo da ausência.
- **AuthService**: Serviço responsável pela autenticação de usuários.
- **AlunoService**: Serviço responsável por dados e requerimentos do aluno.
- **RequerimentoService**: Serviço responsável pela criação e consulta de requerimentos.
- **ArquivoService**: Serviço responsável pelo upload e armazenamento de arquivos.
- **AutorizarPerfilAttribute**: Filtro de autorização que protege controllers e actions por perfil de usuário.
- **Session**: Mecanismo de sessão HTTP do ASP.NET Core usado para manter o estado de autenticação.
- **Dashboard**: Página principal do aluno após autenticação, exibindo dados do perfil e histórico de requerimentos.
- **Modal**: Componente de interface Bootstrap que exibe o formulário de solicitação de segunda chamada.

---

## Requisitos

### Requisito 1: Autenticação de Usuários

**User Story:** Como usuário do sistema (aluno, professor ou coordenador), quero fazer login com minha matrícula e senha, para que eu possa acessar as funcionalidades correspondentes ao meu perfil.

#### Critérios de Aceitação

1. WHEN um usuário submete matrícula e senha válidas, THE AuthService SHALL validar as credenciais consultando o banco de dados e retornar um `UsuarioAutenticadoDto` com os dados do usuário.
2. WHEN as credenciais são válidas, THE Sistema SHALL armazenar o identificador e o perfil do usuário na Session e redirecionar para o dashboard correspondente ao perfil.
3. WHEN um usuário com perfil `AlunoEng` ou `AlunoSI` faz login com sucesso, THE Sistema SHALL redirecionar para `/aluno/dashboard`.
4. WHEN um usuário submete matrícula ou senha inválidas, THE AuthService SHALL retornar `null` e THE Sistema SHALL exibir a View de login com mensagem de erro.
5. WHEN um usuário autenticado acessa `POST /auth/logout`, THE Sistema SHALL encerrar a Session e redirecionar para `/auth/login`.
6. IF a matrícula fornecida for menor ou igual a zero, THEN THE AuthService SHALL rejeitar a validação sem consultar o banco de dados.

---

### Requisito 2: Dashboard do Aluno

**User Story:** Como aluno autenticado, quero visualizar meus dados acadêmicos e o histórico dos meus requerimentos, para que eu possa acompanhar o status das minhas solicitações.

#### Critérios de Aceitação

1. WHEN um aluno acessa `GET /aluno/dashboard`, THE Sistema SHALL exibir a View do dashboard com os dados do aluno (matrícula, curso, período, turno).
2. WHEN o dashboard é carregado, THE Sistema SHALL realizar uma requisição AJAX para `GET /aluno/requerimentos` e renderizar a tabela de histórico.
3. WHEN a lista de requerimentos é retornada, THE AlunoService SHALL garantir que os requerimentos estejam ordenados por `DataCriacao` em ordem decrescente.
4. WHEN a lista de requerimentos está vazia, THE Sistema SHALL exibir uma mensagem indicando que não há requerimentos cadastrados.
5. WHEN um requerimento possui status "Pendente", THE Sistema SHALL exibir o badge com cor âmbar (`warning`).
6. WHEN um requerimento possui status "Aprovado", THE Sistema SHALL exibir o badge com cor verde (`success`).
7. WHEN um requerimento possui status "Negado", THE Sistema SHALL exibir o badge com cor vermelha (`danger`).
8. WHILE o aluno está autenticado, THE Sistema SHALL exibir a navbar do aluno (`_NavbarAluno.cshtml`) em todas as páginas do perfil aluno.

---

### Requisito 3: Solicitação de Segunda Chamada

**User Story:** Como aluno autenticado, quero submeter um requerimento de segunda chamada com o atestado comprobatório, para que eu possa solicitar a realização de uma prova que perdi por motivo justificado.

#### Critérios de Aceitação

1. WHEN o aluno clica no botão "Nova Solicitação", THE Sistema SHALL abrir o Modal de solicitação e carregar a lista de matérias via `GET /aluno/materias`.
2. WHEN o Modal é aberto, THE Sistema SHALL popular o `<select>` de matérias com os dados retornados pelo `AlunoService`.
3. WHEN o aluno submete o formulário com todos os campos válidos (matéria, motivo, tipo de atestado e arquivo), THE RequerimentoService SHALL criar o requerimento com `Status = "Pendente"` e `DataCriacao = DateTime.UtcNow`.
4. WHEN o requerimento é criado com sucesso, THE Sistema SHALL retornar JSON `{ sucesso: true, id: <int>, mensagem: "Requerimento criado com sucesso." }` com HTTP 200.
5. WHEN o aluno submete o formulário com campos obrigatórios ausentes, THE Sistema SHALL retornar JSON `{ sucesso: false, id: null, mensagem: "Arquivo inválido ou campos obrigatórios ausentes." }` com HTTP 400.
6. WHEN o tipo de atestado informado não pertence ao conjunto `{"medico", "trabalho", "obito"}`, THE RequerimentoService SHALL rejeitar a criação do requerimento.
7. WHEN o requerimento é criado com sucesso, THE Sistema SHALL atualizar a tabela de histórico no dashboard sem recarregar a página.
8. WHEN o aluno clica em "Cancelar" no Modal, THE Sistema SHALL fechar o Modal e limpar todos os campos do formulário.
9. WHILE o formulário está sendo enviado, THE Sistema SHALL exibir um spinner de carregamento e desabilitar o botão de envio para evitar submissões duplicadas.
10. IF a sessão do aluno estiver expirada ao tentar criar um requerimento, THEN THE Sistema SHALL retornar HTTP 401 com mensagem `"Sessão expirada."`.

---

### Requisito 4: Upload de Atestado

**User Story:** Como aluno, quero fazer upload do meu atestado digital, para que eu possa comprovar o motivo da minha ausência na prova.

#### Critérios de Aceitação

1. WHEN um arquivo com extensão `.pdf`, `.jpg`, `.jpeg` ou `.png` e tamanho menor ou igual a 10 MB é enviado para `POST /arquivo/upload`, THE ArquivoService SHALL salvar o arquivo em `wwwroot/uploads/` e retornar a URL relativa acessível publicamente.
2. WHEN o arquivo é salvo, THE ArquivoService SHALL gerar o nome do arquivo usando `Guid.NewGuid()` concatenado com a extensão original, nunca utilizando o nome original do arquivo.
3. WHEN um arquivo com extensão não permitida é enviado, THE ArquivoService SHALL rejeitar o upload e THE Sistema SHALL retornar HTTP 400 com mensagem `"Tipo de arquivo não permitido."`.
4. WHEN um arquivo com tamanho superior a 10 MB é enviado, THE ArquivoService SHALL rejeitar o upload e THE Sistema SHALL retornar HTTP 400.
5. WHEN um arquivo é selecionado no formulário, THE Sistema SHALL exibir um preview: ícone PDF para arquivos `.pdf` ou thumbnail para imagens (`.jpg`, `.jpeg`, `.png`).
6. THE ArquivoService SHALL validar tanto a extensão do arquivo quanto o MIME type para prevenir uploads maliciosos.

---

### Requisito 5: Controle de Acesso por Perfil

**User Story:** Como administrador do sistema, quero que cada perfil de usuário acesse apenas as funcionalidades que lhe são permitidas, para que a segurança e a integridade dos dados sejam mantidas.

#### Critérios de Aceitação

1. WHEN uma requisição é feita a um endpoint protegido pelo `AutorizarPerfilAttribute` sem uma Session válida, THE Sistema SHALL redirecionar para `/auth/login`.
2. WHEN uma requisição é feita a um endpoint protegido com um perfil de usuário não autorizado, THE Sistema SHALL redirecionar para `/auth/login`.
3. THE AlunoController SHALL ser acessível apenas por usuários com perfil `AlunoEng` ou `AlunoSI`.
4. THE RequerimentoController SHALL ser acessível apenas por usuários com perfil `AlunoEng` ou `AlunoSI`.
5. WHEN o `AutorizarPerfilAttribute` é executado, THE Sistema SHALL verificar a presença e o valor da chave `"Perfil"` na Session antes de permitir o acesso.

---

### Requisito 6: Consulta de Requerimento

**User Story:** Como aluno autenticado, quero visualizar os detalhes de um requerimento específico, para que eu possa acompanhar todas as informações da minha solicitação.

#### Critérios de Aceitação

1. WHEN uma requisição `GET /requerimento/{id}` é feita com um `id` válido existente no banco, THE RequerimentoService SHALL retornar um `RequerimentoDetalheDto` com todos os campos do requerimento.
2. IF o `id` informado não corresponder a nenhum requerimento no banco, THEN THE RequerimentoService SHALL retornar `null` e THE Sistema SHALL responder com HTTP 404.
3. IF o `id` informado for menor ou igual a zero, THEN THE RequerimentoService SHALL rejeitar a consulta sem acessar o banco de dados.
4. THE RequerimentoDetalheDto SHALL conter os campos: `Id`, `MatriculaAluno`, `NomeMateria`, `Motivo`, `TipoAtestado`, `URLAtestado`, `Status` e `DataCriacao`.

---

### Requisito 7: Validação de Formulário no Cliente

**User Story:** Como aluno, quero receber feedback imediato sobre erros no formulário antes de enviar, para que eu possa corrigir os dados sem aguardar resposta do servidor.

#### Critérios de Aceitação

1. WHEN o aluno tenta submeter o formulário de solicitação sem selecionar uma matéria, THE Sistema SHALL impedir o envio e exibir mensagem de validação.
2. WHEN o aluno tenta submeter o formulário sem preencher o campo motivo, THE Sistema SHALL impedir o envio e exibir mensagem de validação.
3. WHEN o aluno tenta submeter o formulário sem selecionar o tipo de atestado, THE Sistema SHALL impedir o envio e exibir mensagem de validação.
4. WHEN o aluno tenta submeter o formulário sem anexar um arquivo, THE Sistema SHALL impedir o envio e exibir mensagem de validação.
5. WHEN o campo motivo possui mais de 500 caracteres, THE Sistema SHALL impedir o envio e exibir mensagem indicando o limite máximo.
6. WHEN o resultado do envio é recebido do servidor, THE Sistema SHALL exibir alerta verde para sucesso ou alerta vermelho para erro dentro do Modal.

---

### Requisito 8: Segurança e Proteção de Dados

**User Story:** Como administrador do sistema, quero que a aplicação implemente medidas de segurança adequadas, para que os dados dos alunos e os arquivos enviados sejam protegidos contra ataques.

#### Critérios de Aceitação

1. THE Sistema SHALL incluir o token `__RequestVerificationToken` (Anti-CSRF) em todos os formulários com método POST.
2. WHEN um formulário POST é recebido sem token CSRF válido, THE Sistema SHALL rejeitar a requisição.
3. THE Sistema SHALL utilizar LINQ parametrizado via Entity Framework Core em todas as consultas ao banco de dados, sem uso de SQL raw.
4. THE ArquivoService SHALL gerar nomes de arquivo com `Guid.NewGuid()` para prevenir ataques de path traversal.
5. WHERE o ambiente for de produção, THE Sistema SHALL implementar hash de senha utilizando algoritmo seguro (BCrypt ou PBKDF2) antes de armazenar no banco de dados.

---

## Propriedades de Corretude

*Uma propriedade é uma característica ou comportamento que deve ser verdadeiro em todas as execuções válidas do sistema — essencialmente, uma declaração formal sobre o que o sistema deve fazer. As propriedades servem como ponte entre especificações legíveis por humanos e garantias de corretude verificáveis por máquina.*

### Propriedade 1: Autenticação redireciona para o dashboard correto por perfil

*Para qualquer* usuário válido cadastrado no banco com um perfil definido, ao realizar login com credenciais corretas, o sistema deve redirecionar para o dashboard correspondente ao perfil do usuário.

**Valida: Requisitos 1.1, 1.2, 1.3**

---

### Propriedade 2: Credenciais inválidas nunca resultam em autenticação

*Para qualquer* combinação de matrícula e senha que não corresponda a um usuário cadastrado no banco, o `AuthService` deve retornar `null` e o sistema nunca deve criar uma sessão autenticada.

**Valida: Requisitos 1.4, 1.6**

---

### Propriedade 3: Requerimento criado sempre inicia com status Pendente

*Para qualquer* requerimento criado com dados válidos (matéria não vazia, tipo de atestado válido, URL de atestado não vazia), o `RequerimentoService` deve persistir o requerimento com `Status = "Pendente"` e `DataCriacao` preenchida.

**Valida: Requisitos 3.3, 3.4**

---

### Propriedade 4: Round-trip de requerimento preserva todos os dados

*Para qualquer* requerimento criado com dados válidos, buscar o requerimento pelo ID retornado deve produzir um `RequerimentoDetalheDto` com os mesmos valores de todos os campos originais.

**Valida: Requisitos 3.3, 6.1, 6.4**

---

### Propriedade 5: Tipo de atestado inválido sempre é rejeitado

*Para qualquer* valor de `TipoAtestado` que não pertença ao conjunto `{"medico", "trabalho", "obito"}`, o `RequerimentoService` deve rejeitar a criação do requerimento.

**Valida: Requisito 3.6**

---

### Propriedade 6: Upload com extensão inválida sempre é rejeitado

*Para qualquer* arquivo com extensão fora do conjunto `{".pdf", ".jpg", ".jpeg", ".png"}`, o `ArquivoService` deve rejeitar o upload e retornar erro.

**Valida: Requisitos 4.3, 4.6**

---

### Propriedade 7: Nome do arquivo salvo nunca contém o nome original

*Para qualquer* arquivo enviado com qualquer nome original, o `ArquivoService` deve salvar o arquivo com um nome gerado por `Guid.NewGuid()`, de forma que o nome salvo nunca seja igual ao nome original do arquivo.

**Valida: Requisitos 4.2, 8.4**

---

### Propriedade 8: Acesso sem sessão válida sempre redireciona para login

*Para qualquer* endpoint protegido pelo `AutorizarPerfilAttribute`, uma requisição sem Session válida deve sempre resultar em redirecionamento para `/auth/login`, independentemente do endpoint acessado.

**Valida: Requisitos 5.1, 5.2, 5.5**

---

### Propriedade 9: Badge de status sempre exibe a classe CSS correta

*Para qualquer* valor de status válido (`"Pendente"`, `"Aprovado"`, `"Negado"`), a função `obterClasseStatus` deve retornar respectivamente `"warning"`, `"success"` e `"danger"`.

**Valida: Requisitos 2.5, 2.6, 2.7**

---

### Propriedade 10: Histórico de requerimentos sempre ordenado por data decrescente

*Para qualquer* lista de requerimentos de um aluno, o `AlunoService` deve retornar os requerimentos ordenados por `DataCriacao` em ordem decrescente, de forma que o requerimento mais recente sempre apareça primeiro.

**Valida: Requisito 2.3**

---

### Propriedade 11: Validação de formulário rejeita qualquer campo obrigatório ausente

*Para qualquer* combinação de campos obrigatórios ausentes no formulário de solicitação (matéria, motivo, tipo de atestado ou arquivo), a função `validarFormulario` deve retornar `false` e impedir o envio.

**Valida: Requisitos 7.1, 7.2, 7.3, 7.4**
