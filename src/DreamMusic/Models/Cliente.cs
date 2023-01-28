using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models
{
    public class Cliente : ApplicationUser
    {
        
        [Required]
        public virtual string Nombre { get; set; }

        public virtual IList<Comprar> Compras { get; set; }
        public virtual IList<Devolucion> Devoluciones{ get; set; }

    }
}
