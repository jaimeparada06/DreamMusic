using DreamMusic.Data;
using DreamMusic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamMusic.UT.Controladores
{
    public static class Utilities
    {
        public static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            // Crear un nuevo proveedor de servicios, y una nueva
            //instancia de la base de datos temporal:
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
            // Crear una nueva instancia de opciones que use
            // la BD temporal ofrecida por el proveedor de servicios:
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase("DreamMusic")
                .UseInternalServiceProvider(serviceProvider);
            return builder.Options;
        }

        public static void InitializeDbCustomersForTests(ApplicationDbContext db)
        {

            db.Users.Add(GetUsers(0, 1).First());
            db.SaveChanges();
        }

        public static void ReInitializeDbUsersForTests(ApplicationDbContext db)
        {
            db.Users.RemoveRange(db.Users);
            db.SaveChanges();
        }

        public static IList<ApplicationUser> GetUsers(int index, int numOfUsers)
        {
            var allUsers = new List<ApplicationUser>
                {
                   new Cliente { Id = "1", UserName = "peter@uclm.com", PhoneNumber = "967959595",  Email = "peter@uclm.com", Name = "Peter", Apellido1 = "Jackson", Apellido2 = "García" },
                   new Administrador { Id = "1", UserName = "elena@uclm.com", PhoneNumber = "967959598",  Email = "elena@uclm.com", Name = "Elena", Apellido1 = "Navarro", Apellido2 = "Martínez" }
                };
            //return from the list as much instances as specified in numOfGenres
            return allUsers.GetRange(index, numOfUsers);
        }



    }
}
