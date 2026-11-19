using GerenciamentoBiblioteca.Models;
using System.ComponentModel.DataAnnotations;

public class Emprestimo
{
    public int Id { get; set; }

    [Required]
    public int LivroId { get; set; }

    public Livro? Livro { get; set; }

    [Required]
    public int LeitorId { get; set; }

    public Leitor? Leitor { get; set; }

    public DateTime DataEmprestimo { get; set; }

    public DateTime DataPrevistaDevolucao { get; set; }

    public DateTime? DataDevolucao { get; set; }
}