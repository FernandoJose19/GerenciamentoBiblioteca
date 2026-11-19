
using GerenciamentoBiblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GerenciamentoBiblioteca.Controllers
{
    public class EmprestimosController : Controller
    {
        private readonly IEmprestimoService _service;
        private readonly ILivroService _livroService;
        private readonly ILeitorService _leitorService;

        public EmprestimosController(
            IEmprestimoService service,
            ILivroService livroService,
            ILeitorService leitorService)
        {
            _service = service;
            _livroService = livroService;
            _leitorService = leitorService;
        }

        private async Task CarregarListas()
        {
            var livros = await _livroService.ListarTodos();
            var leitores = await _leitorService.ListarTodos();

            ViewBag.Livros = new SelectList(
                livros.Where(l => l.QuantidadeDisponivel > 0),
                "Id", "Titulo");

            ViewBag.Leitores = new SelectList(
                leitores,
                "Id", "Nome");
        }

        // Verifica os pré-requisitos para empréstimos
        private async Task<bool> VerificarPreRequisitos()
        {
            var livros = await _livroService.ListarTodos();
            var leitores = await _leitorService.ListarTodos();

            if (!livros.Any())
            {
                TempData["Aviso"] =
                    "Não é possível registrar empréstimos. " +
                    "Cadastre pelo menos um livro primeiro.";

                return false;
            }

            if (!leitores.Any())
            {
                TempData["Aviso"] =
                    "Não é possível registrar empréstimos. " +
                    "Cadastre pelo menos um leitor primeiro.";

                return false;
            }

            if (!livros.Any(l => l.QuantidadeDisponivel > 0))
            {
                TempData["Aviso"] =
                    "Não existem livros disponíveis para empréstimo.";

                return false;
            }

            return true;
        }

        public async Task<IActionResult> Index()
        {
            var emprestimos = await _service.ListarTodos();

            return View(emprestimos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var emprestimo = await _service.BuscarPorId(id);

            if (emprestimo == null)
                return NotFound();

            return View(emprestimo);
        }

        public async Task<IActionResult> Create()
        {
            if (!await VerificarPreRequisitos())
                return RedirectToAction("Index", "Home");

            await CarregarListas();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Emprestimo emprestimo)
        {
            if (emprestimo.LivroId <= 0 ||
                emprestimo.LeitorId <= 0)
            {
                ModelState.AddModelError("",
                    "Selecione um livro e um leitor.");
            }

            if (!ModelState.IsValid)
            {
                await CarregarListas();
                ViewBag.Livros = new SelectList(
                    (await _livroService.ListarTodos()).Where(l => (l.QuantidadeDisponivel > 0) || l.Id == emprestimo.LivroId),
                    "Id", "Titulo", emprestimo.LivroId);
                ViewBag.Leitores = new SelectList((await _leitorService.ListarTodos()).Where(l => l.Id == emprestimo.LeitorId), "Id", "Nome", emprestimo.LeitorId);
                return View(emprestimo);
            }

            try
            {
                await _service.RegistrarEmprestimo(
                    emprestimo.LivroId,
                    emprestimo.LeitorId
                );

                TempData["Sucesso"] =
                    "Empréstimo registrado com sucesso!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                await CarregarListas();

                return View(emprestimo);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Devolver(int id)
        {
            try
            {
                await _service.RegistrarDevolucao(id);

                TempData["Sucesso"] =
                    "Devolução registrada com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var emprestimo = await _service.BuscarPorId(id);
            if (emprestimo == null) return NotFound();
            await CarregarListas();
            ViewBag.Livros = new SelectList(
                (await _livroService.ListarTodos()).Where(l => (l.QuantidadeDisponivel > 0) || l.Id == emprestimo.LivroId),
                "Id", "Titulo", emprestimo.LivroId);
            ViewBag.Leitores = new SelectList(
                (await _leitorService.ListarTodos()).Where(l => l.Id == emprestimo.LeitorId),
                "Id", "Nome", emprestimo.LeitorId);
            return View(emprestimo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Emprestimo emprestimo)
        {
            if (id != emprestimo.Id) return NotFound();
            ModelState.Remove(nameof(Emprestimo.Livro));
            ModelState.Remove(nameof(Emprestimo.Leitor));
            ModelState.Remove(nameof(Emprestimo.DataEmprestimo));
            ModelState.Remove(nameof(Emprestimo.DataDevolucao));
            if (!ModelState.IsValid)
            {
                await CarregarListas();
                ViewBag.Livros = new SelectList(
                    (await _livroService.ListarTodos()).Where(l => (l.QuantidadeDisponivel > 0) || l.Id == emprestimo.LivroId),
                    "Id", "Titulo", emprestimo.LivroId);
                ViewBag.Leitores = new SelectList((await _leitorService.ListarTodos()).Where(l => l.Id == emprestimo.LeitorId), "Id", "Nome", emprestimo.LeitorId);
                return View(emprestimo);
            }
            try
            {
                await _service.Atualizar(emprestimo);
                TempData["Sucesso"] = "Empréstimo atualizado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CarregarListas();
                ViewBag.Livros = new SelectList(
                    (await _livroService.ListarTodos()).Where(l => (l.QuantidadeDisponivel > 0) || l.Id == emprestimo.LivroId),
                    "Id", "Titulo", emprestimo.LivroId);
                ViewBag.Leitores = new SelectList((await _leitorService.ListarTodos()).Where(l => l.Id == emprestimo.LeitorId), "Id", "Nome", emprestimo.LeitorId);
                return View(emprestimo);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var emprestimo = await _service.BuscarPorId(id);
            if (emprestimo == null) return NotFound();
            return View(emprestimo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _service.Excluir(id);

                TempData["Sucesso"] =
                    "Registro de empréstimo excluído. O estoque foi ajustado quando necessário.";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
