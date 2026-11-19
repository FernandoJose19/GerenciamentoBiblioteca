using GerenciamentoBiblioteca.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciamentoBiblioteca.Migrations;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20261112190000_AdicionarStatusAtivo")]
public partial class AdicionarStatusAtivo : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(name: "Ativo", table: "Livros", type: "bit", nullable: false, defaultValue: true);
        migrationBuilder.AddColumn<bool>(name: "Ativo", table: "Leitores", type: "bit", nullable: false, defaultValue: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "Ativo", table: "Livros");
        migrationBuilder.DropColumn(name: "Ativo", table: "Leitores");
    }
}
