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

    public async Task<IActionResult> IndexAsync()
    {
        var funcionarios = await _context.Funcionarios.ToListAsync();

        ViewBag.Funcionarios = funcionarios;

        return View();
    }
}