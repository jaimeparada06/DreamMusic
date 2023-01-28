using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models
{
    public class CompraProveedor
    {
        [Key]
        public virtual int CompraProveedorId {get;set;}

        public virtual double PrecioTotal{get;set;}

        public virtual DateTime FechaCompraProveedor{get;set;}

        public virtual String Direccion{get;set;}

        [ForeignKey("AdministradorId")]
        public virtual Administrador Administrador { get; set; }

        //Para el aplicacion user
        public virtual string AdministradorId { get; set; }

        public virtual IList<ItemCompraProveedor> ItemCompraProveedor{get;set;}


        public override bool Equals(object obj)
        {
            return obj is CompraProveedor compraProveedor &&
                   CompraProveedorId == compraProveedor.CompraProveedorId &&
                   PrecioTotal == compraProveedor.PrecioTotal &&
                   FechaCompraProveedor == compraProveedor.FechaCompraProveedor &&
                   Direccion == compraProveedor.Direccion &&
                   EqualityComparer<Administrador>.Default.Equals(Administrador, compraProveedor.Administrador) &&
                   AdministradorId == compraProveedor.AdministradorId;
        }











    }
}
