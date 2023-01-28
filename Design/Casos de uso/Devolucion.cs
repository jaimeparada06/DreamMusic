using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models
{
    public class Devolucion
    {
        [Key]
        public virtual int DevolucionId {get;set;}

        public virtual DateTime FechaEntrega { get; set; }

        public virtual double PrecioTotal{get;set;}

        public virtual DateTime FechaDevolucion{get;set;}

        public virtual String Direccion{get;set;}

        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }

        //Para el aplicacion user
        public virtual string ClienteId { get; set; }

        public virtual IList<ItemDevolucion> ItemDevolucions{get;set;}

        [Required()]
        public MetodoDePago MetodoDePago { get; set; }


        public override bool Equals(object obj)
        {
            return obj is Devolucion devolucion &&
                   DevolucionId == devolucion.DevolucionId &&
                   PrecioTotal == devolucion.PrecioTotal &&
                   FechaDevolucion == devolucion.FechaDevolucion &&
                   Direccion == devolucion.Direccion &&
                   EqualityComparer<Cliente>.Default.Equals(Cliente, devolucion.Cliente) &&
                   EqualityComparer<MetodoDePago>.Default.Equals(MetodoDePago, devolucion.MetodoDePago) &&
                   ClienteId == devolucion.ClienteId;
        }











    }
}
