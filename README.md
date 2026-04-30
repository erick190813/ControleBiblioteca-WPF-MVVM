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

## 👨‍💻 Autoria e Créditos
Projeto acadêmico desenvolvido para a unidade curricular de Desenvolvimento de Sistemas.

* **Desenvolvedor:** Erick Silva Fernandes de Araujo
* **Instituição:** SENAI Nova Lima
* **Curso:** Técnico em Desenvolvimento de Sistemas Noite
* **Instrutor:** Frederico Aguiar
* **Data / Prazo:** 30 de Abril de 2026
