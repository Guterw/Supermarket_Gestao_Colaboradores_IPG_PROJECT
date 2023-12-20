using System.Linq;
using Hierarquias.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private readonly HierarquiasDbContext _context;

    public HomeController(HierarquiasDbContext context)
    {
        _context = context;
    }

    public IActionResult Subordinados()
    {
        var funcionarios = _context.Funcionarios.ToList();
        ViewBag.Funcionarios = new SelectList(funcionarios, "Id", "Nome");

        return View();
    }

    [HttpPost]
    public IActionResult ListarSubordinados(int id)
    {
        var subordinados = _context.Funcionarios
            .Where(f => f.SuperiorId == id)
            .ToList();

        // Obtém o nome do superior para exibição
        var nomeSuperior = _context.Funcionarios.Where(f => f.Id == id).Select(f => f.Nome).FirstOrDefault();

        ViewBag.NomeSuperior = nomeSuperior;
        return PartialView("_ListaSubordinados", subordinados);
    }



    public async Task<IActionResult> IndexAsync()
    {
        var funcionarios = await _context.Funcionarios.Include(f => f.Superior).ToListAsync();

        ViewBag.Funcionarios = funcionarios;

        return View();
    }
}