
using GerenciamentoBiblioteca.Models;

namespace GerenciamentoBiblioteca.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<List<Categoria>> ListarTodos();
        Task<Categoria?> BuscarPorId(int id);
        Task Adicionar(Categoria categoria);
        Task Atualizar(Categoria categoria);
        Task Excluir(int id);
    }
}
