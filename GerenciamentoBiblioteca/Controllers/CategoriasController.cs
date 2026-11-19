
using GerenciamentoBiblioteca.Models;
using GerenciamentoBiblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoBiblioteca.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ICategoriaService _service;

        public CategoriasController(ICategoriaService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.ListarTodos());
        }

        public async Task<IActionResult> Details(int id)
        {
            var categoria = await _service.BuscarPorId(id);

            if (categoria == null)
                return NotFound();

            return View(categoria);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            if (!ModelState.IsValid)
                return View(categoria);

            try
            {
                await _service.Adicionar(categoria);
                TempData["Sucesso"] = "Categoria cadastrada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(categoria);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var categoria = await _service.BuscarPorId(id);

            if (categoria == null)
                return NotFound();

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Categoria categoria)
        {
            if (id != categoria.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(categoria);

            try
            {
                await _service.Atualizar(categoria);
                TempData["Sucesso"] = "Categoria atualizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(categoria);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var categoria = await _service.BuscarPorId(id);

            if (categoria == null)
                return NotFound();

            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _service.Excluir(id);
                TempData["Sucesso"] = "Categoria excluída com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
