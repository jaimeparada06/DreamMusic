using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models
{
    public class PayPal: MetodoDePago
    {
        [EmailAddress]
        public string CorreoElectronico{ get; set; }

        public string Prefijo { get; set; }


        public string NumTelefono { get; set; }

        public override bool Equals(object obj)
        {
            return obj is PayPal pal &&
                   base.Equals(obj) &&
                   ID == pal.ID &&
                   CorreoElectronico == pal.CorreoElectronico &&
                   Prefijo == pal.Prefijo &&
                   NumTelefono == pal.NumTelefono;
        }
    }
}
