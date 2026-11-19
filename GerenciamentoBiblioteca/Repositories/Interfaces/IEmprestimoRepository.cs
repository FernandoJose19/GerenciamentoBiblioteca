
using GerenciamentoBiblioteca.Models;

namespace GerenciamentoBiblioteca.Repositories.Interfaces
{
    public interface IEmprestimoRepository
    {
        Task<List<Emprestimo>> ListarTodos();
        Task<Emprestimo?> BuscarPorId(int id);
        Task Adicionar(Emprestimo emprestimo);
        Task Atualizar(Emprestimo emprestimo);
        Task Excluir(int id);
    }
}
