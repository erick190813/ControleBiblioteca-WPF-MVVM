# 📚 Sistema de Controle de Biblioteca - Integração de Sistemas

[cite_start]Este projeto foi desenvolvido como parte da Situação de Aprendizagem [cite: 1] [cite_start]do curso Técnico em Desenvolvimento de Sistemas do SENAI Nova Lima[cite: 27]. [cite_start]O objetivo principal é demonstrar a Integração de Sistemas com APIs e Persistência de Dados [cite: 6][cite_start], focando no cenário de **API de Biblioteca → Controle de Empréstimos**[cite: 11, 12].

## 🏗️ Arquitetura do Projeto

[cite_start]A solução foi dividida em três projetos independentes para garantir o isolamento de responsabilidades e simular uma integração real de mercado[cite: 7, 10]:

1. **ControleBiblioteca.Api (REST API):** O coração do sistema. [cite_start]Desenvolvida em C# (.NET 8) [cite: 8][cite_start], expõe endpoints (CRUD) para gerenciar Livros, Usuários e Empréstimos[cite: 8]. Contém toda a regra de negócio e gerencia a persistência no banco de dados SQLite via Entity Framework Core.
2. [cite_start]**ControleBiblioteca.Simulador (App Console):** Um serviço que roda em segundo plano consumindo a API em tempo real. Ele simula o tráfego de uma biblioteca, gerando requisições aleatórias de novos empréstimos e devoluções para popular o banco de dados.
3. [cite_start]**ControleBiblioteca.Wpf (Dashboard):** A interface gráfica do bibliotecário. [cite_start]Construída em WPF utilizando o padrão arquitetural MVVM  (via `CommunityToolkit.Mvvm`). Consome a API para exibir o status do estoque e gerenciar devoluções.

## 🚀 Tecnologias Utilizadas

* [cite_start]**Linguagem:** C# 12 / .NET 8.0 [cite: 8]
* **Persistência:** Entity Framework Core + SQLite
* [cite_start]**Padrões de Projeto:** Repository Pattern, DTOs, MVVM 
* [cite_start]**Interface:** Windows Presentation Foundation (WPF) 
* **Documentação de API:** Swagger / OpenAPI

## ⚙️ Como Executar o Projeto

Como o sistema é composto por múltiplos serviços integrados, é necessário rodar os projetos simultaneamente.

1. Clone o repositório:
   ```bash
   git clone [https://github.com/seu-usuario/seu-repositorio.git](https://github.com/seu-usuario/seu-repositorio.git)
