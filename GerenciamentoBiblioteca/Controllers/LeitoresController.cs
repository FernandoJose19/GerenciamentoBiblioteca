
using GerenciamentoBiblioteca.Models;
using GerenciamentoBiblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoBiblioteca.Controllers
{
    public class LeitoresController : Controller
    {
        private readonly ILeitorService _service;

        public LeitoresController(ILeitorService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.ListarTodos());
        }

        public async Task<IActionResult> Details(int id)
        {
            var leitor = await _service.BuscarPorId(id);

            if (leitor == null)
                return NotFound();

            return View(leitor);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Leitor leitor)
        {
            if (!ModelState.IsValid)
                return View(leitor);

            try
            {
                await _service.Adicionar(leitor);

                TempData["Sucesso"] =
                    "Leitor cadastrado com sucesso!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(leitor);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var leitor = await _service.BuscarPorId(id);

            if (leitor == null)
                return NotFound();

            return View(leitor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Leitor leitor)
        {
            if (id != leitor.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(leitor);

            try
            {
                await _service.Atualizar(leitor);

                TempData["Sucesso"] =
                    "Leitor atualizado com sucesso!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(leitor);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var leitor = await _service.BuscarPorId(id);

            if (leitor == null)
                return NotFound();

            return View(leitor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _service.Excluir(id);

                TempData["Sucesso"] =
                    "Registro excluído definitivamente.";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
