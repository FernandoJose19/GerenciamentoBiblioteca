
using GerenciamentoBiblioteca.Models;
using GerenciamentoBiblioteca.Repositories.Interfaces;
using GerenciamentoBiblioteca.Services.Interfaces;

namespace GerenciamentoBiblioteca.Services
{
    public class LeitorService : ILeitorService
    {
        private readonly ILeitorRepository _repository;
        private readonly IEmprestimoRepository _emprestimoRepository;

        public LeitorService(
            ILeitorRepository repository,
            IEmprestimoRepository emprestimoRepository)
        {
            _repository = repository;
            _emprestimoRepository = emprestimoRepository;
        }

        public async Task<List<Leitor>> ListarTodos()
        {
            return await _repository.ListarTodos();
        }

        public async Task<Leitor?> BuscarPorId(int id)
        {
            return await _repository.BuscarPorId(id);
        }

        public async Task Adicionar(Leitor leitor)
        {
            if (string.IsNullOrWhiteSpace(leitor.Nome))
                throw new Exception("Informe o nome do leitor.");

            if (string.IsNullOrWhiteSpace(leitor.Email))
                throw new Exception("Informe o e-mail do leitor.");

            leitor.Ativo = true;
            await _repository.Adicionar(leitor);
        }

        public async Task Atualizar(Leitor leitor)
        {
            var existente = await _repository.BuscarPorId(leitor.Id);

            if (existente == null)
                throw new Exception("Leitor não encontrado.");

            if (string.IsNullOrWhiteSpace(leitor.Nome) ||
                string.IsNullOrWhiteSpace(leitor.Email))
                throw new Exception("Preencha os dados obrigatórios.");

            existente.Nome = leitor.Nome;
            existente.Email = leitor.Email;
            existente.Telefone = leitor.Telefone;

            await _repository.Atualizar(existente);
        }

        public async Task Excluir(int id)
        {
            var leitor = await _repository.BuscarPorId(id);
            if (leitor == null)
                throw new Exception("Leitor não encontrado.");

            // Ao apagar o leitor, remove seus empréstimos. Empréstimos ativos
            // devolvem a disponibilidade ao livro antes da exclusão do histórico.
            var emprestimos = await _emprestimoRepository.ListarTodos();
            foreach (var emprestimo in emprestimos.Where(e => e.LeitorId == id))
            {
                if (emprestimo.DataDevolucao == null)
                {
                    var livro = emprestimo.Livro;
                    if (livro != null && livro.QuantidadeDisponivel < livro.QuantidadeTotal)
                    {
                        livro.QuantidadeDisponivel++;
                        // Atualização do livro pelo contexto do empréstimo ocorre
                        // no repositório ao salvar o empréstimo removido? Atualiza via navegação.
                        await _emprestimoRepository.Atualizar(emprestimo);
                    }
                }
                await _emprestimoRepository.Excluir(emprestimo.Id);
            }

            await _repository.Excluir(id);
        }

    }
}
