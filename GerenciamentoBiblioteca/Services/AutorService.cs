
using GerenciamentoBiblioteca.Models;
using GerenciamentoBiblioteca.Repositories.Interfaces;
using GerenciamentoBiblioteca.Services.Interfaces;

namespace GerenciamentoBiblioteca.Services
{
    public class AutorService : IAutorService
    {
        private readonly IAutorRepository _repository;
        private readonly ILivroRepository _livroRepository;

        public AutorService(
            IAutorRepository repository,
            ILivroRepository livroRepository)
        {
            _repository = repository;
            _livroRepository = livroRepository;
        }

        public async Task<List<Autor>> ListarTodos()
        {
            return await _repository.ListarTodos();
        }

        public async Task<Autor?> BuscarPorId(int id)
        {
            return await _repository.BuscarPorId(id);
        }

        public async Task Adicionar(Autor autor)
        {
            if (string.IsNullOrWhiteSpace(autor.Nome))
                throw new Exception("Informe o nome do autor.");

            await _repository.Adicionar(autor);
        }

        public async Task Atualizar(Autor autor)
        {
            var existente = await _repository.BuscarPorId(autor.Id);

            if (existente == null)
                throw new Exception("Autor não encontrado.");

            if (string.IsNullOrWhiteSpace(autor.Nome))
                throw new Exception("Informe o nome do autor.");

            existente.Nome = autor.Nome;

            await _repository.Atualizar(existente);
        }

        public async Task Excluir(int id)
        {
            var autor = await _repository.BuscarPorId(id);

            if (autor == null)
                throw new Exception("Autor não encontrado.");

            var livros = await _livroRepository.ListarTodos();

            if (livros.Any(l => l.AutorId == id))
                throw new Exception(
                    "Não é possível excluir um autor que possui livros cadastrados.");

            await _repository.Excluir(id);
        }
    }
}
