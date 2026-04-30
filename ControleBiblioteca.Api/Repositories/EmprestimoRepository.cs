using ControleBiblioteca.Api.Data;
using ControleBiblioteca.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleBiblioteca.Api.Repositories
{
    public class EmprestimoRepository : IEmprestimoRepository
    {
        private readonly BibliotecaDbContext _context;

        public EmprestimoRepository(BibliotecaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Emprestimo>> ObterTodosAsync()
        {
            return await _context.Emprestimos
                .Include(e => e.Livro)
                .Include(e => e.Usuario)
                .ToListAsync();
        }

        public async Task<Emprestimo?> ObterPorIdAsync(int id)
        {
            return await _context.Emprestimos
                .Include(e => e.Livro)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Emprestimo> AdicionarAsync(Emprestimo emprestimo)
        {
            _context.Emprestimos.Add(emprestimo);
            await _context.SaveChangesAsync();
            return emprestimo;
        }

        public async Task AtualizarAsync(Emprestimo emprestimo)
        {
            _context.Entry(emprestimo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(Emprestimo emprestimo)
        {
            _context.Emprestimos.Remove(emprestimo);
            await _context.SaveChangesAsync();
        }
    }
}