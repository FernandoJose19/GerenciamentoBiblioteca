using GerenciamentoBiblioteca.Data;
using GerenciamentoBiblioteca.Models;
using GerenciamentoBiblioteca.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoBiblioteca.Repositories
{
    public class AutorRepository : IAutorRepository
    {
        private readonly ApplicationDbContext _context;

        public AutorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Autor>> ListarTodos()
        {
            return await _context.Autores.ToListAsync();
        }

        public async Task<Autor?> BuscarPorId(int id)
        {
            return await _context.Autores.FindAsync(id);
        }

        public async Task Adicionar(Autor autor)
        {
            _context.Autores.Add(autor);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(Autor autor)
        {
            _context.Autores.Update(autor);
            await _context.SaveChangesAsync();
        }

        public async Task Excluir(int id)
        {
            var autor = await BuscarPorId(id);

            if (autor != null)
            {
                _context.Autores.Remove(autor);
                await _context.SaveChangesAsync();
            }
        }
    }
}
