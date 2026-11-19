using GerenciamentoBiblioteca.Data;
using GerenciamentoBiblioteca.Models;
using GerenciamentoBiblioteca.Repositories.Interfaces;
using GerenciamentoBiblioteca.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoBiblioteca.Services;

public class EmprestimoService : IEmprestimoService
{
    private readonly IEmprestimoRepository _repository;
    private readonly ILivroRepository _livroRepository;
    private readonly ILeitorRepository _leitorRepository;
    private readonly ApplicationDbContext _context;

    public EmprestimoService(IEmprestimoRepository repository, ILivroRepository livroRepository,
        ILeitorRepository leitorRepository, ApplicationDbContext context)
    {
        _repository = repository;
        _livroRepository = livroRepository;
        _leitorRepository = leitorRepository;
        _context = context;
    }

    public Task<List<Emprestimo>> ListarTodos() => _repository.ListarTodos();
    public Task<Emprestimo?> BuscarPorId(int id) => _repository.BuscarPorId(id);

    public async Task RegistrarEmprestimo(int livroId, int leitorId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var livro = await _livroRepository.BuscarPorId(livroId) ?? throw new Exception("Livro não encontrado.");
            var leitor = await _leitorRepository.BuscarPorId(leitorId) ?? throw new Exception("Leitor não encontrado.");
            if (livro.QuantidadeDisponivel <= 0) throw new Exception("Não existem exemplares disponíveis para empréstimo.");

            livro.QuantidadeDisponivel--;
            await _livroRepository.Atualizar(livro);
            await _repository.Adicionar(new Emprestimo { LivroId = livroId, LeitorId = leitorId,
                DataEmprestimo = DateTime.Today, DataPrevistaDevolucao = DateTime.Today.AddDays(7) });
            await transaction.CommitAsync();
        }
        catch { await transaction.RollbackAsync(); throw; }
    }

    public async Task RegistrarDevolucao(int id)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var emprestimo = await _repository.BuscarPorId(id) ?? throw new Exception("Empréstimo não encontrado.");
            if (emprestimo.DataDevolucao != null) throw new Exception("Este empréstimo já foi devolvido.");
            var livro = await _livroRepository.BuscarPorId(emprestimo.LivroId) ?? throw new Exception("Livro não encontrado.");
            if (livro.QuantidadeDisponivel >= livro.QuantidadeTotal) throw new Exception("A disponibilidade já atingiu o total do acervo.");
            emprestimo.DataDevolucao = DateTime.Today;
            livro.QuantidadeDisponivel++;
            await _livroRepository.Atualizar(livro);
            await _repository.Atualizar(emprestimo);
            await transaction.CommitAsync();
        }
        catch { await transaction.RollbackAsync(); throw; }
    }

    public async Task Atualizar(Emprestimo dados)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var atual = await _repository.BuscarPorId(dados.Id) ?? throw new Exception("Empréstimo não encontrado.");
            if (dados.DataPrevistaDevolucao.Date < atual.DataEmprestimo.Date) throw new Exception("A previsão de devolução não pode ser anterior à data do empréstimo.");

            // Empréstimos devolvidos permitem apenas corrigir a previsão original;
            // livro, leitor e data efetiva de devolução permanecem imutáveis.
            if (atual.DataDevolucao.HasValue)
            {
                if (dados.LivroId != atual.LivroId || dados.LeitorId != atual.LeitorId)
                    throw new Exception("Empréstimos devolvidos mantêm o livro e o leitor do histórico.");
                atual.DataPrevistaDevolucao = dados.DataPrevistaDevolucao.Date;
                await _repository.Atualizar(atual);
                await transaction.CommitAsync();
                return;
            }

            var leitor = await _leitorRepository.BuscarPorId(dados.LeitorId) ?? throw new Exception("Leitor não encontrado.");

            if (dados.LivroId != atual.LivroId)
            {
                var livroNovo = await _livroRepository.BuscarPorId(dados.LivroId) ?? throw new Exception("Livro não encontrado.");
                if (livroNovo.QuantidadeDisponivel <= 0) throw new Exception("O novo livro não possui exemplares disponíveis.");
                var livroAnterior = await _livroRepository.BuscarPorId(atual.LivroId) ?? throw new Exception("Livro original não encontrado.");
                livroAnterior.QuantidadeDisponivel++;
                livroNovo.QuantidadeDisponivel--;
                await _livroRepository.Atualizar(livroAnterior);
                await _livroRepository.Atualizar(livroNovo);
                atual.LivroId = dados.LivroId;
            }

            atual.LeitorId = dados.LeitorId;
            atual.DataPrevistaDevolucao = dados.DataPrevistaDevolucao.Date;
            await _repository.Atualizar(atual);
            await transaction.CommitAsync();
        }
        catch { await transaction.RollbackAsync(); throw; }
    }

    public async Task Excluir(int id)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var emprestimo = await _repository.BuscarPorId(id) ?? throw new Exception("Empréstimo não encontrado.");
            if (!emprestimo.DataDevolucao.HasValue)
            {
                var livro = await _livroRepository.BuscarPorId(emprestimo.LivroId) ?? throw new Exception("Livro não encontrado.");
                if (livro.QuantidadeDisponivel >= livro.QuantidadeTotal) throw new Exception("Não foi possível ajustar o estoque com segurança.");
                livro.QuantidadeDisponivel++;
                await _livroRepository.Atualizar(livro);
            }
            await _repository.Excluir(id);
            await transaction.CommitAsync();
        }
        catch { await transaction.RollbackAsync(); throw; }
    }
}
