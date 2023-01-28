using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models
{
    public class Genero
    {
        [Key]
        public virtual int GeneroID { get; set; }

        [Required]
        public virtual string Nombre { get; set; }

        public virtual ICollection<Disco> Discos { get; set; } //Para la relacion

        public override bool Equals(object obj)
        {
            return obj is Genero genero &&
                   GeneroID == genero.GeneroID &&
                   Nombre == genero.Nombre;
        }
        
    }
}
