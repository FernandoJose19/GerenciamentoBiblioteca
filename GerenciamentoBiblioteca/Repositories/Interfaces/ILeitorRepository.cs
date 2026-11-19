
using GerenciamentoBiblioteca.Models;

namespace GerenciamentoBiblioteca.Repositories.Interfaces
{
    public interface ILeitorRepository
    {
        Task<List<Leitor>> ListarTodos();
        Task<Leitor?> BuscarPorId(int id);
        Task Adicionar(Leitor leitor);
        Task Atualizar(Leitor leitor);
        Task Excluir(int id);
    }
}
