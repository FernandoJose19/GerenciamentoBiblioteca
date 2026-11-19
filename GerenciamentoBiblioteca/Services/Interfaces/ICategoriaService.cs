
using GerenciamentoBiblioteca.Models;

namespace GerenciamentoBiblioteca.Services.Interfaces
{
    public interface ICategoriaService
    {
        Task<List<Categoria>> ListarTodos();
        Task<Categoria?> BuscarPorId(int id);
        Task Adicionar(Categoria categoria);
        Task Atualizar(Categoria categoria);
        Task Excluir(int id);
    }
}
