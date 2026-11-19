using GerenciamentoBiblioteca.Data;
using GerenciamentoBiblioteca.Models;
using GerenciamentoBiblioteca.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoBiblioteca.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Categoria>> ListarTodos()
        {
            return await _context.Categorias.ToListAsync();
        }

        public async Task<Categoria?> BuscarPorId(int id)
        {
            return await _context.Categorias.FindAsync(id);
        }

        public async Task Adicionar(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task Excluir(int id)
        {
            var categoria = await BuscarPorId(id);

            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
            }
        }
    }
}
