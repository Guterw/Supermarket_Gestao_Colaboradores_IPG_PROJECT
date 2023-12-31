﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hierarquias.Models;
using Hierarquias.Data;

namespace Hierarquias.Controllers
{
    public class CargosController : Controller
    {
        private readonly HierarquiasDbContext _context;

        public CargosController(HierarquiasDbContext context)
        {
            _context = context;
        }

        // GET: Cargos
        public async Task<IActionResult> Index()
        {
            if (TempData.ContainsKey("MensagemExclusao"))
            {
                ViewBag.MensagemExclusao = TempData["MensagemExclusao"];
            }

            return View(await _context.Cargos.ToListAsync());
        }

        // GET: Cargos/Detalhes
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cargo = await _context.Cargos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Cargo == null)
            {
                return RedirectToAction("ItemNaoEncontrado");
            }

            if (TempData.ContainsKey("MensagemCriadoSuccess"))
            {
                ViewBag.MensagemCriadoSuccess = TempData["MensagemCriadoSuccess"];
            }

            return View(Cargo);
        }

        // GET: Cargos/Criar
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cargos/Criar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Cargos Cargo)
        {
            if (ModelState.IsValid)
            {
                // Verificar se o nome do cargo já existe
                if (_context.Cargos.Any(c => c.Nome == Cargo.Nome))
                {
                    ModelState.AddModelError("Nome", "Já existe um cargo com este nome.");
                    return View(Cargo);
                }

                _context.Add(Cargo);
                await _context.SaveChangesAsync();

                TempData["MensagemCriadoSuccess"] = "Cargo criado com sucesso!";

                return RedirectToAction(nameof(Details), new { id = Cargo.Id });
            }

            return View(Cargo);
        }

        // GET: Cargos/Editar
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cargo = await _context.Cargos.FindAsync(id);
            if (Cargo == null)
            {
                return RedirectToAction("ItemNaoEncontrado");
            }
            return View(Cargo);
        }

        // POST: Cargos/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Cargos Cargo)
        {
            if (id != Cargo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Verificar se o nome do cargo já existe (exceto para o cargo sendo editado)
                if (_context.Cargos.Any(c => c.Nome == Cargo.Nome && c.Id != Cargo.Id))
                {
                    ModelState.AddModelError("Nome", "Já existe um cargo com este nome.");
                    return View(Cargo);
                }

                try
                {
                    _context.Update(Cargo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CargoExists(Cargo.Id))
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
            return View(Cargo);
        }

        // GET: Cargos/Deletar
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cargo = await _context.Cargos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Cargo == null)
            {
                return NotFound();
            }

            return View(Cargo);
        }

        // POST: Cargos/Deletar
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cargos = await _context.Cargos.FindAsync(id);
            if (cargos == null)
            {
                return NotFound();
            }

            _context.Cargos.Remove(cargos);
            await _context.SaveChangesAsync();

            TempData["MensagemExclusao"] = "Cargo excluído com sucesso.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ItemNaoEncontrado()
        {
            return View();
        }

        private bool CargoExists(int id)
        {
            return _context.Cargos.Any(e => e.Id == id);
        }
    }
}