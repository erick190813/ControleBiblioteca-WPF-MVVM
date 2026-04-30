namespace ControleBiblioteca.Api.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public bool StatusAtivo { get; set; } = true;
    }
}