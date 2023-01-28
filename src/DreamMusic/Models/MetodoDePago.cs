using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models
{
    public abstract class MetodoDePago
    {
        [Key]
        public virtual int ID{get;set;}

        public override bool Equals(object obj)
        {
            return obj is MetodoDePago method &&
                   ID == method.ID;
        }
    }
}
