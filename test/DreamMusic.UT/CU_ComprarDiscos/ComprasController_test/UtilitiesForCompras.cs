using DreamMusic.Models;
using DreamMusic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamMusic.UT.Controladores;
using DreamMusic.UT.CU_ComprarDiscos;

namespace DreamMusic.UT.CU_ComprarDiscos.ComprasController_test
{
    public static class UtilitiesForCompras
    {
        public static void InitializeDbPurchasesForTests(ApplicationDbContext db)
        {
            var purchases = GetPurchases(0, 1);
            foreach(Comprar purchase in purchases)
            {
                db.Compras.Add(purchase as Comprar);
            }
            db.SaveChanges();

        }

        public static void ReInitializeDbPurchasesForTests(ApplicationDbContext db)
        {
            db.ItemDeCompras.RemoveRange(db.ItemDeCompras);
            db.Compras.RemoveRange(db.Compras);
            db.SaveChanges();
        }

        public static IList<Comprar> GetPurchases(int index, int numOfPurchases)
        {

            Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
            var allPurchases = new List<Comprar>();
            Comprar purchase;
            Disco disco;
            ItemDeCompra purchaseItem;
            int quantity = 2;

            for (int i = 1; i < 3; i++)
            {
                disco = UtilitiesForDiscos.GetDiscos(i - 1, 1).First();
                disco.CantidadCompra = disco.CantidadCompra - quantity;
                purchase = new Comprar
                {
                    CompraId = i,
                    Cliente = customer,
                    ClienteId = customer.Id,
                    Direccion = "Avd. España s/n",
                    MetodoDePago = GetPaymentMethod(i - 1, 1).First(),
                    FechaCompra = System.DateTime.Now,
                    PrecioTotal = disco.PrecioDeCompra,
                    ComprarItem = new List<ItemDeCompra>()
                };
                purchaseItem = new ItemDeCompra
                {
                    Id = i,
                    CantidadCompra = quantity,
                    Disco = disco,
                    DiscoId = disco.DiscoId,
                    Comprar = purchase,
                    CompraId = purchase.CompraId

                };
                purchase.ComprarItem.Add(purchaseItem);
                purchase.PrecioTotal = purchaseItem.CantidadCompra * purchaseItem.Disco.PrecioDeCompra;
                allPurchases.Add(purchase);

            }

            return allPurchases.GetRange(index, numOfPurchases);
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
