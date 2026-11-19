
using GerenciamentoBiblioteca.Models;

namespace GerenciamentoBiblioteca.Services.Interfaces
{
    public interface IEmprestimoService
    {
        Task<List<Emprestimo>> ListarTodos();
        Task<Emprestimo?> BuscarPorId(int id);
        Task RegistrarEmprestimo(int livroId, int leitorId);
        Task RegistrarDevolucao(int id);
        Task Atualizar(Emprestimo emprestimo);
        Task Excluir(int id);
    }
}
