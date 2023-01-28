using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models.DiscoViewModels
{
    public class SelectDiscosParaDevolucionViewModel
    {
        public IEnumerable<Disco> Discos { get; set; }

        public IEnumerable<Devolucion> Devoluciones { get; set; }

        //public IEnumerable<Comprar> Compras { get; set; }

        public IEnumerable<ItemDeCompra> ItemDeCompras { get; set; }

        public SelectList Generos;

        [Display(Name = "Genero")]
        public string discoGeneroSeleccionado { get; set; }


        [Display(Name = "Titulo")]
        public string discoTitulo { get; set; }


        [Display(Name ="Artista")]
        public string discoArtista { get; set; }

        [Display(Name = "Año")]
        public int? discoAño { get; set; }

        [Display(Name = "Mes")]
        public int? discoMes { get; set; }

        [Display(Name = "Dia")]
        public int? discoDia { get; set; }


    }
}
