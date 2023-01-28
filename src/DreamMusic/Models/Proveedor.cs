using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models
{
    public class Proveedor
    {

        [Key]
        public virtual int Id { get;  set; }

        [Required]
        public virtual string Nombre { get; set; }

        public virtual IList<CompraProveedor> CompraProveedores { get; set; }




    }
}