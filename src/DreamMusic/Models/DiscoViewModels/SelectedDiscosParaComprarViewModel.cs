using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models.DiscoViewModels
{
    public class SelectedDiscosParaComprarViewModel
    {
        public string[] IdsToAdd { get; set; }

        [Display(Name = "Genero")]
        public string discoGeneroSeleccionado { get; set; }

        [Display(Name = "Titulo")]
        public string discoTitulo { get; set; }

        [Display(Name = "Artista")]
        public string discoArtista { get; set; }
    }
}
