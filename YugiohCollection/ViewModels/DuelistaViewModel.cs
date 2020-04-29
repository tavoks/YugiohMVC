using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YugiohCollection.Models;

namespace YugiohCollection.ViewModels
{
    public class DuelistaViewModel
    {
        //public DuelistaViewModel()
        //{
        //    Id = Guid.NewGuid();
        //}

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public IFormFile ImagemUpload { get; set; }
        public string Imagem { get; set; }
        public IEnumerable<Carta> Cartas { get; set; }
    }
}
