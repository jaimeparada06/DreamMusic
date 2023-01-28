using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models
{
    public class ItemDevolucion
    {
        [Key]
        public virtual int Id{ get; set; }

        [ForeignKey("DiscoId")]
        public virtual Disco Disco{ get; set; }

        public virtual int DiscoId { get; set; }

        

        [ForeignKey("DevolucionId")]
        public virtual Devolucion Devolucion { get; set; }

        public virtual int DevolucionId { get; set; }


        public virtual int CantidadDevolucion { get; set; }


        public override bool Equals(object obj)
        {
            return obj is ItemDevolucion item &&
                   Id == item.Id &&
                   EqualityComparer<Disco>.Default.Equals(Disco, item.Disco) &&
                   CantidadDevolucion == item.CantidadDevolucion &&
                   DiscoId == item.DiscoId &&
                   EqualityComparer<Devolucion>.Default.Equals(Devolucion, item.Devolucion) &&
                   DevolucionId == item.DevolucionId;
        }

    }
}
