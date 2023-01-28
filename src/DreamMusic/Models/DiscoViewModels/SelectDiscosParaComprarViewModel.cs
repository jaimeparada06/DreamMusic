using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models.DiscoViewModels
{
    public class SelectDiscosParaComprarViewModel
    {
        public IEnumerable<Disco> Discos { get; set; }
        public SelectList Generos;


        [Display(Name = "Genero")]
        public string discoGeneroSeleccionado { get; set; }

        [Display(Name = "Titulo")]
        public string discoTitulo { get; set; }

        [Display(Name = "Artista")]
        public string discoArtista { get; set; }
    }
}
