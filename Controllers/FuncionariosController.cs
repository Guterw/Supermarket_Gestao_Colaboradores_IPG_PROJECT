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
                .Include(f => f.Superiores)
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
                .Include(f => f.Superiores)
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
        public async Task<IActionResult> Create([Bind("Id,Nome,Sobrenome,CargoId")] Funcionarios funcionario, int[] Superiores)
        {
            if (ModelState.IsValid)
            {
                // Adiciona o funcionário ao contexto
                _context.Add(funcionario);

                // Adiciona os superiores ao funcionário
                if (Superiores != null && Superiores.Any())
                {
                    funcionario.Superiores = await _context.Funcionarios.Where(f => Superiores.Contains(f.Id)).ToListAsync();
                }

                // Salva as alterações no banco de dados
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Se houver erros de validação, recupera os superiores novamente para preencher a ViewBag
            ViewBag.TodosOsFuncionarios = new SelectList(await _context.Funcionarios.ToListAsync(), "Id", "Nome");
            return View(funcionario);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Obtenha o funcionário que está sendo editado
            var funcionario = await _context.Funcionarios
                .Include(f => f.Superiores)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (funcionario == null)
            {
                return NotFound();
            }

            // Popule a lista de todos os funcionários
            var todosOsFuncionarios = await _context.Funcionarios.ToListAsync();
            ViewBag.TodosOsFuncionarios = todosOsFuncionarios;

            // Selecione os superiores do funcionário que está sendo editado
            ViewBag.SuperioresSelecionados = funcionario.Superiores.Select(s => s.Id).ToList();

            return View(funcionario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Funcionarios viewModel)
        {
            if (ModelState.IsValid)
            {
                var funcionarioOriginal = await _context.Funcionarios
                    .Include(f => f.Superiores)
                    .FirstOrDefaultAsync(f => f.Id == viewModel.Id);

                if (funcionarioOriginal == null)
                {
                    return NotFound();
                }

                // Atualize outras propriedades...
                funcionarioOriginal.Nome = viewModel.Nome;

                // Atualize a lista de superiores
                funcionarioOriginal.Superiores.Clear();
                if (viewModel.Superiores != null)
                {
                    foreach (var superior in viewModel.Superiores)
                    {
                        // Certifique-se de que o 'superior' seja do tipo Funcionarios
                        if (superior is Funcionarios)
                        {
                            var superiorExistente = ((Funcionarios)superior)?.Id != null
    ? await _context.Funcionarios.FindAsync(((Funcionarios)superior).Id)
    : null;

                            if (superiorExistente != null)
                            {
                                funcionarioOriginal.Superiores.Add(superior);
                            }
                        }
                    }
                }

                // Atualize a lista de subordinados
                funcionarioOriginal.Subordinados.Clear();
                if (viewModel.Subordinados != null)
                {
                    foreach (var subordinado in viewModel.Subordinados)
                    {
                        // Certifique-se de que o 'subordinado' seja do tipo Funcionarios
                        if (subordinado is Funcionarios)
                        {
                            var subordinadoExistente = await _context.Funcionarios.FindAsync(((Funcionarios)subordinado).Id);
                            if (subordinadoExistente != null)
                            {
                                funcionarioOriginal.Subordinados.Add(subordinadoExistente);
                            }
                        }
                    }
                }

                // Salve as alterações no banco de dados
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new { id = viewModel.Id });
            }

            // Se houver erros de validação, recupere a lista de todos os funcionários novamente
            var todosOsFuncionarios = await _context.Funcionarios.ToListAsync();
            ViewBag.TodosOsFuncionarios = todosOsFuncionarios;

            return View(viewModel);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = _context.Funcionarios
                .Include(f => f.Subordinados)
                .Include(f => f.Superiores)
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
