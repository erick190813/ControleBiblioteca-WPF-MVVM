# 📚 Sistema de Controle de Biblioteca - Integração de Sistemas

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](#)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=c-sharp&logoColor=white)](#)
[![SQLite](https://img.shields.io/badge/SQLite-07405E?logo=sqlite&logoColor=white)](#)
[![WPF](https://img.shields.io/badge/WPF-MVVM-blue)](#)

Este projeto foi desenvolvido como parte da Situação de Aprendizagem de Integração de Sistemas do curso Técnico em Desenvolvimento de Sistemas. O objetivo principal é solucionar desafios comuns de comunicação entre sistemas distintos, garantindo consistência de dados e eficiência operacional. 

O escopo escolhido para demonstrar essa integração foi **API de Biblioteca → Controle de Empréstimos**.

---

## 🏗️ Arquitetura da Solução

Para simular um ambiente corporativo real e garantir escalabilidade, a solução foi dividida em três frentes assíncronas que se comunicam via rede:

### 1. `ControleBiblioteca.Api` (Back-end / REST API)
O núcleo do sistema. Desenvolvida em ASP.NET Core Web API, concentra todas as regras de negócios e segurança.
* **Repository Pattern:** Abstrai a camada de acesso a dados, permitindo manutenções facilitadas e injeção de dependência via `IServiceCollection`.
* **Data Transfer Objects (DTOs):** Protege a API contra ataques de *Over-Posting*, separando o modelo de banco de dados do modelo de tráfego HTTP.
* **Transações Seguras:** Utiliza `BeginTransactionAsync` para garantir que o controle de estoque dos livros e a geração do empréstimo ocorram de forma atômica (tudo ou nada).
* **Tratamento de Concorrência:** Implementa proteções para evitar que dois serviços alterem o mesmo registro simultaneamente.

### 2. `ControleBiblioteca.Simulador` (Worker / App Console)
Um serviço *headless* (sem interface) operando em segundo plano. Seu objetivo é estressar a API gerando informações em tempo real.
* Utiliza a classe `HttpClient` para realizar requisições assíncronas.
* Implementa um algoritmo estocástico: sorteia a probabilidade de um evento (60% de chance para novos empréstimos, 40% para devoluções) e aguarda um tempo aleatório entre 3s e 8s para simular o comportamento de usuários humanos operando totens de autoatendimento.

### 3. `ControleBiblioteca.Wpf` (Front-end / Dashboard)
A interface administrativa voltada para o bibliotecário, com foco na eficiência do usuário.
* **Arquitetura MVVM:** Utiliza o pacote `CommunityToolkit.Mvvm` para desacoplar totalmente a interface visual (Views em XAML) das lógicas de apresentação e chamadas de API (ViewModels).
* **Consumo de API REST:** Mapeia as respostas HTTP assíncronas diretamente para coleções observáveis (`ObservableCollection`), garantindo que a tabela seja renderizada sem travar a interface gráfica.

---

## 🗄️ Modelagem de Dados e Persistência

A persistência de dados foi implementada utilizando o **Entity Framework Core (EF Core)** integrado ao **SQLite**, garantindo portabilidade. O schema é composto por três entidades relacionais:

* **Livros:** Controla metadados (Título, ISBN, Categoria) e gerencia quantitativamente o estoque (Total e Disponível).
* **Usuários:** Mantém os dados dos clientes da biblioteca.
* **Empréstimos (Tabela Transacional):** Faz a junção (Foreign Keys) entre Livro e Usuário. É responsável por mapear o ciclo de vida da transação:
  * Gerencia `DataRetirada`, `DataVencimento` e `DataDevolucaoReal`.
  * Calcula automaticamente `MultaAtraso` baseada em dias excedentes no momento do HTTP PUT.
  * Gerencia o `StatusEmprestimo` (Ativo, Devolvido).

> **Data Seeding:** O banco de dados foi configurado com uma carga inicial de 10 usuários fictícios e 10 livros clássicos focados na área de Tecnologia da Informação (ex: *Clean Code*, *Design Patterns*), permitindo que o sistema seja testado imediatamente após a compilação.

---

## 🔌 Documentação dos Endpoints (Swagger)

A API possui documentação interativa gerada a partir de comentários XML (`<remarks>`), acessível via Swagger.

| Método | Endpoint | Descrição |
| :--- | :--- | :--- |
| `GET` | `/api/Emprestimos` | Retorna o catálogo completo de empréstimos, populando os nomes vinculados. |
| `GET` | `/api/Emprestimos/{id}` | Busca os detalhes de uma transação específica via ID. |
| `POST` | `/api/Emprestimos` | Cria um novo empréstimo. Deduz 1 unidade do inventário do livro associado. |
| `PUT` | `/api/Emprestimos/{id}/devolver` | Processa a devolução. Restaura o estoque do livro e calcula multas de atraso. |
| `DELETE` | `/api/Emprestimos/{id}` | Realiza a exclusão física (Hard Delete). Se ativo, retorna o livro ao estoque. |

---

## 🚀 Como Executar o Projeto Localmente

Para testar a integração sistêmica em sua totalidade, é imperativo que os projetos executem de forma simultânea.

### Pré-requisitos
* **Visual Studio 2022** (com suporte a ASP.NET e desenvolvimento Desktop).
* **SDK do .NET 8.0**.

### Passo a Passo
1. Clone este repositório para o seu ambiente local:
   ```bash
   git clone [https://github.com/seu-usuario/ControleBiblioteca-DotNet.git](https://github.com/seu-usuario/ControleBiblioteca-DotNet.git)
2. Abra a solução `ControleBiblioteca.Sistema.sln` no **Visual Studio 2022**.
3. No painel **Gerenciador de Soluções**, clique com o botão direito sobre a Solução (primeiro item da lista) e selecione **Propriedades**.
4. Acesse a aba **Projeto de Inicialização** e selecione a opção **Vários projetos de inicialização**.
5. Na lista apresentada, altere a coluna "Ação" para **Iniciar** nos três projetos (`Api`, `Simulador` e `Wpf`). Clique em **Aplicar** e **OK**.
6. Pressione `F5` para compilar e iniciar.
   
> **Nota:** Na primeira execução, o EF Core aplicará as migrações e criará o arquivo `biblioteca.db` automaticamente.

**Validação Visual:** Uma aba do navegador abrirá o Swagger da API, uma janela preta exibirá o log do Simulador operando, e a janela WPF mostrará o Dashboard interativo aguardando sua interação.

---

## 🔮 Melhorias Futuras (Roadmap)
Visando a evolução contínua da arquitetura para cenários de altíssima escala:

* Implementação do **AutoMapper** para o bind automático entre Models e DTOs.
* Inclusão do **Serilog** para geração de arquivos de log estruturados.
* Criação de testes unitários com **xUnit** e **Moq** para validar as regras do `EmprestimoRepository`.
* Implementação de mensageria assíncrona (ex: **RabbitMQ**) para desacoplar o Simulador da API, processando empréstimos em fila.

---

## 🧪 Guia de Testes, Validação e Decisões Arquiteturais

A qualidade desta solução é garantida através de testes isolados por responsabilidade. Cada camada do sistema (API, Interface e Worker) foi testada com objetivos arquiteturais específicos, provando a resiliência das regras de negócio e a estabilidade da integração.

### 1. Validação da API REST (Verbos HTTP via Swagger)
A API é o "coração" do sistema e a única porta de entrada para o banco de dados. Os testes manuais via Swagger servem para isolar o Back-end do Front-end, garantindo que as regras de proteção funcionem independente de quem faz a requisição.

* **GET (Listagem e Busca):** 
  * **O que testar:** Acessar `/api/Emprestimos` (todos) e `/api/Emprestimos/{id}` (específico).
  * **Por que testar assim:** Valida a capacidade de Leitura (Read) e o mapeamento correto do Entity Framework (Join entre Livros, Usuários e Empréstimos). Prova que a API consegue serializar os dados do banco para o formato JSON corretamente e retornar códigos HTTP semânticos (ex: `200 OK` se achar, `404 Not Found` se o ID não existir).
* **POST (Criação de Empréstimo):**
  * **O que testar:** Enviar requisições para criar empréstimos válidos e inválidos (ex: alugar um livro com estoque zerado).
  * **Por que testar assim:** O POST não apenas insere dados, ele processa lógica de negócio. Testá-lo valida a **Atomicidade** da transação: a API só pode registrar o empréstimo se, e somente se, houver sucesso na dedução do estoque do livro. Retornar um erro `400 Bad Request` ao tentar burlar o estoque prova que o sistema é seguro e inviolável.
* **PUT (Devolução e Atualização):**
  * **O que testar:** Executar a rota `/api/Emprestimos/{id}/devolver` em empréstimos dentro do prazo e em atraso.
  * **Por que testar assim:** O PUT, por padrão REST, deve garantir atualizações consistentes. Testar esta rota valida duas lógicas cruciais em uma única requisição: a devolução do livro para a prateleira (soma no estoque) e o cálculo dinâmico da `MultaAtraso` (comparando a data de vencimento com o momento exato do *request* HTTP).
* **DELETE (Exclusão de Registros):**
  * **O que testar:** Apagar um registro que esteja com o status "Ativo".
  * **Por que testar assim:** Testar a exclusão garante que o banco de dados não sofra com lixo relacional. Além disso, valida a regra de compensação: se um administrador deleta um empréstimo ativo por engano, a API deve ser inteligente o suficiente para devolver a unidade daquele livro ao estoque geral antes de destruir o registro, evitando o sumiço de inventário.

---

### 2. Validação da Interface Gráfica (WPF e MVVM)
O Front-end desktop existe para consumir a API de forma amigável ao usuário final (o bibliotecário). Os testes aqui não focam no banco de dados, mas sim na Experiência do Usuário (UX) e no comportamento da memória.

* **O que testar:** Realizar buscas de atualização da tabela, cliques rápidos repetidos nos botões de Devolução e observação do comportamento da tela.
* **Por que testar assim:** 
  1. **Validação do Assincronismo (`async/await`):** Ao clicar em "Atualizar", a tela (UI Thread) não pode congelar enquanto aguarda a resposta da rede. Se a tela continuar clicável, o assincronismo foi bem implementado.
  2. **Validação do MVVM (Model-View-ViewModel):** Prova que a interface (`.xaml`) está 100% dependente do `ObservableCollection` e da ViewModel. Quando a API responde com os dados atualizados, a tabela reflete instantaneamente os novos valores e multas, comprovando que o *Data Binding* bidirecional está funcionando sem a necessidade de lógicas de tela pesadas (Code-Behind).

---

### 3. Teste de Estresse e Concorrência (Simulador / Console Worker)
O simulador é um "robô" sem interface gráfica (*headless*), criado para injetar caos controlado e estressar o sistema operacionalmente.

* **O que testar:** Iniciar o Console e deixá-lo rodando de forma ininterrupta, disparando dezenas de `POSTs` e `PUTs` baseados em probabilidades matemáticas, competindo com as ações manuais feitas no painel WPF.
* **Por que testar assim:** 
  1. **Prevenção de Race Conditions (Condições de Corrida):** Simula múltiplos totens de biblioteca funcionando ao mesmo tempo. Garante que se duas requisições tentarem alugar a última cópia de um livro no exato mesmo milissegundo, o Entity Framework gerenciará o travamento (*Lock*) do banco de dados, entregando a cópia para a primeira requisição e bloqueando a segunda com um erro de estoque.
  2. **Resiliência da Rede:** Prova que a classe `HttpClient` foi bem implementada e está reaproveitando conexões corretamente (sem causar *Socket Exhaustion*), mantendo a API de pé e respondendo rapidamente sob volume constante de dados.

### 5. Teste de Regras de Tempo e Lógica Financeira (Cálculo de Multas)
Como o sistema define automaticamente um prazo de 7 dias para a devolução, todos os novos empréstimos criados estarão no prazo e retornarão multa de `R$ 0,00`. Para validar o algoritmo matemático de cálculo de atraso (R$ 2,00 por dia), é necessário realizar uma "viagem no tempo" manipulando o banco de dados.

**Passo a passo para testar (Time Travel Simulation):**
1. **Acesso ao Banco:** Abra o arquivo `biblioteca.db` utilizando um gerenciador de banco de dados (como *DB Browser for SQLite*, *SQLiteStudio* ou uma extensão do *VS Code*).
2. **Manipulação de Estado:** Acesse a tabela `Emprestimos` e localize um registro cujo status seja `"Ativo"`.
3. **Alteração da Data:** Modifique o valor da coluna `DataVencimento` desse registro para uma data no passado (por exemplo, voltando 10 dias atrás). Salve (Commit) a alteração no banco de dados.
4. **Execução do Teste:** Volte ao painel **WPF** (clique em Atualizar Dados) ou ao **Swagger**. Execute a devolução (`PUT /api/Emprestimos/{id}/devolver`) deste registro específico.
5. **Por que testar assim:** O cálculo da multa é dinâmico e ocorre estritamente no momento do `PUT`. Ao forçar uma data de vencimento no passado direto no banco, provamos que a API é a única fonte da verdade. Ela pega a data exata da requisição (`DataDevolucaoReal`), compara com a `DataVencimento` fraudada por nós, e processa perfeitamente a matemática da diferença de dias multiplicada pela taxa de atraso antes de salvar o status final.

---

## 👨‍💻 Autoria e Créditos
Projeto acadêmico desenvolvido para a unidade curricular de Desenvolvimento de Sistemas.

* **Desenvolvedor:** Erick Silva Fernandes de Araujo
* **Instituição:** SENAI Nova Lima
* **Curso:** Técnico em Desenvolvimento de Sistemas Noite
* **Instrutor:** Frederico Aguiar
* **Data / Prazo:** 30 de Abril de 2026
