using DreamMusic.Data;
using DreamMusic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamMusic.UT.CU_ComprarDiscos
{
    public static class UtilitiesForDiscos
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

        public static IList<Disco> GetDiscos(int index, int numeroDeDiscos)
        {
            Genero genero = GetGeneros(0, 1).First();
            Genero genero2 = GetGeneros(1, 1).First();
            Genero genero3 = GetGeneros(3, 1).First();
            var todosLosDiscos = new List<Disco>
        {
            new Disco { DiscoId = 1, Titulo = "Scorpion", Genero= genero,
            FechaLanzamiento = new DateTime(2018, 10, 20), PrecioDeCompra = 30,
            Artista = "Drake", CantidadCompra=10,CantidadDevolucion=10 },
            new Disco { DiscoId = 2, Titulo = "UnderPressure", Genero= genero,
            FechaLanzamiento = new DateTime(2014, 9, 13), PrecioDeCompra = 23,
            Artista = "Logic", CantidadCompra=10,CantidadDevolucion=10},
            new Disco { DiscoId = 3, Titulo = "Bohemian Rhapsody", Genero= genero3,
            FechaLanzamiento = new DateTime(2018, 5, 27), PrecioDeCompra = 45,
            Artista = "Queen",CantidadCompra=10,CantidadDevolucion=10 },
            new Disco { DiscoId = 4, Titulo = "Thriller", Genero= genero2,
            FechaLanzamiento = new DateTime(1982, 4, 19), PrecioDeCompra = 29,
            Artista = "Michael Jackson",CantidadCompra=10,CantidadDevolucion=10 }
        };
            return todosLosDiscos.GetRange(index, numeroDeDiscos);
        }



        public static IList<Genero> GetGeneros(int index, int numeroDeGeneros)
        {

            var todosLosGeneros = new List<Genero>
        {
            new Genero { GeneroID = 1, Nombre = "Rap"},
            new Genero { GeneroID = 2, Nombre = "Pop"},
            new Genero { GeneroID = 3, Nombre = "Electrónica"},
            new Genero { GeneroID = 4, Nombre = "Rock"},
        };
            return todosLosGeneros.GetRange(index, numeroDeGeneros);
        }


        public static void InitializeDbGenresForTests(ApplicationDbContext db)
        {
            db.Generos.AddRange(GetGeneros(0, 3));
            db.SaveChanges();

        }

        public static void ReInitializeDbGenresForTests(ApplicationDbContext db)
        {
            db.Generos.RemoveRange(db.Generos);
            db.SaveChanges();
        }



        public static void InitializeDbDiscosForTests(ApplicationDbContext db)
        {
            db.Discos.AddRange(GetDiscos(0, 4));
            //genre id=1 it is already added because it is related to the discos
            db.Generos.AddRange(GetGeneros(2,1));
            db.SaveChanges();

            db.Users.Add(new Cliente { Id = "1", UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Name = "Peter", Apellido1 = "Jackson", Apellido2 = "García" });
            db.SaveChanges();

        }
        public static void ReInitializeDbDiscosForTests(ApplicationDbContext db)
        {
            db.Discos.RemoveRange(db.Discos);
            db.Generos.RemoveRange(db.Generos);
            db.SaveChanges();
        }



    }
}