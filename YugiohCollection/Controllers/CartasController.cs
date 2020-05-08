using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class CartasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cartas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Cartas.Include(c => c.Duelista);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Cartas/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carta = await _context.Cartas
                .Include(c => c.Duelista)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carta == null)
            {
                return NotFound();
            }

            return View(carta);
        }

        // GET: Cartas/Create
        public IActionResult Create()
        {
            ViewData["DuelistaID"] = new SelectList(_context.Duelistas, "Id", "Nome");
            return View();
        }

        // POST: Cartas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CartaViewModel cartavm)
        {
            Carta carta = new Carta();

            if (ModelState.IsValid)
            {
                carta.Id = cartavm.Id;
                carta.Duelista = cartavm.Duelista;
                carta.DuelistaID = cartavm.DuelistaID;
                carta.Efeito = cartavm.Efeito;
                carta.Imagem = cartavm.Imagem;
                carta.Nome = cartavm.Nome;
                carta.Tipo = cartavm.Tipo;

                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await UploadArquivo(cartavm.ImagemUpload, imgPrefixo))
                {
                    return View(cartavm);
                }

                carta.Imagem = imgPrefixo + cartavm.ImagemUpload.FileName;
                _context.Add(carta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DuelistaID"] = new SelectList(_context.Duelistas, "Id", "Nome", carta.DuelistaID);
            return RedirectToAction("Index");
        }

        // GET: Cartas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CartaViewModel cartavm = new CartaViewModel();

            var carta = await _context.Cartas.FindAsync(id);
            if (carta == null)
            {
                return NotFound();
            }

            cartavm.Id = carta.Id;
            cartavm.Duelista = carta.Duelista;
            cartavm.DuelistaID = carta.DuelistaID;
            cartavm.Efeito = carta.Efeito;
            cartavm.Imagem = carta.Imagem;
            cartavm.Nome = carta.Nome;
            cartavm.Tipo = carta.Tipo;

            ViewData["DuelistaID"] = new SelectList(_context.Duelistas, "Id", "Nome", carta.DuelistaID);
            return View(cartavm);
        }

        // POST: Cartas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CartaViewModel cartavm)
        {
            Carta carta = new Carta();

            if (id != cartavm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    carta.Id = cartavm.Id;
                    carta.Duelista = cartavm.Duelista;
                    carta.DuelistaID = cartavm.DuelistaID;
                    carta.Efeito = cartavm.Efeito;
                    carta.Nome = cartavm.Nome;
                    carta.Tipo = cartavm.Tipo;

                    var imgPrefixo = Guid.NewGuid() + "_";

                    if(cartavm.ImagemUpload != null)
                    {
                        if (!await UploadArquivo(cartavm.ImagemUpload, imgPrefixo))
                        {
                            return View(cartavm);
                        }

                        carta.Imagem = imgPrefixo + cartavm.ImagemUpload.FileName;
                    }
                    else
                    {
                        var cartas = _context.Cartas.AsNoTracking();
                        var cartaFinal = (from cartaAtual in cartas
                                           where cartaAtual.Id == id
                                           select cartaAtual).First();

                        carta.Imagem = cartaFinal.Imagem;
                    }
                    
                    _context.Update(carta);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartaExists(cartavm.Id))
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
            ViewData["DuelistaID"] = new SelectList(_context.Duelistas, "Id", "Nome", carta.DuelistaID);
            return View(carta);
        }

        // GET: Cartas/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carta = await _context.Cartas
                .Include(c => c.Duelista)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carta == null)
            {
                return NotFound();
            }

            return View(carta);
        }

        // POST: Cartas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var carta = await _context.Cartas.FindAsync(id);
            _context.Cartas.Remove(carta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartaExists(Guid id)
        {
            return _context.Cartas.Any(e => e.Id == id);
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
