
using GerenciamentoBiblioteca.Data;
using GerenciamentoBiblioteca.Repositories;
using GerenciamentoBiblioteca.Repositories.Interfaces;
using GerenciamentoBiblioteca.Services;
using GerenciamentoBiblioteca.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoBiblioteca
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // MVC
            builder.Services.AddControllersWithViews();

            // Conexão com SQL Server
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString(
                        "DefaultConnection"
                    )
                )
            );

            // Repositories
            
            builder.Services.AddScoped<ILivroRepository, LivroRepository>();
            builder.Services.AddScoped<IAutorRepository, AutorRepository>();
            builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            builder.Services.AddScoped<ILeitorRepository, LeitorRepository>();
            builder.Services.AddScoped<IEmprestimoRepository, EmprestimoRepository>();

            // Services
            builder.Services.AddScoped<ILivroService, LivroService>();
            builder.Services.AddScoped<IAutorService, AutorService>();
            builder.Services.AddScoped<ICategoriaService, CategoriaService>();
            builder.Services.AddScoped<ILeitorService, LeitorService>();
            builder.Services.AddScoped<IEmprestimoService, EmprestimoService>();
            

            var app = builder.Build();

            // Insere autores e categorias iniciais sem duplicar registros existentes.
            // O banco precisa estar criado e com as migrations aplicadas.
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                DatabaseSeeder.Seed(dbContext);
            }

            // Configuração do ambiente
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // Rota padrão
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            app.Run();
        }
    }
}
