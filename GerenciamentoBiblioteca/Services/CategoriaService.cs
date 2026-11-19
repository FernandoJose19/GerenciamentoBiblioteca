
using GerenciamentoBiblioteca.Models;
using GerenciamentoBiblioteca.Repositories.Interfaces;
using GerenciamentoBiblioteca.Services.Interfaces;

namespace GerenciamentoBiblioteca.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;
        private readonly ILivroRepository _livroRepository;

        public CategoriaService(
            ICategoriaRepository repository,
            ILivroRepository livroRepository)
        {
            _repository = repository;
            _livroRepository = livroRepository;
        }

        public async Task<List<Categoria>> ListarTodos()
        {
            return await _repository.ListarTodos();
        }

        public async Task<Categoria?> BuscarPorId(int id)
        {
            return await _repository.BuscarPorId(id);
        }

        public async Task Adicionar(Categoria categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.Nome))
                throw new Exception("Informe o nome da categoria.");

            await _repository.Adicionar(categoria);
        }

        public async Task Atualizar(Categoria categoria)
        {
            var existente = await _repository.BuscarPorId(categoria.Id);

            if (existente == null)
                throw new Exception("Categoria não encontrada.");

            if (string.IsNullOrWhiteSpace(categoria.Nome))
                throw new Exception("Informe o nome da categoria.");

            existente.Nome = categoria.Nome;

            await _repository.Atualizar(existente);
        }

        public async Task Excluir(int id)
        {
            var categoria = await _repository.BuscarPorId(id);

            if (categoria == null)
                throw new Exception("Categoria não encontrada.");

            var livros = await _livroRepository.ListarTodos();

            if (livros.Any(l => l.CategoriaId == id))
                throw new Exception(
                    "Não é possível excluir uma categoria que possui livros cadastrados.");

            await _repository.Excluir(id);
        }
    }
}
