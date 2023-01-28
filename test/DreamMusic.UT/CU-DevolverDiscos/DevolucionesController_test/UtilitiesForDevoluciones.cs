using DreamMusic.Models;
using DreamMusic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamMusic.UT.Controladores;
using DreamMusic.UT.CU_ComprarDiscos;

namespace DreamMusic.UT.CU_DevolverDiscos.DiscoController_test
{
    public static class UtilitiesForDevoluciones
    {
        public static void InitializeDbPurchasesForTests(ApplicationDbContext db)
        {
            var devoluciones = GetDevolucion(0, 1);
            foreach (Devolucion devolucion in devoluciones)
            {
                db.Devoluciones.Add(devolucion as Devolucion);
            }
            db.SaveChanges();

        }

        public static void ReInitializeDbPurchasesForTests(ApplicationDbContext db)
        {
            db.ItemDevoluciones.RemoveRange(db.ItemDevoluciones);
            db.Devoluciones.RemoveRange(db.Devoluciones);
            db.SaveChanges();
        }

        public static IList<Devolucion> GetDevolucion(int index, int numOfPurchases)
        {

            Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
            var allDevoluciones = new List<Devolucion>();
            Devolucion devolucion;
            Disco disco;
            ItemDevolucion itemdevolucion ;
            int quantity = 2;

            for (int i = 1; i < 3; i++)
            {
                disco = UtilitiesForDiscos.GetDiscos(i - 1, 1).First();
                disco.CantidadDevolucion = disco.CantidadDevolucion - quantity;
                devolucion = new Devolucion
                {
                    DevolucionId = i,
                    Cliente = customer,
                    ClienteId = customer.Id,
                    Direccion = "Avd. EspaÃ±a s/n",
                    MetodoDePago= GetPaymentMethod(i-1,1).First(),
                    FechaDevolucion = System.DateTime.Now.Date,
                    PrecioTotal = disco.PrecioDeDevolucion,
                    ItemDevolucions = new List<ItemDevolucion>()
                };
                itemdevolucion = new ItemDevolucion
                {
                    Id = i,
                    CantidadDevolucion = quantity,
                    Disco = disco,
                    DiscoId = disco.DiscoId,
                    Devolucion = devolucion,
                    DevolucionId = devolucion.DevolucionId

                };
                devolucion.ItemDevolucions.Add(itemdevolucion);
                devolucion.PrecioTotal = itemdevolucion.CantidadDevolucion * itemdevolucion.Disco.PrecioDeDevolucion;
                allDevoluciones.Add(devolucion);

            }

            return allDevoluciones.GetRange(index, numOfPurchases);
        }

        public static IList<MetodoDePago> GetPaymentMethod(int index, int numOfPaymentMethods)
        {
            Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
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
