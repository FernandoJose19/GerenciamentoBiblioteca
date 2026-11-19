using GerenciamentoBiblioteca.Models;

public interface ILivroRepository
{
    Task<List<Livro>> ListarTodos();

    Task<Livro?> BuscarPorId(int id);

    Task Adicionar(Livro livro);

    Task Atualizar(Livro livro);

    Task Excluir(int id);
}