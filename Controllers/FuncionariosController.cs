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

        public async Task<IActionResult> Index()
        {
            if (TempData.ContainsKey("MensagemExclusao"))
            {
                ViewBag.MensagemExclusao = TempData["MensagemExclusao"];
            }

            var funcionarios = await _context.Funcionarios
                .ToListAsync();

            return View(funcionarios);
        }
        public IActionResult Hierarquia(int id)
        {
            var funcionario = _context.Funcionarios
                .Include(f => f.Subordinados)
                .Include(f => f.Superior)
                .FirstOrDefault(f => f.Id == id);

            if (funcionario == null)
            {
                return NotFound();
            }

            return View(funcionario);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = _context.Funcionarios
                .Include(f => f.Subordinados)
                .Include(f => f.Superior)
                .FirstOrDefault(f => f.Id == id);

            if (funcionario == null)
            {
                return NotFound();
            }

            return View(funcionario);
        }

        public IActionResult Create()
        {
            ViewBag.Cargos = new SelectList(_context.Cargos, "Id", "Nome");

            // Obtenha todos os funcionários disponíveis, incluindo o caso de nenhum superior selecionado
            var todosOsFuncionarios = _context.Funcionarios.ToList();
            todosOsFuncionarios.Insert(0, new Funcionarios { Id = 0, Nome = "Nenhum superior" });

            ViewBag.TodosOsFuncionarios = new SelectList(todosOsFuncionarios, "Id", "Nome");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Sobrenome,CargoId,SuperiorId")] Funcionarios funcionario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(funcionario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Cargos = new SelectList(_context.Cargos, "Id", "Nome", funcionario.Cargo);

            // Obtenha todos os funcionários disponíveis, incluindo o caso de nenhum superior selecionado
            var todosOsFuncionarios = _context.Funcionarios.ToList();
            todosOsFuncionarios.Insert(0, new Funcionarios { Id = 0, Nome = "Nenhum superior" });

            ViewBag.TodosOsFuncionarios = new SelectList(todosOsFuncionarios, "Id", "Nome", funcionario.SuperiorId);

            return View(funcionario);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionarios
                .Include(f => f.Subordinados)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (funcionario == null)
            {
                return NotFound();
            }

            // Carregue todos os funcionários disponíveis para seleção
            var todosOsFuncionarios = await _context.Funcionarios.ToListAsync();

            ViewBag.TodosOsFuncionarios = todosOsFuncionarios;

            return View(funcionario);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Funcionarios funcionario)
        {
            if (ModelState.IsValid)
            {
                // Obtenha o funcionário original do banco de dados
                var funcionarioOriginal = await _context.Funcionarios
                    .Include(f => f.Subordinados)
                    .FirstOrDefaultAsync(f => f.Id == funcionario.Id);

                if (funcionarioOriginal == null)
                {
                    return NotFound();
                }

                // Atualize outras propriedades...
                funcionarioOriginal.Nome = funcionario.Nome;

                // Atualize o superior
                funcionarioOriginal.SuperiorId = funcionario.SuperiorId;

                // Atualize a lista de subordinados
                funcionarioOriginal.Subordinados.Clear();
                foreach (var subordinadoId in funcionario.Subordinados)
                {
                    var subordinado = await _context.Funcionarios.FindAsync(subordinadoId);
                    if (subordinado != null)
                    {
                        funcionarioOriginal.Subordinados.Add(subordinado);
                    }
                }

                // Salve as alterações no banco de dados
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new { id = funcionario.Id });
            }

            // Recarregar lista de todos os funcionários
            var todosOsFuncionarios = await _context.Funcionarios.ToListAsync();
            ViewBag.TodosOsFuncionarios = await _context.Funcionarios.ToListAsync();

            return View(funcionario);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = _context.Funcionarios
                .Include(f => f.Subordinados)
                .Include(f => f.Superior)
                .FirstOrDefault(f => f.Id == id);

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

            TempData["MensagemExclusao"] = "Funcionário excluído com sucesso.";

            return RedirectToAction(nameof(Index));
        }

        private bool FuncionarioExists(int id)
        {
            return _context.Funcionarios.Any(e => e.Id == id);
        }
    }
}
