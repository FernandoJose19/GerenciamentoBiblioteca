using GerenciamentoBiblioteca.Models;
using System.ComponentModel.DataAnnotations;

public class Leitor
{
    public int Id { get; set; }

    [Required]
    public string Nome { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Telefone { get; set; }

    public bool Ativo { get; set; } = true;

    public ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();
}