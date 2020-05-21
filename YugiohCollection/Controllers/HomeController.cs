using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YugiohCollection.Data;
using YugiohCollection.Models;
using YugiohCollection.ViewModels;

namespace YugiohCollection.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var duelista = ObterDuelistas();
            var carta = ObterCartas();

            DetalheViewModel detalhes = new DetalheViewModel();

            detalhes.Duelista = duelista;
            detalhes.Carta = carta;
            //_DuelistasPartialView();
            //_CartasPartialView();
            return View(detalhes); 
        }

        public IActionResult _DuelistasPartialView()
        {
            var Duelistas = ObterDuelistas();

            return PartialView(Duelistas);
        }

        public IActionResult _CartasPartialView()
        {
            var Cartas = ObterCartas();

            return PartialView(Cartas);
        }


        public List<Duelista> ObterDuelistas()
        {
            List<Duelista> listaDuelistas = new List<Duelista>();

            listaDuelistas = _context.Duelistas.AsQueryable().ToList();
            return listaDuelistas;
        }

        public List<Carta> ObterCartas()
        {
            List<Carta> listaCartas = new List<Carta>();

            listaCartas = _context.Cartas.AsQueryable().ToList();
            return listaCartas;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
