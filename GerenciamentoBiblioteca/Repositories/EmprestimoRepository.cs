using GerenciamentoBiblioteca.Data;
using GerenciamentoBiblioteca.Models;
using GerenciamentoBiblioteca.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoBiblioteca.Repositories
{
    public class EmprestimoRepository : IEmprestimoRepository
    {
        private readonly ApplicationDbContext _context;

        public EmprestimoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Emprestimo>> ListarTodos()
        {
            return await _context.Emprestimos
                .Include(e => e.Livro)
                .Include(e => e.Leitor)
                .ToListAsync();
        }

        public async Task<Emprestimo?> BuscarPorId(int id)
        {
            return await _context.Emprestimos
                .Include(e => e.Livro)
                .Include(e => e.Leitor)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Adicionar(Emprestimo emprestimo)
        {
            _context.Emprestimos.Add(emprestimo);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(Emprestimo emprestimo)
        {
            _context.Emprestimos.Update(emprestimo);
            await _context.SaveChangesAsync();
        }

        public async Task Excluir(int id)
        {
            var emprestimo = await BuscarPorId(id);

            if (emprestimo != null)
            {
                _context.Emprestimos.Remove(emprestimo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
