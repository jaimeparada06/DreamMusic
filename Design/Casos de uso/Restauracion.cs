using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models
{
    public class Restauracion
    {
        [Key]
        public virtual int RestauracionId {get;set;}
        
        public virtual double PrecioTotal{get;set;}

        public virtual DateTime FechaRestauracion{get;set;}

        public virtual String Direccion{get;set;}

        [ForeignKey("AdministradorId")]
        public virtual Administrador Administrador { get; set; }

        //Para el aplicacion user
        public virtual string AdministradorId { get; set; }

        public virtual IList<ItemRestauracion> ItemRestauracion{get;set;}
        [Required()]
        public MetodoDePago MetodoDePago { get; set; }



        public override bool Equals(object obj)
        {
            return obj is Restauracion restauracion &&
                   RestauracionId == restauracion.RestauracionId &&
                   PrecioTotal == restauracion.PrecioTotal &&
                   FechaRestauracion == restauracion.FechaRestauracion &&
                   Direccion == restauracion.Direccion &&
                   EqualityComparer<Administrador>.Default.Equals(Administrador, restauracion.Administrador) &&
                   AdministradorId == restauracion.AdministradorId;
        }






        




    }
}
