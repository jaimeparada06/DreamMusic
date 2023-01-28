using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models
{
    public class Disco
    {
        [Key]
        public virtual int DiscoId { get; set; }
        public virtual int PrecioDeDevolucion { get; set; }
        public virtual int PrecioDeCompra { get; set; }

        [Required]
        public virtual Genero Genero { get; set; }
        public virtual int PrecioComprarProveedor { get; set; }

        public virtual int PrecioDeRestauracion { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "El nombre no puede ser mayor de 40 caracteres")]
        public virtual string Titulo { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El nombre no puede ser mayor de 100 caracteres")]
        public virtual string Artista { get; set; }

        


        public virtual IList<ItemDevolucion> ItemDevolucion { get; set; }

        public virtual DateTime FechaLanzamiento { get; set; }
        public virtual IList<ItemDeCompra> ItemCompra { get; set; }
        public virtual IList<ItemRestauracion> ItemRestauracion { get; set; }
        public virtual IList<ItemCompraProveedor> DiscosCompradosProveedor { get; set; }

        public virtual int CantidadCompra { get; set; }

        public virtual int CantidadDevolucion { get; set; }
        public virtual int CantidadRestauracion{get;set;}

        public override bool Equals(object obj)
        {
            return obj is Disco disco &&
                   DiscoId == disco.DiscoId &&
                   Titulo == disco.Titulo &&
                   PrecioDeCompra == disco.PrecioDeCompra &&
                   PrecioComprarProveedor == disco.PrecioComprarProveedor &&
                   PrecioDeDevolucion == disco.PrecioDeDevolucion &&
                   PrecioDeRestauracion == disco.PrecioDeRestauracion &&
                   CantidadDevolucion == disco.CantidadDevolucion &&
                   Artista == disco.Artista &&
                   CantidadCompra == disco.CantidadCompra &&
                   EqualityComparer<Genero>.Default.Equals(Genero, disco.Genero) &&
                   //ItemCompra == disco.ItemCompra &&
                   //ItemDevolucion == disco.ItemDevolucion &&
                   FechaLanzamiento == disco.FechaLanzamiento;
        }

    }
}
