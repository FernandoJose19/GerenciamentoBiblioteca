
using GerenciamentoBiblioteca.Models;

namespace GerenciamentoBiblioteca.Services.Interfaces
{
    public interface ILeitorService
    {
        Task<List<Leitor>> ListarTodos();
        Task<Leitor?> BuscarPorId(int id);
        Task Adicionar(Leitor leitor);
        Task Atualizar(Leitor leitor);
        Task Excluir(int id);
    }
}
