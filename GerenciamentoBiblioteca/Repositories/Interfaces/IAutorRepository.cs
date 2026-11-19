using GerenciamentoBiblioteca.Models;

namespace GerenciamentoBiblioteca.Repositories.Interfaces
{
    public interface IAutorRepository
    {
        Task<List<Autor>> ListarTodos();
        Task<Autor?> BuscarPorId(int id);
        Task Adicionar(Autor autor);
        Task Atualizar(Autor autor);
        Task Excluir(int id);
    }
}