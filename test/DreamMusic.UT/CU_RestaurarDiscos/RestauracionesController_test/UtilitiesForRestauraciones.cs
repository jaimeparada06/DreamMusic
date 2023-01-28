using DreamMusic.Models;
using DreamMusic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamMusic.UT.Controladores;
using DreamMusic.UT.CU_ComprarDiscos;

namespace DreamMusic.UT.CU_RestaurarDiscos.DiscoController_test
{
    public static class UtilitiesForRestauraciones
    {
        public static void InitializeDbPurchasesForTests(ApplicationDbContext db)
        {
            var restauraciones = GetRestauracion(0, 1);
            foreach (Restauracion restauracion in restauraciones)
            {
                db.restauracion.Add(restauracion as Restauracion);
            }
            db.SaveChanges();

        }

        public static void ReInitializeDbPurchasesForTests(ApplicationDbContext db)
        {
            db.ItemRestauracion.RemoveRange(db.ItemRestauracion);
            db.restauracion.RemoveRange(db.restauracion);
            db.SaveChanges();
        }

        public static IList<Restauracion> GetRestauracion(int index, int numOfPurchases)
        {

            Administrador customer = Utilities.GetUsers(1, 1).First() as Administrador;
            var allRestauraciones = new List<Restauracion>();
            Restauracion restauracion;
            Disco disco;
            ItemRestauracion itemrestauracion;
            int quantity = 2;

            for (int i = 1; i < 3; i++)
            {
                disco = UtilitiesForDiscos.GetDiscos(i - 1, 1).First();
                disco.CantidadRestauracion = disco.CantidadRestauracion - quantity;
                restauracion = new Restauracion
                {
                    RestauracionId = i,
                    Administrador = customer,
                    AdministradorId = customer.Id,
                    Direccion = "Avd. EspaÃ±a s/n",
                    MetodoDePago = GetPaymentMethod(i - 1, 1).First(),
                    FechaRestauracion = System.DateTime.Now.Date,
                    PrecioTotal = disco.PrecioDeDevolucion,
                    ItemRestauracion = new List<ItemRestauracion>()
                };
                itemrestauracion = new ItemRestauracion
                {
                    Id = i,
                    CantidadRestauracion = quantity,
                    Disco = disco,
                    DiscoId = disco.DiscoId,
                    Restauracion = restauracion,
                    RestauracionId = restauracion.RestauracionId

                };
                restauracion.ItemRestauracion.Add(itemrestauracion);
                restauracion.PrecioTotal = itemrestauracion.CantidadRestauracion * itemrestauracion.Disco.PrecioDeRestauracion;
                allRestauraciones.Add(restauracion);

            }

            return allRestauraciones.GetRange(index, numOfPurchases);
        }

        public static IList<MetodoDePago> GetPaymentMethod(int index, int numOfPaymentMethods)
        {
            Administrador customer = Utilities.GetUsers(1, 1).First() as Administrador;
            var allPaymentMethods = new List<MetodoDePago>
                {
                new TarjetaDeCredito {ID = 1, NumeroTarjeta = "1111111111111111", CCV = "111", FechaCaducidad = new DateTime(2020, 10, 10) },
                new PayPal { ID = 2, CorreoElectronico = customer.Email, NumTelefono = customer.PhoneNumber, Prefijo = "+34" },

            };
            //return from the list as much instances as specified in numOfGenres
            return allPaymentMethods.GetRange(index, numOfPaymentMethods);
        }

    }
}
