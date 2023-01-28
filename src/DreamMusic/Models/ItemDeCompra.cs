using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models
{
    public class ItemDeCompra
    {

        [Key]
        public virtual int Id { get; set; }

        [ForeignKey("DiscoId")]
        public virtual Disco Disco { get; set; }

        public virtual int DiscoId { get; set; }

        [ForeignKey("CompraId")]
        public virtual Comprar Comprar { get; set; }

        public virtual int CompraId { get; set; }

        public virtual int CantidadCompra { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ItemDeCompra item &&
                   Id == item.Id &&
                   EqualityComparer<Disco>.Default.Equals(Disco, item.Disco) &&
                   CantidadCompra == item.CantidadCompra &&
                   DiscoId == item.DiscoId &&
                   EqualityComparer<Comprar>.Default.Equals(Comprar, item.Comprar) &&
                   CompraId == item.CompraId;
        }

        public static implicit operator ItemDeCompra(int v)
        {
            throw new NotImplementedException();
        }
    }
}