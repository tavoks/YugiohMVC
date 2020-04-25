using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YugiohCollection.Data;
using YugiohCollection.Models;

namespace YugiohCollection.Controllers
{
    public class DuelistasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DuelistasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Duelistas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Duelistas.ToListAsync());
        }

        // GET: Duelistas/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var duelista = await _context.Duelistas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (duelista == null)
            {
                return NotFound();
            }

            return View(duelista);
        }

        // GET: Duelistas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Duelistas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Duelista duelista)
        {
            if (ModelState.IsValid)
            {
                _context.Add(duelista);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(duelista);
        }

        // GET: Duelistas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var duelista = await _context.Duelistas.FindAsync(id);
            if (duelista == null)
            {
                return NotFound();
            }
            return View(duelista);
        }

        // POST: Duelistas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Duelista duelista)
        {
            if (id != duelista.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(duelista);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DuelistaExists(duelista.Id))
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
            return View(duelista);
        }

        // GET: Duelistas/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var duelista = await _context.Duelistas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (duelista == null)
            {
                return NotFound();
            }

            return View(duelista);
        }

        // POST: Duelistas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var duelista = await _context.Duelistas.FindAsync(id);
            _context.Duelistas.Remove(duelista);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DuelistaExists(Guid id)
        {
            return _context.Duelistas.Any(e => e.Id == id);
        }
    }
}
