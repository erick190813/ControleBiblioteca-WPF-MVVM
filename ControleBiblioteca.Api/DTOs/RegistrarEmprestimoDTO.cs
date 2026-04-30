namespace ControleBiblioteca.Api.DTOs
{
    // Note que pedimos APENAS o necessário para iniciar um empréstimo.
    // Datas, status e multas serão calculados pelo sistema.
    public class RegistrarEmprestimoDTO
    {
        public int LivroId { get; set; }
        public int UsuarioId { get; set; }
    }
}