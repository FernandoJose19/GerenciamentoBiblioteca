using GerenciamentoBiblioteca.Models;
using GerenciamentoBiblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoBiblioteca.Controllers
{
    public class AutoresController : Controller
    {
        private readonly IAutorService _service;

        public AutoresController(IAutorService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.ListarTodos());
        }

        public async Task<IActionResult> Details(int id)
        {
            var autor = await _service.BuscarPorId(id);

            if (autor == null)
                return NotFound();

            return View(autor);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Autor autor)
        {
            if (!ModelState.IsValid)
                return View(autor);

            try
            {
                await _service.Adicionar(autor);
                TempData["Sucesso"] = "Autor cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(autor);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var autor = await _service.BuscarPorId(id);

            if (autor == null)
                return NotFound();

            return View(autor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Autor autor)
        {
            if (id != autor.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(autor);

            try
            {
                await _service.Atualizar(autor);
                TempData["Sucesso"] = "Autor atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(autor);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var autor = await _service.BuscarPorId(id);

            if (autor == null)
                return NotFound();

            return View(autor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _service.Excluir(id);
                TempData["Sucesso"] = "Autor excluído com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}