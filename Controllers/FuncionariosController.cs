using System.Linq;
using System.Threading.Tasks;
using Hierarquias.Data;
using Hierarquias.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hierarquias.Controllers
{
    public class FuncionariosController : Controller
    {
        private readonly HierarquiasDbContext _context;

        public FuncionariosController(HierarquiasDbContext context)
        {
            _context = context;
        }

        // GET: Funcionarios
        public async Task<IActionResult> Index()
        {
            // Verifica se há uma mensagem de exclusão na TempData
            if (TempData.ContainsKey("MensagemExclusao"))
            {
                ViewBag.MensagemExclusao = TempData["MensagemExclusao"];
            }

            var funcionarios = await _context.Funcionarios
                .Include(f => f.Cargo)
                .ToListAsync();

            return View(funcionarios);
        }

        // GET: Funcionarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionarios
                .Include(f => f.Cargo)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (funcionario == null)
            {
                return NotFound();
            }

            return View(funcionario);
        }

        // GET: Funcionarios/Create
        public IActionResult Create()
        {
            ViewBag.Cargos = new SelectList(_context.Cargos, "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Sobrenome,CargoId")] Funcionarios funcionario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(funcionario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Cargos = new SelectList(_context.Cargos, "Id", "Nome", funcionario.CargoId);
            return View(funcionario);
        }

        // GET: Funcionarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionarios.FindAsync(id);

            if (funcionario == null)
            {
                return NotFound();
            }

            ViewBag.Cargos = new SelectList(_context.Cargos, "Id", "Nome", funcionario.CargoId);
            return View(funcionario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Sobrenome,CargoId")] Funcionarios funcionario)
        {
            if (id != funcionario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(funcionario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuncionarioExists(funcionario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Cargos = new SelectList(_context.Cargos, "Id", "Nome", funcionario.CargoId);
            return View(funcionario);
        }


        // GET: Funcionarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionarios
                .Include(f => f.Cargo)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (funcionario == null)
            {
                return NotFound();
            }

            return View(funcionario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
            {
                return NotFound();
            }

            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();

            // Adiciona uma mensagem TempData para ser exibida na próxima solicitação
            TempData["MensagemExclusao"] = "Funcionário excluído com sucesso.";

            return RedirectToAction(nameof(Index));
        }


        private bool FuncionarioExists(int id)
        {
            return _context.Funcionarios.Any(e => e.Id == id);
        }
    }
}
