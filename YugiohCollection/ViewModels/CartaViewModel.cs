using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YugiohCollection.Models;

namespace YugiohCollection.ViewModels
{
    public class CartaViewModel
    {

        //public CartaViewModel()
        //{
        //    Id = Guid.NewGuid();
        //}
        public Guid Id { get; set; }
        public Guid DuelistaID { get; set; }
        public string Nome { get; set; }
        public TipoCarta Tipo { get; set; }
        public string Efeito { get; set; }
        public string Imagem { get; set; }
        public IFormFile ImagemUpload { get; set; }
        public Duelista Duelista { get; set; }
    }
}
