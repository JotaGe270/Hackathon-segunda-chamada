# 📋 Ficha de Acompanhamento e Diagnóstico do Projeto

> **Orientações para a Equipe:** Este documento deve ser preenchido pela equipe para alinhar as expectativas do projeto com os mentores e organizadores. Sejam diretos, honestos e realistas nas respostas.

---

## 🏛️ 1. Identificação da Equipe

- **Equipe 25:**
Gabriel Ronald da Silva Fabrício 5º Período de Engenharia de Software**

JOAO GABRIEL MIRANDA MACEDO 5º Período de Engenharia de Software

JOAO PEDRO YAMAGUTI DA SILVA 5º Período de Engenharia de Software

JOAO VITOR RODRIGUES DA SILVA MUNIZ 5º Período de Engenharia de Software

MARCELO HENRIQUE DA SILVA COSTA 5º Período de Engenharia de Software
**
- **https://github.com/JotaGe270/Hackathon-segunda-chamada**
- https://www.figma.com/design/6WrPaQOOGUKAhDrSg1E8xR/Untitled?node-id=0-1&t=QoemxKcBm13FyoSl-1

---

## 💡 2. O Problema e a Proposta de Valor (O Coração da Ideia)

### 2.1. Qual problema real e específico vocês estão resolvendo?

>  Nosso caso, o problema é a dificuldade que os alunos enfrentam para solicitar e acompanhar requerimentos de segunda chamada, e a falta de um sistema centralizado para coordenadores e professores gerenciarem esses pedidos.

### 2.2. O diferencial da solução está claro? O que torna a ideia de vocês única?

>  Nossa solução é um sistema web completo que integra todas as etapas do processo de requerimento de segunda chamada, desde o envio do pedido pelo aluno até a aprovação ou negação pelo coordenador, com uma interface intuitiva e funcionalidades específicas para cada perfil de usuário (aluno, coordenador e professor). Além disso, o sistema permite o upload de atestados médicos e gera relatórios para os professores, facilitando a gestão acadêmica.

---

## ⚙️ 3. A Solução na Prática (Como Funciona)

### 3.1. Como a solução funciona para o usuário final?

> a solução é um sistema web onde os alunos podem fazer login, visualizar seus dados acadêmicos, enviar requerimentos de segunda chamada com upload de atestados médicos e acompanhar o status de seus pedidos. Os coordenadores podem acessar um painel para gerenciar todos os requerimentos, aprovando ou negando pedidos com justificativas. Os professores têm acesso a uma interface que mostra os requerimentos aprovados por curso, facilitando a organização das aulas e a comunicação com os alunos.

### 3.2. Quais são as principais tecnologias, linguagens ou ferramentas que decidiram usar?

> ASP.NET Core MVC, Entity Framework, SQL Server (LocalDB), HTML, CSS, JavaScript.
---

## 👥 4. Gestão e Divisão de Trabalho

### 4.1. Quem está fazendo o quê na equipe?

- **João Gabriel Miranda Macedo** parte do back-end e um pouco do font-end
- ** João Pedro ** front-end
- **Marceelo ** Deu a ideia das telas e como ficaria 
- **João Vitor** Criação do Figma e apresentação do projeto
- **Gabriel** Criou a logica dos relacionamentos do banco de dados

---

## 🛠️ 5. Status Atual do Desenvolvimento (O MVP)

### 5.1. Vocês já começaram o protótipo visual ou o código do MVP? Qual o percentual de conclusão estimado?

- **Status:** ( ) Não começamos | ( ) Apenas rascunho visual | ( ) Código inicial iniciado | ( ) Mais da metade pronto | (x) Projeto finalizado

### 5.2. O projeto já funciona em alguma parte? O que já está codificado e operacional?

> Projeto finalizado, o sistema web para solicitação e gestão de requerimentos de segunda chamada está completo e funcional, com todas as funcionalidades descritas implementadas e testadas.

### 5.3. O que foi ou será "Mockado" (dados fictícios/estáticos)?

> a solução é um sistema web completo, portanto, não há partes "mockadas". Todos os dados são reais e operacionais, com funcionalidades implementadas para cada perfil de usuário (aluno, coordenador e professor).

### 5.4. O que ainda falta finalizar obrigatoriamente para a entrega?

> Ta finalizado e funcional

---

## 🚧 6. Obstáculos e Pedidos de Ajuda

### 6.1. Qual maior dificuldade da equipe?

> criação de login distintos, a logica de upload e validação de arquivos, garantindo que apenas formatos permitidos (PDF, JPG, PNG) e tamanhos adequados (máx 10 MB) sejam aceitos, além de implementar a funcionalidade de aprovação/negação com justificativas obrigatórias para os coordenadores.

---

## 🎤 7. Preparação para o Show (O Pitch)

### 7.1. Como será a estratégia de apresentação de vocês na segunda-feira?

> nossa estrategia é mostrar um Figma de como seria implementado caso o sistema seja aplicado na UGB no portal do Aluno, um video funcional do sistema em ação destacando as principais funcionalidades para cada perfil de usuário (aluno, coordenador e professor), e explicando como a solução resolve o problema identificado de forma eficiente e intuitiva.



----------------------------------------------------------------------------------------------------------------------------------------------------------------


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



## Configuração do Banco de Dados

Este projeto utiliza o **Entity Framework Core**. Siga os passos abaixo para criar e atualizar o banco de dados na sua máquina.

### Como Criar o Banco de Dados

1. Abra o projeto no **Visual Studio**.
2. No menu superior, acesse: 
   `Ferramentas` ➡️ `Gerenciador de Pacotes NuGet` ➡️ `Console do Gerenciador de Pacotes`.
3. No console que abrir (`PM>`), digite o comando abaixo e aperte **Enter**:
   
powershell
  `` Update-Database``

O Entity Framework criará automaticamente o banco `HackathonSegundaChamada` com todas as tabelas.

---

### Como Atualizar o Banco 

Se você modificar alguma classe/tabela no código, execute estes dois comandos em sequência no console para aplicar as mudanças:
powershell
``
Add-Migration NomeDaSuaAlteracao
Update-Database
``

---

### 🧹 Como Desfazer ou Resetar uma Migração Local

Caso precise apagar a última migração criada (que ainda não foi para o banco) para refazê-la com um novo nome ou corrigir algo, use:
powershell

``
Remove-Migration
Add-Migration InicializacaoBanco
Update-Database
``


## Como rodar

1. Certifique-se de ter o .NET 10 SDK e SQL Server LocalDB instalados.
2. Clone o repositório.
3. Execute as migrations a cima:
4. Rode o projeto:
   ```
   dotnet run
   ```
5. Acesse `https://localhost:{porta}` e faça login com um dos usuários acima.

