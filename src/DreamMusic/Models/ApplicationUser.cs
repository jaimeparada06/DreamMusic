using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models
{
    public class ApplicationUser: IdentityUser
    {
        public virtual string Name { get; set; }

        public virtual string Apellido1 { get; set; }

        public virtual string Apellido2 { get; set; }

        [Required]
        [StringLength(9, ErrorMessage = "Dni no tiene 9 caracteres")]
        public virtual string Dni { get; set; }



        public override bool Equals(object obj)
        {
            return obj is ApplicationUser user &&
                   Id == user.Id &&
                   Email == user.Email &&
                   PhoneNumber == user.PhoneNumber &&
                   EqualityComparer<DateTimeOffset?>.Default.Equals(LockoutEnd, user.LockoutEnd) &&
                   LockoutEnabled == user.LockoutEnabled &&
                   Name == user.Name &&
                   Apellido1 == user.Apellido1 &&
                   Apellido2 == user.Apellido2;
        }

    }
}