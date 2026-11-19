
using GerenciamentoBiblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoBiblioteca.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tabelas do banco de dados

        public DbSet<Livro> Livros { get; set; }

        public DbSet<Autor> Autores { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Leitor> Leitores { get; set; }

        public DbSet<Emprestimo> Emprestimos { get; set; }

        // Configuração dos relacionamentos

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Autor possui vários livros
            modelBuilder.Entity<Livro>()
                .HasOne(l => l.Autor)
                .WithMany(a => a.Livros)
                .HasForeignKey(l => l.AutorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Categoria possui vários livros
            modelBuilder.Entity<Livro>()
                .HasOne(l => l.Categoria)
                .WithMany(c => c.Livros)
                .HasForeignKey(l => l.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Livro possui vários empréstimos
            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.Livro)
                .WithMany(l => l.Emprestimos)
                .HasForeignKey(e => e.LivroId)
                .OnDelete(DeleteBehavior.Restrict);

            // Leitor possui vários empréstimos
            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.Leitor)
                .WithMany(l => l.Emprestimos)
                .HasForeignKey(e => e.LeitorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
