using DreamMusic.Controllers;
using DreamMusic.Data;
using DreamMusic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using DreamMusic.Models.CompraViewModels;
using DreamMusic.Models.DiscoViewModels;
using DreamMusic.UT.Controladores;
using System.Threading.Tasks;
using DreamMusic.UT.CU_ComprarDiscos.ComprasController_test;


namespace DreamMusic.UT.CU_ComprarDiscos.ComprasController_test
{
    public class Create_test
    {

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext purchaseContext;


        public Create_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            UtilitiesForDiscos.InitializeDbDiscosForTests(context);


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new(user);
            purchaseContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
            {
                User = identity
            };

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Get_WithSelectedDiscos()
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = purchaseContext;

                String[] ids = new string[1] { "1" };
                SelectedDiscosParaComprarViewModel discos = new() { IdsToAdd = ids };
                Disco expectedDisco = UtilitiesForDiscos.GetDiscos(0, 1).First();
                Cliente expectedCustomer = Utilities.GetUsers(0, 1).First() as Cliente;

                IList<CompraItemViewModel> expectedPurchaseItems = new CompraItemViewModel[1] {
                    new CompraItemViewModel {Quantity=0, DiscoID = expectedDisco.DiscoId, Title = expectedDisco.Titulo,
                        PriceForCompra = expectedDisco.PrecioDeCompra, Genre = expectedDisco.Genero.Nombre} };
                CompraCreateViewModel expectedPurchase = new() { CompraItems = expectedPurchaseItems, Name = expectedCustomer.Name, FirstSurname = expectedCustomer.Apellido1, SecondSurname = expectedCustomer.Apellido2 };

                // Act
                var result = controller.Create(discos);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                CompraCreateViewModel currentPurchase = viewResult.Model as CompraCreateViewModel;

                Assert.Equal(currentPurchase, expectedPurchase);

            }
        }
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Get_WithoutDisco()
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = purchaseContext;
                Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
                SelectedDiscosParaComprarViewModel discos = new ();

                CompraCreateViewModel expectedPurchase = new ()
                {
                    Name = customer.Name,
                    FirstSurname = customer.Apellido1,
                    SecondSurname = customer.Apellido2,
                    CompraItems = new List<CompraItemViewModel>()
                };


                // Act
                var result = controller.Create(discos);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                CompraCreateViewModel currentPurchase = viewResult.Model as CompraCreateViewModel;
                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentPurchase, expectedPurchase);
                Assert.Equal("Tienes que seleccionar algún disco para que sea comprado, por favor", error.ErrorMessage);
            }
        }

        public static IEnumerable<object[]> TestCasesForPurchasesCreatePost_WithErrors()
        {
            //Las siguientes dos pruebas sustituyen a los métodos indicados usando Theory. No usar los métodos Fact.
            //The following two tests are subtitutes of the indicated facts methods using Theory instead of Fact. Please, do not use the Fact methods.
            //First error: Create_Post_WithoutEnoughMoviesToBePurchased

            Disco disco = UtilitiesForDiscos.GetDiscos(0, 1).First();
            Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
            //var payment1 = new PayPal { CorreoElectronico = customer.Email, NumTelefono = customer.PhoneNumber, Prefijo = "+34" };

            //Input values
            IList<CompraItemViewModel> purchaseItemsViewModel1 = new CompraItemViewModel[1] { new CompraItemViewModel { Quantity = 12, DiscoID = disco.DiscoId, Title = disco.Titulo, Genre = disco.Genero.Nombre, PriceForCompra = disco.PrecioDeCompra } };
            CompraCreateViewModel purchase1 = new () { Name = customer.Name, FirstSurname = customer.Apellido1, SecondSurname = customer.Apellido2, CompraItems = purchaseItemsViewModel1, DeliveryAddress = "Albacete", Email = customer.Email, Phone = customer.PhoneNumber, Prefix="+34"};

            //Expected values
            IList<CompraItemViewModel> expectedPurchaseItemsViewModel1 = new CompraItemViewModel[1] { new CompraItemViewModel { Quantity = 12, DiscoID = disco.DiscoId, Title = disco.Titulo, Genre = disco.Genero.Nombre, PriceForCompra = disco.PrecioDeCompra } };
            CompraCreateViewModel expectedPurchaseVM1 = new () { Name = customer.Name, FirstSurname = customer.Apellido1, SecondSurname = customer.Apellido2, CompraItems = expectedPurchaseItemsViewModel1, DeliveryAddress = "Albacete", Email = customer.Email, Phone = customer.PhoneNumber, Prefix = "+34" };
            string expetedErrorMessage1 = "No hay disponibles los discos titulados Scorpion";


            //Second error: Create_Post_WithQuantity0ForPurchase

            //Input values
           // IList<CompraItemViewModel> purchaseItemsViewModel2 = new CompraItemViewModel[1] { new CompraItemViewModel { Quantity = 0, DiscoID = disco.DiscoId, Title = disco.Titulo, Genre = disco.Genero.Nombre, PriceForCompra = disco.PrecioDeCompra } };
            //CompraCreateViewModel purchase2 = new () { Name = customer.Name, FirstSurname = customer.Apellido1, SecondSurname = customer.Apellido2, CompraItems = purchaseItemsViewModel2, DeliveryAddress = "Albacete", Email = customer.Email, Phone = customer.PhoneNumber, Prefix = "+34" };

            //expected values
            //IList<CompraItemViewModel> expectedPurchaseItemsViewModel2 = new CompraItemViewModel[1] { new CompraItemViewModel { Quantity = 0, DiscoID = disco.DiscoId, Title = disco.Titulo, Genre = disco.Genero.Nombre, PriceForCompra = disco.PrecioDeCompra } };
            //CompraCreateViewModel expectedPurchaseVM2 = new () { Name = customer.Name, FirstSurname = customer.Apellido1, SecondSurname = customer.Apellido2, CompraItems = expectedPurchaseItemsViewModel2, DeliveryAddress = "Albacete", Email = customer.Email, Phone = customer.PhoneNumber, Prefix = "+34" };
            //string expetedErrorMessage2 = "Please select at least a movie to be bought or cancel your purchase";

            var allTests = new List<object[]>
            {                  //Input values                                       // expected values
                new object[] { purchase1, expectedPurchaseVM1, expetedErrorMessage1 },
                //new object[] { purchase2, purchaseItemsViewModel2, null , payment1, expectedPurchaseVM2, expetedErrorMessage2 }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForPurchasesCreatePost_WithErrors))]
        public void Create_Post_WithErrors(CompraCreateViewModel purchase, CompraCreateViewModel expectedPurchaseVM, string errorMessage)
        {
            using (context)
            {
                // Arrange
                var controller = new ComprasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = purchaseContext;

                // Act
                var result = controller.CreatePost(purchase);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                CompraCreateViewModel currentPurchase = viewResult.Model as CompraCreateViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First(); ;
                Assert.Equal(expectedPurchaseVM, currentPurchase);
                Assert.Equal(errorMessage, error.ErrorMessage);


            }

        }

        public static IEnumerable<object[]> TestCasesForPurchasesCreatePost_WithoutErrors()
        {
            //Substitución similar a la vista anteriormente.
            //Same substitution as the former two tests.

            //Purchase with CreditCard
            Comprar expectedPurchase1 = UtilitiesForCompras.GetPurchases(0, 1).First();
            Cliente expectedCustomer1 = expectedPurchase1.Cliente;
            var expectedPayment1 = expectedPurchase1.MetodoDePago as TarjetaDeCredito;
            ItemDeCompra expectedPurchaseItem1 = expectedPurchase1.ComprarItem.First();
            int expectedQuantityForPurchase1 = UtilitiesForDiscos.GetDiscos(0, 1).First().CantidadCompra - expectedPurchaseItem1.CantidadCompra;
            IList<CompraItemViewModel> purchaseItemsViewModel1 = new CompraItemViewModel[1] { new CompraItemViewModel {
                    Quantity = expectedPurchaseItem1.CantidadCompra, DiscoID = expectedPurchaseItem1.DiscoId,
                    Title=expectedPurchaseItem1.Disco.Titulo, Genre=expectedPurchaseItem1.Disco.Genero.Nombre,
                    PriceForCompra=expectedPurchaseItem1.Disco.PrecioDeCompra} };
            CompraCreateViewModel purchase1 = new()
            {
                Name = expectedCustomer1.Name,
                FirstSurname = expectedCustomer1.Apellido1,
                SecondSurname = expectedCustomer1.Apellido2,
                CompraItems = purchaseItemsViewModel1,
                DeliveryAddress = expectedPurchase1.Direccion,
                PaymentMethod = "CreditCard",
                CreditCardNumber = expectedPayment1.NumeroTarjeta,
                CCV = expectedPayment1.CCV,
                ExpirationDate = expectedPayment1.FechaCaducidad

            };

            //Payment with Paypal
            Comprar expectedPurchase2 = UtilitiesForCompras.GetPurchases(1, 1).First();
            expectedPurchase2.CompraId = 1;
            expectedPurchase2.ComprarItem.First().Id = 1;
            expectedPurchase2.ComprarItem.First().CompraId = 1;
            ItemDeCompra expectedPurchaseItem2 = expectedPurchase2.ComprarItem.First();
            int expectedQuantityForPurchase2 = UtilitiesForDiscos.GetDiscos(1, 1).First().CantidadCompra - expectedPurchaseItem2.CantidadCompra;
            var expectedPayment2 = expectedPurchase2.MetodoDePago as PayPal;
            expectedPayment2.ID = 1;
            Cliente expectedCustomer2 = expectedPurchase2.Cliente;

            IList<CompraItemViewModel> purchaseItemsViewModel2 = new CompraItemViewModel[1] { new CompraItemViewModel {
                    Quantity = expectedPurchaseItem2.CantidadCompra, DiscoID = expectedPurchaseItem2.DiscoId,
                    Title=expectedPurchaseItem2.Disco.Titulo, Genre=expectedPurchaseItem2.Disco.Genero.Nombre,
                    PriceForCompra=expectedPurchaseItem2.Disco.PrecioDeCompra} };
            CompraCreateViewModel purchase2 = new()
            {
                Name = expectedCustomer2.Name,
                FirstSurname = expectedCustomer2.Apellido1,
                SecondSurname = expectedCustomer2.Apellido2,
                CompraItems = purchaseItemsViewModel2,
                DeliveryAddress = expectedPurchase2.Direccion,
                PaymentMethod = "PayPal",
                Phone = expectedPayment2.NumTelefono,
                Prefix = expectedPayment2.Prefijo,
                Email = expectedPayment2.CorreoElectronico
            };

            var allTests = new List<object[]>
            {                  //Input values   // expected values
                new object[] { purchase1,  expectedPurchase1, expectedQuantityForPurchase1},
                new object[] { purchase2,  expectedPurchase2, expectedQuantityForPurchase2}
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForPurchasesCreatePost_WithoutErrors))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Post_WithoutErrors(CompraCreateViewModel purchase, Comprar expectedPurchase, int expectedQuantityForPurchase)
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = purchaseContext;

                // Act
                var result = controller.CreatePost(purchase);
                //Assert
                //we should check it is redirected to details
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                Assert.Equal("Details", viewResult.ActionName);


                //we should check the purchase has been created in the database
                var actualPurchase = context.Compras.Include(p => p.ComprarItem).
                                   FirstOrDefault(p => p.CompraId == expectedPurchase.CompraId);
                Assert.Equal(expectedPurchase, actualPurchase);

                //And that the quantity for purchase of each associated movie has been modified accordingly 
                Assert.Equal(expectedQuantityForPurchase,
                    context.Discos.First(m => m.DiscoId == expectedPurchase.ComprarItem.First().DiscoId).CantidadCompra);


            }

        }



    }
}
