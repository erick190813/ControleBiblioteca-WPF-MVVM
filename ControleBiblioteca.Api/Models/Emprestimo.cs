namespace ControleBiblioteca.Api.Models
{
    public class Emprestimo
    {
        public int Id { get; set; }

        // Chaves Estrangeiras e Propriedades de Navegação
        public int LivroId { get; set; }
        public Livro? Livro { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        // Controle de Datas e Status
        public DateTime DataRetirada { get; set; } = DateTime.Now;
        public DateTime DataVencimento { get; set; }
        public DateTime? DataDevolucaoReal { get; set; }

        public decimal MultaAtraso { get; set; } = 0.0m;
        public string StatusEmprestimo { get; set; } = "Ativo"; // Pode ser: Ativo, Devolvido, Atrasado
        public string Observacoes { get; set; } = string.Empty;
    }
}