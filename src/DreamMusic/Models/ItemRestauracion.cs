using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models
{
    public class ItemRestauracion
    {
        [Key]
        public virtual int Id { get; set; }

        public virtual int DiscoId { get; set; }

        

        [ForeignKey("RestauracionId")]
        public virtual Restauracion Restauracion { get; set; }
        [ForeignKey("DiscoId")]
        public virtual Disco Disco { get; set; }
        public virtual int RestauracionId { get; set; }


        public virtual int CantidadRestauracion { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ItemRestauracion item &&
                   Id == item.Id &&
                   EqualityComparer<Disco>.Default.Equals(Disco, item.Disco) &&
                   CantidadRestauracion == item.CantidadRestauracion &&
                   DiscoId == item.DiscoId &&
                   EqualityComparer<Restauracion>.Default.Equals(Restauracion, item.Restauracion) &&
                   RestauracionId == item.RestauracionId;
        }
        
    }
}
