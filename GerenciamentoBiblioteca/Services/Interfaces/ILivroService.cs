using GerenciamentoBiblioteca.Models;

namespace GerenciamentoBiblioteca.Services.Interfaces
{
    public interface ILivroService
    {
        Task<List<Livro>> ListarTodos();
        Task<Livro?> BuscarPorId(int id);
        Task Adicionar(Livro livro);
        Task Atualizar(Livro livro);
        Task Excluir(int id);
    }
}