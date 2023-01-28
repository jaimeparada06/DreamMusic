using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models
{
    public class TarjetaDeCredito: MetodoDePago
    {
        [Display(Name = "Tarjeta De Crédito")]
        [Required]
        public virtual string NumeroTarjeta { get; set; }

        [Required]
        public virtual string CCV { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/yyyy}")]

        public virtual DateTime FechaCaducidad { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TarjetaDeCredito card &&
                   base.Equals(obj) &&
                   ID == card.ID &&
                   NumeroTarjeta == card.NumeroTarjeta &&
                   CCV == card.CCV &&
                   FechaCaducidad == card.FechaCaducidad;
        }




    }
}
