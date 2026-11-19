
using GerenciamentoBiblioteca.Data;
using GerenciamentoBiblioteca.Models;
using GerenciamentoBiblioteca.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoBiblioteca.Repositories
{
    public class LeitorRepository : ILeitorRepository
    {
        private readonly ApplicationDbContext _context;

        public LeitorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Leitor>> ListarTodos()
        {
            return await _context.Leitores.ToListAsync();
        }

        public async Task<Leitor?> BuscarPorId(int id)
        {
            return await _context.Leitores.FindAsync(id);
        }

        public async Task Adicionar(Leitor leitor)
        {
            _context.Leitores.Add(leitor);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(Leitor leitor)
        {
            _context.Leitores.Update(leitor);
            await _context.SaveChangesAsync();
        }

        public async Task Excluir(int id)
        {
            var leitor = await BuscarPorId(id);

            if (leitor != null)
            {
                _context.Leitores.Remove(leitor);
                await _context.SaveChangesAsync();
            }
        }
    }
}
