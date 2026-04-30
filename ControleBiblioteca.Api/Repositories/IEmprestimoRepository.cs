using ControleBiblioteca.Api.Models;

namespace ControleBiblioteca.Api.Repositories
{
    public interface IEmprestimoRepository
    {
        Task<IEnumerable<Emprestimo>> ObterTodosAsync();
        Task<Emprestimo?> ObterPorIdAsync(int id);
        Task<Emprestimo> AdicionarAsync(Emprestimo emprestimo);
        Task AtualizarAsync(Emprestimo emprestimo);
        Task DeletarAsync(Emprestimo emprestimo);
    }
}