
using GerenciamentoBiblioteca.Models;
using GerenciamentoBiblioteca.Repositories.Interfaces;
using GerenciamentoBiblioteca.Services.Interfaces;

namespace GerenciamentoBiblioteca.Services
{
    public class LivroService : ILivroService
    {
        private readonly ILivroRepository _repository;
        private readonly IEmprestimoRepository _emprestimoRepository;

        public LivroService(
            ILivroRepository repository,
            IEmprestimoRepository emprestimoRepository)
        {
            _repository = repository;
            _emprestimoRepository = emprestimoRepository;
        }

        public async Task<List<Livro>> ListarTodos()
        {
            return await _repository.ListarTodos();
        }

        public async Task<Livro?> BuscarPorId(int id)
        {
            return await _repository.BuscarPorId(id);
        }

        public async Task Adicionar(Livro livro)
        {
            if (livro.QuantidadeTotal <= 0)
                throw new Exception("Informe uma quantidade total maior que zero.");

            // No cadastro, todos os exemplares começam disponíveis.
            livro.QuantidadeDisponivel = livro.QuantidadeTotal;
            livro.Ativo = true;

            await _repository.Adicionar(livro);
        }

        public async Task Atualizar(Livro livro)
        {
            var existente = await _repository.BuscarPorId(livro.Id);

            if (existente == null)
                throw new Exception("Livro não encontrado.");

            var emprestimos = await _emprestimoRepository.ListarTodos();

            int emprestados = emprestimos.Count(e =>
                e.LivroId == livro.Id && e.DataDevolucao == null);

            if (livro.QuantidadeTotal < emprestados)
                throw new Exception(
                    "A quantidade total não pode ser menor que os exemplares emprestados.");

            if (livro.QuantidadeTotal <= 0)
                throw new Exception("A quantidade total deve ser maior que zero.");

            existente.Titulo = livro.Titulo;
            existente.AutorId = livro.AutorId;
            existente.CategoriaId = livro.CategoriaId;
            existente.QuantidadeTotal = livro.QuantidadeTotal;

            // Estoque disponível = total - empréstimos ativos
            existente.QuantidadeDisponivel =
                livro.QuantidadeTotal - emprestados;

            await _repository.Atualizar(existente);
        }

        public async Task Excluir(int id)
        {
            var livro = await _repository.BuscarPorId(id);
            if (livro == null)
                throw new Exception("Livro não encontrado.");

            // Exclusão definitiva: remove também os empréstimos associados,
            // pois o usuário solicitou que o botão Excluir não inative registros.
            var emprestimos = await _emprestimoRepository.ListarTodos();
            foreach (var emprestimo in emprestimos.Where(e => e.LivroId == id))
                await _emprestimoRepository.Excluir(emprestimo.Id);

            await _repository.Excluir(id);
        }

    }
}
