using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ControleBiblioteca.Api.Migrations
{
    /// <inheritdoc />
    public partial class CargaTemaTI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Livros",
                columns: new[] { "Id", "AnoPublicacao", "Autor", "Categoria", "Editora", "Isbn", "QuantidadeDisponivel", "QuantidadeTotal", "Titulo" },
                values: new object[,]
                {
                    { 1, 2024, "Microsoft Press", "Programação", "TechBooks", "978-1-01", 10, 10, "Arquitetura de Software com C# e MVVM" },
                    { 2, 2023, "OpenSource Ed", "Infraestrutura", "TechBooks", "978-1-02", 5, 5, "Guia Avançado de Linux e Administração de Servidores" },
                    { 3, 2025, "CyberSec", "Segurança da Informação", "Security Press", "978-1-03", 4, 4, "Ethical Hacking e Penetration Testing" },
                    { 4, 2025, "Science Press", "Tecnologias Emergentes", "Quantum Labs", "978-1-04", 3, 3, "Fundamentos de Computação Quântica e IA" },
                    { 5, 2024, "TechBooks", "Programação", "TechBooks", "978-1-05", 8, 8, "Desenvolvimento de APIs RESTful com .NET 8" },
                    { 6, 2009, "Robert C. Martin", "Engenharia de Software", "Alta Books", "978-1-06", 15, 15, "Clean Code: Habilidades Práticas do Agile Software" },
                    { 7, 2021, "Kurose & Ross", "Infraestrutura", "Pearson", "978-1-07", 6, 6, "Redes de Computadores e Internet" },
                    { 8, 2013, "Goodrich", "Programação", "Bookman", "978-1-08", 7, 7, "Estruturas de Dados e Algoritmos em Python" },
                    { 9, 2000, "GoF", "Arquitetura de Software", "Bookman", "978-1-09", 5, 5, "Design Patterns: Elementos de Software Orientado a Objetos" },
                    { 10, 2017, "Susan J. Fowler", "Arquitetura de Software", "Novatec", "978-1-10", 4, 4, "Microsserviços Prontos Para a Produção" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Cpf", "DataCadastro", "Email", "NomeCompleto", "StatusAtivo", "Telefone" },
                values: new object[,]
                {
                    { 1, "111.111.111-11", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "erick.araujo@aluno.senai.br", "Erick Silva Fernandes de Araujo", true, "31999999999" },
                    { 2, "222.222.222-22", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "frederico.instrutor@senai.br", "Frederico Aguiar", true, "31988888888" },
                    { 3, "333.333.333-33", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ada.lovelace@tech.com", "Ada Lovelace", true, "31977777777" },
                    { 4, "444.444.444-44", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "alan.turing@tech.com", "Alan Turing", true, "31966666666" },
                    { 5, "555.555.555-55", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "linus.t@tech.com", "Linus Torvalds", true, "31955555555" },
                    { 6, "666.666.666-66", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "grace.h@tech.com", "Grace Hopper", true, "31944444444" },
                    { 7, "777.777.777-77", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "margaret.h@tech.com", "Margaret Hamilton", true, "31933333333" },
                    { 8, "888.888.888-88", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "dennis.r@tech.com", "Dennis Ritchie", true, "31922222222" },
                    { 9, "999.999.999-99", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tim.bl@tech.com", "Tim Berners-Lee", true, "31911111111" },
                    { 10, "000.000.000-00", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "bjarne.s@tech.com", "Bjarne Stroustrup", true, "31900000000" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Livros",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Livros",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Livros",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Livros",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Livros",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Livros",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Livros",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Livros",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Livros",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Livros",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
