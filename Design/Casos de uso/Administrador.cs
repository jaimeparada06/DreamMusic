using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models
{
    public class Administrador : ApplicationUser
    {

        [Required]
        public virtual string Nombre { get; set; }

        public virtual IList<CompraProveedor> CompraProveedores { get; set; }
        public virtual IList<Restauracion> Restauraciones { get; set; }
        public virtual IList<Comprar> Compras { get; set; }



    }
}

