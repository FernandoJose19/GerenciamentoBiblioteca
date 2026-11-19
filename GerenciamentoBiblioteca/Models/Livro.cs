
using System.ComponentModel.DataAnnotations;

namespace GerenciamentoBiblioteca.Models
{
    public class Livro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o título.")]
        [StringLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public int AutorId { get; set; }

        public Autor? Autor { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        public Categoria? Categoria { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "A quantidade total deve ser maior que zero.")]
        public int QuantidadeTotal { get; set; }

        [Range(0, int.MaxValue)]
        public int QuantidadeDisponivel { get; set; }

        public bool Ativo { get; set; } = true;

        // Relacionamento com empréstimos
        public ICollection<Emprestimo> Emprestimos { get; set; }
            = new List<Emprestimo>();
    }
}
