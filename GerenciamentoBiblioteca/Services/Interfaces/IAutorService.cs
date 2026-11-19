
using GerenciamentoBiblioteca.Models;

namespace GerenciamentoBiblioteca.Services.Interfaces
{
    public interface IAutorService
    {
        Task<List<Autor>> ListarTodos();
        Task<Autor?> BuscarPorId(int id);
        Task Adicionar(Autor autor);
        Task Atualizar(Autor autor);
        Task Excluir(int id);
    }
}
