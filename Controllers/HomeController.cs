using System.Linq;
using Hierarquias.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private readonly HierarquiasDbContext _context;

    public HomeController(HierarquiasDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var funcionarios = _context.Funcionarios
            .Include(f => f.Cargo)
            .ToList();

        ViewBag.Funcionarios = funcionarios;

        return View();
    }
}