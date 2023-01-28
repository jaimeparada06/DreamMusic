using DreamMusic.Controllers;
using DreamMusic.Data;
using DreamMusic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using DreamMusic.Models.DevolucionViewModels;
using DreamMusic.Models.DiscoViewModels;
using DreamMusic.UT.Controladores;
using System.Threading.Tasks;
using DreamMusic.UT.CU_DevolverDiscos.DevolucionesController_test;
using DreamMusic.UT.CU_DevolverDiscos.DiscoController_test;

namespace DreamMusic.UT.CU_DevolverDiscos.DevolucionesController_test
{
    public class Create_test
    {

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext devolverContext;


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
            devolverContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
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
                var controller = new DevolucionesController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = devolverContext;

                String[] ids = new string[1] { "1" };
                SelectedDiscosParaDevolucionViewModel discos = new() { IdsToAdd = ids };
                Disco expectedDisco = UtilitiesForDiscos.GetDiscos(0, 1).First();
                Cliente expectedCustomer = Utilities.GetUsers(0, 1).First() as Cliente;

                IList<DevolucionItemViewModel> expectedDevolucionItems = new DevolucionItemViewModel[1] {
                    new DevolucionItemViewModel {Quantity=0, DiscoID = expectedDisco.DiscoId, Title = expectedDisco.Titulo,
                        PriceDeDevolucion = expectedDisco.PrecioDeDevolucion, Genre = expectedDisco.Genero.Nombre} };
                DevolucionCreateViewModel expectedDevolucion = new() { DevolucionItems = expectedDevolucionItems, Name = expectedCustomer.Name, FirstSurname = expectedCustomer.Apellido1, SecondSurname = expectedCustomer.Apellido2 };

                // Act
                var result = controller.Create(discos);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                DevolucionCreateViewModel currentPurchase = viewResult.Model as DevolucionCreateViewModel;

                Assert.Equal(currentPurchase, expectedDevolucion);

            }
        }
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Get_WithoutDiscos()
        {
            using (context)
            {

                // Arrange
                var controller = new DevolucionesController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = devolverContext;
                Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
                SelectedDiscosParaDevolucionViewModel discos = new();

                DevolucionCreateViewModel expectedDevolucion = new()
                {
                    Name = customer.Name,
                    FirstSurname = customer.Apellido1,
                    SecondSurname = customer.Apellido2,
                    DevolucionItems = new List<DevolucionItemViewModel>()
                };


                // Act
                var result = controller.Create(discos);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                DevolucionCreateViewModel currentDevolucion = viewResult.Model as DevolucionCreateViewModel;
                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentDevolucion, expectedDevolucion);
                Assert.Equal("Tienes que seleccionar algún disco para que sea devuelto, por favor", error.ErrorMessage);
            }
        }

        public static IEnumerable<object[]> TestCasesForDevolverCreatePost_WithErrors()
        {
            //Las siguientes dos pruebas sustituyen a los métodos indicados usando Theory. No usar los métodos Fact.
            //The following two tests are subtitutes of the indicated facts methods using Theory instead of Fact. Please, do not use the Fact methods.
            //First error: Create_Post_WithoutEnoughMoviesToBePurchased

            Disco disco = UtilitiesForDiscos.GetDiscos(0, 1).First();
            Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
            //var payment1 = new PayPal { CorreoElectronico = customer.Email, NumTelefono = customer.PhoneNumber, Prefijo = "+34" };

            //Input values
            IList<DevolucionItemViewModel> devolucionItemsViewModel1 = new DevolucionItemViewModel[1] { new DevolucionItemViewModel { Quantity = 12, DiscoID = disco.DiscoId, Title = disco.Titulo, Genre = disco.Genero.Nombre, PriceDeDevolucion = disco.PrecioDeDevolucion } };
            DevolucionCreateViewModel devolucion1 = new() { Name = customer.Name, FirstSurname = customer.Apellido1, SecondSurname = customer.Apellido2, DevolucionItems = devolucionItemsViewModel1, DeliveryAddress = "Albacete", Email = customer.Email, Phone = customer.PhoneNumber, Prefix = "+34" };

            //Expected values
            IList<DevolucionItemViewModel> expectedDevolucionItemsViewModel1 = new DevolucionItemViewModel[1] { new DevolucionItemViewModel { Quantity = 12, DiscoID = disco.DiscoId, Title = disco.Titulo, Genre = disco.Genero.Nombre, PriceDeDevolucion = disco.PrecioDeDevolucion } };
            DevolucionCreateViewModel expectedDevolucionVM1 = new() { Name = customer.Name, FirstSurname = customer.Apellido1, SecondSurname = customer.Apellido2, DevolucionItems = devolucionItemsViewModel1, DeliveryAddress = "Albacete", Email = customer.Email, Phone = customer.PhoneNumber, Prefix = "+34" };
            string expetedErrorMessage1 = "No hay suficientes discos titulados Scorpion";


            //Second error: Create_Post_WithQuantity0ForPurchase

            //Input values
            //IList<DevolucionItemViewModel> devolucionItemsViewModel2 = new DevolucionItemViewModel[1] { new DevolucionItemViewModel { Quantity = 0, DiscoID = disco.DiscoId, Title = disco.Titulo, Genre = disco.Genero.Nombre, PriceDeDevolucion = disco.PrecioDeDevolucion } };
            //DevolucionCreateViewModel devolucion2 = new() { Name = customer.Name, FirstSurname = customer.Apellido1, SecondSurname = customer.Apellido2, DevolucionItems = devolucionItemsViewModel2, DeliveryAddress = "Albacete", PayPal = payment1 };

            //expected values
            //IList<DevolucionItemViewModel> expectedPurchaseItemsViewModel2 = new DevolucionItemViewModel[1] { new DevolucionItemViewModel { Quantity = 0, DiscoID = disco.DiscoId, Title = disco.Titulo, Genre = disco.Genero.Nombre, PriceDeDevolucion = disco.PrecioDeDevolucion} };
            //DevolucionCreateViewModel expectedDevolucionVM2 = new() { Name = customer.Name, FirstSurname = customer.Apellido1, SecondSurname = customer.Apellido2, DevolucionItems = expectedPurchaseItemsViewModel2, DeliveryAddress = "Albacete", PayPal = payment1 };
            //string expetedErrorMessage2 = "Please select at least a movie to be bought or cancel your purchase";

            var allTests = new List<object[]>
            {                  //Input values                                       // expected values
                new object[] { devolucion1, expectedDevolucionVM1, expetedErrorMessage1},
                //new object[] { devolucion2, devolucionItemsViewModel2, null , payment1, expectedDevolucionVM2, expetedErrorMessage2 }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForDevolverCreatePost_WithErrors))]
        public void Create_Post_WithErrors(DevolucionCreateViewModel devolucion, DevolucionCreateViewModel expectedDevolucionVM, string errorMessage)
        {
            using (context)
            {
                // Arrange
                var controller = new DevolucionesController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = devolverContext;

                // Act
                var result = controller.CreatePost(devolucion);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                DevolucionCreateViewModel currentDevolucion = viewResult.Model as DevolucionCreateViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First(); ;
                Assert.Equal(expectedDevolucionVM, currentDevolucion);
                Assert.Equal(errorMessage, error.ErrorMessage);


            }

        }

        public static IEnumerable<object[]> TestCasesForPurchasesCreatePost_WithoutErrors()
        {
            //Substitución similar a la vista anteriormente.
            //Same substitution as the former two tests. 
         
            //Purchase with CreditCard
            Devolucion expectedDevolucion1 = UtilitiesForDevoluciones.GetDevolucion(0, 1).First();
            Cliente expectedCustomer1 = expectedDevolucion1.Cliente;
            var expectedPayment1 = expectedDevolucion1.MetodoDePago as TarjetaDeCredito;
            ItemDevolucion expectedDevolucionItem1 = expectedDevolucion1.ItemDevolucions.First();
            int expectedQuantityForDevolucion1 = UtilitiesForDiscos.GetDiscos(0, 1).First().CantidadDevolucion - expectedDevolucionItem1.CantidadDevolucion;
            IList<DevolucionItemViewModel> devolucionItemsViewModel1 = new DevolucionItemViewModel[1] { new DevolucionItemViewModel {
                    Quantity = expectedDevolucionItem1.CantidadDevolucion, DiscoID = expectedDevolucionItem1.DiscoId,
                    Title=expectedDevolucionItem1.Disco.Titulo, Genre=expectedDevolucionItem1.Disco.Genero.Nombre,
                    PriceDeDevolucion=expectedDevolucionItem1.Disco.PrecioDeDevolucion} };
           
            DevolucionCreateViewModel devolucion1 = new()
            {
                Name = expectedCustomer1.Name,
                FirstSurname = expectedCustomer1.Apellido1,
                SecondSurname = expectedCustomer1.Apellido2,
                DevolucionItems = devolucionItemsViewModel1,
                DeliveryAddress = expectedDevolucion1.Direccion,
                PaymentMethod = "CreditCard",
                CreditCardNumber = expectedPayment1.NumeroTarjeta,
                CCV = expectedPayment1.CCV,
                ExpirationDate = expectedPayment1.FechaCaducidad
            };

            //Payment with Paypal
            Devolucion expectedDevolucion2 = UtilitiesForDevoluciones.GetDevolucion(1, 1).First();
            expectedDevolucion2.DevolucionId = 1;
            expectedDevolucion2.ItemDevolucions.First().Id = 1;
            expectedDevolucion2.ItemDevolucions.First().DevolucionId = 1;
            ItemDevolucion expectedDevolucionItem2 = expectedDevolucion2.ItemDevolucions.First();
            int expectedQuantityForDevolucion2 = UtilitiesForDiscos.GetDiscos(1, 1).First().CantidadDevolucion - expectedDevolucionItem2.CantidadDevolucion;
            var expectedPayment2 = expectedDevolucion2.MetodoDePago as PayPal;
            expectedPayment2.ID = 1;
            Cliente expectedCustomer2 = expectedDevolucion2.Cliente;

            IList<DevolucionItemViewModel> devolucionItemsViewModel2 = new DevolucionItemViewModel[1] { new DevolucionItemViewModel {
                    Quantity = expectedDevolucionItem2.CantidadDevolucion, DiscoID = expectedDevolucionItem2.DiscoId,
                    Title=expectedDevolucionItem2.Disco.Titulo, Genre=expectedDevolucionItem2.Disco.Genero.Nombre,
                    PriceDeDevolucion=expectedDevolucionItem2.Disco.PrecioDeDevolucion} };
            DevolucionCreateViewModel devolucion2 = new()
            {
                Name = expectedCustomer2.Name,
                FirstSurname = expectedCustomer2.Apellido1,
                SecondSurname = expectedCustomer2.Apellido2,
                DevolucionItems = devolucionItemsViewModel2,
                DeliveryAddress = expectedDevolucion2.Direccion,
                PaymentMethod = "PayPal",
                Phone = expectedPayment2.NumTelefono,
                Prefix = expectedPayment2.Prefijo,
                Email = expectedPayment2.CorreoElectronico
            };


            var allTests = new List<object[]>
            {                  //Input values                                              // expected values
                new object[] { devolucion1, expectedDevolucion1, expectedQuantityForDevolucion1},
                new object[] { devolucion2,expectedDevolucion2, expectedQuantityForDevolucion2}
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForPurchasesCreatePost_WithoutErrors))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Post_WithoutErrors(DevolucionCreateViewModel devolucion, Devolucion expectedDevolucion, int expectedQuantityForDevolucion)
        {
            using (context)
            {

                // Arrange
                var controller = new DevolucionesController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = devolverContext;

                // Act
                var result = controller.CreatePost(devolucion);

                //Assert
                //we should check it is redirected to details
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                Assert.Equal("Details", viewResult.ActionName);

                //we should check the purchase has been created in the database
                var actualDevolucion = context.Devoluciones.Include(p => p.ItemDevolucions).
                                    FirstOrDefault(p => p.DevolucionId == expectedDevolucion.DevolucionId);
                Assert.Equal(expectedDevolucion, actualDevolucion);

                //And that the quantity for purchase of each associated movie has been modified accordingly 
                Assert.Equal(expectedQuantityForDevolucion,
                context.Discos.First(m => m.DiscoId == expectedDevolucion.ItemDevolucions.First().DiscoId).CantidadDevolucion);


            }

        }



    }
}
