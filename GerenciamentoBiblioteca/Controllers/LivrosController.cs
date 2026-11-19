
using GerenciamentoBiblioteca.Models;
using GerenciamentoBiblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GerenciamentoBiblioteca.Controllers
{
    public class LivrosController : Controller
    {
        private readonly ILivroService _livroService;
        private readonly IAutorService _autorService;
        private readonly ICategoriaService _categoriaService;

        public LivrosController(
            ILivroService livroService,
            IAutorService autorService,
            ICategoriaService categoriaService)
        {
            _livroService = livroService;
            _autorService = autorService;
            _categoriaService = categoriaService;
        }

        // Verifica se existem autor e categoria
        private async Task<bool> PodeCadastrarLivro()
        {
            var autores = await _autorService.ListarTodos();
            var categorias = await _categoriaService.ListarTodos();

            return autores.Any() && categorias.Any();
        }

        private async Task CarregarListas()
        {
            var autores = await _autorService.ListarTodos();
            var categorias = await _categoriaService.ListarTodos();

            ViewBag.Autores = new SelectList(autores, "Id", "Nome");
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nome");
        }

        public async Task<IActionResult> Index()
        {
            var livros = await _livroService.ListarTodos();
            return View(livros);
        }

        public async Task<IActionResult> Details(int id)
        {
            var livro = await _livroService.BuscarPorId(id);

            if (livro == null)
                return NotFound();

            return View(livro);
        }

        // GET: Livros/Create
        public async Task<IActionResult> Create()
        {
            if (!await PodeCadastrarLivro())
            {
                TempData["Aviso"] =
                    "Não é possível cadastrar livros. Cadastre pelo menos um autor e uma categoria primeiro.";

                return RedirectToAction("Index", "Home");
            }

            await CarregarListas();
            return View();
        }

        // POST: Livros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Livro livro)
        {
            // Verifica novamente antes de salvar
            if (!await PodeCadastrarLivro())
            {
                TempData["Aviso"] =
                    "Cadastro bloqueado! É necessário possuir pelo menos um autor e uma categoria.";

                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                await CarregarListas();
                return View(livro);
            }

            try
            {
                await _livroService.Adicionar(livro);

                TempData["Sucesso"] = "Livro cadastrado com sucesso!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            await CarregarListas();
            return View(livro);
        }

        // GET: Livros/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var livro = await _livroService.BuscarPorId(id);

            if (livro == null)
                return NotFound();

            await CarregarListas();
            return View(livro);
        }

        // POST: Livros/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Livro livro)
        {
            if (id != livro.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await CarregarListas();
                return View(livro);
            }

            try
            {
                await _livroService.Atualizar(livro);

                TempData["Sucesso"] = "Livro atualizado com sucesso!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            await CarregarListas();
            return View(livro);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var livro = await _livroService.BuscarPorId(id);

            if (livro == null)
                return NotFound();

            return View(livro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _livroService.Excluir(id);

                TempData["Sucesso"] = "Registro excluído definitivamente.";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
