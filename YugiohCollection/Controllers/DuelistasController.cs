using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YugiohCollection.Data;
using YugiohCollection.Models;
using YugiohCollection.ViewModels;

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
        public async Task<IActionResult> Create(DuelistaViewModel duelistavm)
        {

            Duelista duelista = new Duelista();

            if (ModelState.IsValid)
            {
                duelista.Id = duelistavm.Id;
                duelista.Cartas = duelistavm.Cartas;
                duelista.Nome = duelistavm.Nome;

                var imgPrefixo = Guid.NewGuid() + "_";
                if(! await UploadArquivo(duelistavm.ImagemUpload, imgPrefixo))
                {
                    return View(duelistavm);
                }

                duelista.Imagem = imgPrefixo + duelistavm.ImagemUpload.FileName;
                _context.Add(duelista);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index");
        }

        // GET: Duelistas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DuelistaViewModel duelistavm = new DuelistaViewModel();

            var duelista = await _context.Duelistas.FindAsync(id);

            duelistavm.Id = duelista.Id;
            duelistavm.Nome = duelista.Nome;
            duelistavm.Imagem = duelista.Imagem;
            duelistavm.Cartas = duelista.Cartas;

            if (duelista == null)
            {
                return NotFound();
            }
            return View(duelistavm);
        }

        // POST: Duelistas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, DuelistaViewModel duelistavm)
        {
            Duelista duelista = new Duelista();

            if (id != duelistavm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    duelista.Id = duelistavm.Id;
                    duelista.Cartas = duelistavm.Cartas;
                    duelista.Nome = duelistavm.Nome;

                    var imgPrefixo = Guid.NewGuid() + "_";

                    if(duelistavm.ImagemUpload != null)
                    {
                        if (!await UploadArquivo(duelistavm.ImagemUpload, imgPrefixo))
                        {
                            return View(duelistavm);
                        }

                        duelista.Imagem = imgPrefixo + duelistavm.ImagemUpload.FileName;
                    }
                    else
                    {
                        var duelistas = _context.Duelistas.AsNoTracking();
                        var duelistaFinal = (from duelistaAtual in duelistas
                                             where duelistaAtual.Id == id
                                             select duelistaAtual).First();

                        duelista.Imagem = duelistaFinal.Imagem;
                    }
                    
                    _context.Update(duelista);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DuelistaExists(duelistavm.Id))
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

        private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com esse nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }
    }
}
