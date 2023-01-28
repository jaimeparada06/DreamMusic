using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models
{
    public class ItemCompraProveedor
    {
        [Key]
        public virtual int Id { get; set; }

        [ForeignKey("DiscoId")]
        public virtual Disco Disco{ get; set; }

        public virtual int DiscoId { get; set; }

        public virtual int CantidadComprarProveedor { get; set; }



        [ForeignKey("CompraProveedorId")]
        public virtual CompraProveedor CompraProveedor { get; set; }

        public virtual int CompraProveedorId { get; set; }


        

        public override bool Equals(object obj)
        {
            return obj is ItemCompraProveedor item &&
                   Id == item.Id &&
                   EqualityComparer<Disco>.Default.Equals(Disco, item.Disco) &&
                   CantidadComprarProveedor == item.CantidadComprarProveedor &&
                   DiscoId == item.DiscoId &&
                   EqualityComparer<CompraProveedor>.Default.Equals(CompraProveedor, item.CompraProveedor) &&
                   CompraProveedor == item.CompraProveedor;
        }

    }
}
