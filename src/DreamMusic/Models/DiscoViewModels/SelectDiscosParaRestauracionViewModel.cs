using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models.DiscoViewModels
{
    public class SelectDiscosParaRestauracionViewModel
    {
        public IEnumerable<Disco> Discos { get; set; }



        [Display(Name = "Titulo")]
        public string discoTitulo { get; set; }

        [Display(Name = "Año")]
        public int? discoAño { get; set; }

        [Display(Name = "Mes")]
        public int? discoMes { get; set; }

        [Display(Name = "Dia")]
        public int? discoDia { get; set; }
        [Display(Name = "Nombre")]
        public string nombreC { get; set; }
        [Display(Name = "Apellido")]
        public string apellidoC { get; set; }

    }
}