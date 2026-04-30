using ControleBiblioteca.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleBiblioteca.Api.Data
{
    public class BibliotecaDbContext : DbContext
    {
        public BibliotecaDbContext(DbContextOptions<BibliotecaDbContext> options) : base(options)
        {
        }

        // Estas propriedades representam as tabelas no banco de dados
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Carga inicial de Usuários (IDs de 1 a 10)
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, NomeCompleto = "Erick Silva Fernandes de Araujo", Cpf = "111.111.111-11", Email = "erick.araujo@aluno.senai.br", Telefone = "31999999999", DataCadastro = new DateTime(2026, 1, 1), StatusAtivo = true },
                new Usuario { Id = 2, NomeCompleto = "Frederico Aguiar", Cpf = "222.222.222-22", Email = "frederico.instrutor@senai.br", Telefone = "31988888888", DataCadastro = new DateTime(2026, 1, 1), StatusAtivo = true },
                new Usuario { Id = 3, NomeCompleto = "Ada Lovelace", Cpf = "333.333.333-33", Email = "ada.lovelace@tech.com", Telefone = "31977777777", DataCadastro = new DateTime(2026, 1, 1), StatusAtivo = true },
                new Usuario { Id = 4, NomeCompleto = "Alan Turing", Cpf = "444.444.444-44", Email = "alan.turing@tech.com", Telefone = "31966666666", DataCadastro = new DateTime(2026, 1, 1), StatusAtivo = true },
                new Usuario { Id = 5, NomeCompleto = "Linus Torvalds", Cpf = "555.555.555-55", Email = "linus.t@tech.com", Telefone = "31955555555", DataCadastro = new DateTime(2026, 1, 1), StatusAtivo = true },
                new Usuario { Id = 6, NomeCompleto = "Grace Hopper", Cpf = "666.666.666-66", Email = "grace.h@tech.com", Telefone = "31944444444", DataCadastro = new DateTime(2026, 1, 1), StatusAtivo = true },
                new Usuario { Id = 7, NomeCompleto = "Margaret Hamilton", Cpf = "777.777.777-77", Email = "margaret.h@tech.com", Telefone = "31933333333", DataCadastro = new DateTime(2026, 1, 1), StatusAtivo = true },
                new Usuario { Id = 8, NomeCompleto = "Dennis Ritchie", Cpf = "888.888.888-88", Email = "dennis.r@tech.com", Telefone = "31922222222", DataCadastro = new DateTime(2026, 1, 1), StatusAtivo = true },
                new Usuario { Id = 9, NomeCompleto = "Tim Berners-Lee", Cpf = "999.999.999-99", Email = "tim.bl@tech.com", Telefone = "31911111111", DataCadastro = new DateTime(2026, 1, 1), StatusAtivo = true },
                new Usuario { Id = 10, NomeCompleto = "Bjarne Stroustrup", Cpf = "000.000.000-00", Email = "bjarne.s@tech.com", Telefone = "31900000000", DataCadastro = new DateTime(2026, 1, 1), StatusAtivo = true }
            );

            // Carga inicial de Livros focados em TI (IDs de 1 a 10)
            modelBuilder.Entity<Livro>().HasData(
                new Livro { Id = 1, Titulo = "Arquitetura de Software com C# e MVVM", Autor = "Microsoft Press", Isbn = "978-1-01", Editora = "TechBooks", AnoPublicacao = 2024, QuantidadeTotal = 10, QuantidadeDisponivel = 10, Categoria = "Programação" },
                new Livro { Id = 2, Titulo = "Guia Avançado de Linux e Administração de Servidores", Autor = "OpenSource Ed", Isbn = "978-1-02", Editora = "TechBooks", AnoPublicacao = 2023, QuantidadeTotal = 5, QuantidadeDisponivel = 5, Categoria = "Infraestrutura" },
                new Livro { Id = 3, Titulo = "Ethical Hacking e Penetration Testing", Autor = "CyberSec", Isbn = "978-1-03", Editora = "Security Press", AnoPublicacao = 2025, QuantidadeTotal = 4, QuantidadeDisponivel = 4, Categoria = "Segurança da Informação" },
                new Livro { Id = 4, Titulo = "Fundamentos de Computação Quântica e IA", Autor = "Science Press", Isbn = "978-1-04", Editora = "Quantum Labs", AnoPublicacao = 2025, QuantidadeTotal = 3, QuantidadeDisponivel = 3, Categoria = "Tecnologias Emergentes" },
                new Livro { Id = 5, Titulo = "Desenvolvimento de APIs RESTful com .NET 8", Autor = "TechBooks", Isbn = "978-1-05", Editora = "TechBooks", AnoPublicacao = 2024, QuantidadeTotal = 8, QuantidadeDisponivel = 8, Categoria = "Programação" },
                new Livro { Id = 6, Titulo = "Clean Code: Habilidades Práticas do Agile Software", Autor = "Robert C. Martin", Isbn = "978-1-06", Editora = "Alta Books", AnoPublicacao = 2009, QuantidadeTotal = 15, QuantidadeDisponivel = 15, Categoria = "Engenharia de Software" },
                new Livro { Id = 7, Titulo = "Redes de Computadores e Internet", Autor = "Kurose & Ross", Isbn = "978-1-07", Editora = "Pearson", AnoPublicacao = 2021, QuantidadeTotal = 6, QuantidadeDisponivel = 6, Categoria = "Infraestrutura" },
                new Livro { Id = 8, Titulo = "Estruturas de Dados e Algoritmos em Python", Autor = "Goodrich", Isbn = "978-1-08", Editora = "Bookman", AnoPublicacao = 2013, QuantidadeTotal = 7, QuantidadeDisponivel = 7, Categoria = "Programação" },
                new Livro { Id = 9, Titulo = "Design Patterns: Elementos de Software Orientado a Objetos", Autor = "GoF", Isbn = "978-1-09", Editora = "Bookman", AnoPublicacao = 2000, QuantidadeTotal = 5, QuantidadeDisponivel = 5, Categoria = "Arquitetura de Software" },
                new Livro { Id = 10, Titulo = "Microsserviços Prontos Para a Produção", Autor = "Susan J. Fowler", Isbn = "978-1-10", Editora = "Novatec", AnoPublicacao = 2017, QuantidadeTotal = 4, QuantidadeDisponivel = 4, Categoria = "Arquitetura de Software" }
            );
        }
    }
}