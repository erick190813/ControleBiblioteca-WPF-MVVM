namespace ControleBiblioteca.Api.DTOs
{
    public class EmprestimoResponseDTO
    {
        public int Id { get; set; }
        public string TituloLivro { get; set; } = string.Empty;
        public string NomeUsuario { get; set; } = string.Empty;
        public DateTime DataRetirada { get; set; }
        public DateTime? DataDevolucaoReal { get; set; }
        public decimal MultaAtraso { get; set; }
        public string StatusEmprestimo { get; set; } = string.Empty;
    }
}