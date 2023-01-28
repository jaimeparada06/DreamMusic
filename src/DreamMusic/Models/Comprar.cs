using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models
{
    public class Comprar
    {

        [Key]
        public virtual int CompraId { get; set; }

        public virtual double PrecioTotal { get; set; }

        public virtual DateTime FechaCompra { get; set; }

        public virtual IList<ItemDeCompra> ComprarItem { get; set; }

        public Comprar()
        {ComprarItem = new List<ItemDeCompra>();}


        public virtual String Direccion { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }

        public virtual String ClienteId { get; set; }


        [Required()]
        public MetodoDePago MetodoDePago{ get;set;}

        
        public override bool Equals(object obj)
        {


        
            return obj is Comprar purchase &&
                   CompraId == purchase.CompraId &&
                   PrecioTotal == purchase.PrecioTotal &&
                   (this.FechaCompra.Subtract(purchase.FechaCompra) < new TimeSpan(0, 1, 0)) &&
                   Direccion == purchase.Direccion &&
                   EqualityComparer<Cliente>.Default.Equals(Cliente, purchase.Cliente) &&
                   ClienteId == purchase.ClienteId &&
                   EqualityComparer<MetodoDePago>.Default.Equals(MetodoDePago, purchase.MetodoDePago);
        }

    }
}
