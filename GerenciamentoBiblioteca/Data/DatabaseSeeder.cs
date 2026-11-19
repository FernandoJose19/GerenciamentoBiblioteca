using Microsoft.EntityFrameworkCore;

namespace GerenciamentoBiblioteca.Data
{
    public static class DatabaseSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            // O banco deve estar criado e com as migrations aplicadas.
            var autoresIniciais = new[]
            {
                "Machado de Assis",
                "Clarice Lispector",
                "Monteiro Lobato",
                "José de Alencar",
                "Jorge Amado",
                "Carlos Drummond de Andrade",
                "Paulo Freire"
            };

            var categoriasIniciais = new[]
            {
                "Romance", "Conto", "Poesia", "Literatura Brasileira",
                "Literatura Estrangeira", "Infantil", "Juvenil", "Educação",
                "História", "Biografia", "Ciências", "Tecnologia", "Artigo"
            };
            var leitoresIniciais = new[]
            {
                new
                {
                    Nome = "Ana Silva",
                    Email = "ana.silva@exemplo.com",
                    Telefone = "(27) 99999-1111"
                },
                new
                {
                    Nome = "João Santos",
                    Email = "joao.santos@exemplo.com",
                    Telefone = "(27) 99999-2222"
                },
                new
                {
                    Nome = "Maria Oliveira",
                    Email = "maria.oliveira@exemplo.com",
                    Telefone = "(27) 99999-3333"
                },
                new
                {
                    Nome = "Pedro Souza",
                    Email = "pedro.souza@exemplo.com",
                    Telefone = "(27) 99999-4444"
                },
                new
                {
                    Nome = "Carla Mendes",
                    Email = "carla.mendes@exemplo.com",
                    Telefone = "(27) 99999-5555"
                }
            };

            var emailsExistentes = context.Leitores
                .AsNoTracking()
                .Select(l => l.Email)
                .ToList()
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var leitor in leitoresIniciais)
            {
                if (!emailsExistentes.Contains(leitor.Email))
                {
                    context.Leitores.Add(new Leitor
                    {
                        Nome = leitor.Nome,
                        Email = leitor.Email,
                        Telefone = leitor.Telefone,
                        Ativo = true
                    });
                }
            }

            context.SaveChanges();

            context.SaveChanges();

            var autoresExistentes = context.Autores
                .AsNoTracking().Select(a => a.Nome).ToList()
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var nome in autoresIniciais)
            {
                if (!autoresExistentes.Contains(nome))
                    context.Autores.Add(new Models.Autor { Nome = nome });
            }

            var categoriasExistentes = context.Categorias
                .AsNoTracking().Select(c => c.Nome).ToList()
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var nome in categoriasIniciais)
            {
                if (!categoriasExistentes.Contains(nome))
                    context.Categorias.Add(new global::Categoria { Nome = nome });
            }

            context.SaveChanges();
        }
    }
}
