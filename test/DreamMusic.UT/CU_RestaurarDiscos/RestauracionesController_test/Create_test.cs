using DreamMusic.Controllers;
using DreamMusic.Data;
using DreamMusic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using DreamMusic.Models.RestauracionViewModels;
using DreamMusic.Models.DiscoViewModels;
using DreamMusic.UT.Controladores;
using System.Threading.Tasks;
using DreamMusic.UT.CU_RestaurarDiscos.RestauracionesController_test;
using DreamMusic.UT.CU_RestaurarDiscos.DiscoController_test;
using static DreamMusic.Models.RestauracionViewModels.RestauracionCreateViewModel;

namespace DreamMusic.UT.CU_RestaurarDiscos.RestauracionesController_test
{
    public class Create_test
    {

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext restaurarContext;


        public Create_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            UtilitiesForDiscos.InitializeDbDiscosForTests(context);


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new("elena@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new(user);
            restaurarContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
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
                var controller = new RestauracionesController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = restaurarContext;

                String[] ids = new string[1] { "1" };
                SelectedDiscosParaRestauracionViewModel discos = new() { IdsToAdd = ids };
                Disco expectedDisco = UtilitiesForDiscos.GetDiscos(0, 1).First();
                Administrador expectedCustomer = Utilities.GetUsers(1, 1).First() as Administrador;

                IList<RestauracionItemViewModel> expectedRestauracionItems = new RestauracionItemViewModel[1] {
                    new RestauracionItemViewModel {Quantity=0, DiscoID = expectedDisco.DiscoId, Title = expectedDisco.Titulo,
                        PriceDeRestauracion = expectedDisco.PrecioDeRestauracion, Genre = expectedDisco.Genero.Nombre} };
                RestauracionCreateViewModel expectedRestauracion = new() { RestauracionItems = expectedRestauracionItems, Name = expectedCustomer.Name, FirstSurname = expectedCustomer.Apellido1, SecondSurname = expectedCustomer.Apellido2 };

                // Act
                var result = controller.Create(discos);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                RestauracionCreateViewModel currentPurchase = viewResult.Model as RestauracionCreateViewModel;

                Assert.Equal(currentPurchase, expectedRestauracion);

            }
        }
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Get_WithoutDiscos()
        {
            using (context)
            {

                // Arrange
                var controller = new RestauracionesController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = restaurarContext;
                Administrador customer = Utilities.GetUsers(1, 1).First() as Administrador;
                SelectedDiscosParaRestauracionViewModel discos = new();

                RestauracionCreateViewModel expectedRestauracion = new()
                {
                    Name = customer.Name,
                    FirstSurname = customer.Apellido1,
                    SecondSurname = customer.Apellido2,
                    RestauracionItems = new List<RestauracionItemViewModel>()
                };


                // Act
                var result = controller.Create(discos);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                RestauracionCreateViewModel currentRestauracion = viewResult.Model as RestauracionCreateViewModel;
                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentRestauracion, expectedRestauracion);
                Assert.Equal("Tienes que seleccionar algún disco para que sea restaurado, por favor", error.ErrorMessage);
            }
        }

        public static IEnumerable<object[]> TestCasesForRestaurarCreatePost_WithErrors()
        {
            //Las siguientes dos pruebas sustituyen a los métodos indicados usando Theory. No usar los métodos Fact.
            //The following two tests are subtitutes of the indicated facts methods using Theory instead of Fact. Please, do not use the Fact methods.
            //First error: Create_Post_WithoutEnoughMoviesToBePurchased

            Disco disco = UtilitiesForDiscos.GetDiscos(0, 1).First();
            Administrador customer = Utilities.GetUsers(1, 1).First() as Administrador;
            //var payment1 = new PayPal { CorreoElectronico = customer.Email, NumTelefono = customer.PhoneNumber, Prefijo = "+34" };

            //Input values
            IList<RestauracionItemViewModel> RestauracionItemsViewModel1 = new RestauracionItemViewModel[1] { new RestauracionItemViewModel { Quantity = 12, DiscoID = disco.DiscoId, Title = disco.Titulo, Genre = disco.Genero.Nombre, PriceDeRestauracion = disco.PrecioDeRestauracion } };
            RestauracionCreateViewModel restauracion1 = new() { Name = customer.Name, FirstSurname = customer.Apellido1, SecondSurname = customer.Apellido2, RestauracionItems = RestauracionItemsViewModel1, DeliveryAddress = "Albacete", Email = customer.Email, Phone = customer.PhoneNumber, Prefix = "+34" };

            //Expected values
            IList<RestauracionItemViewModel> expectedRestauracionItemsViewModel1 = new RestauracionItemViewModel[1] { new RestauracionItemViewModel { Quantity = 12, DiscoID = disco.DiscoId, Title = disco.Titulo, Genre = disco.Genero.Nombre, PriceDeRestauracion = disco.PrecioDeRestauracion } };
            RestauracionCreateViewModel expectedRestauracionVM1 = new() { Name = customer.Name, FirstSurname = customer.Apellido1, SecondSurname = customer.Apellido2, RestauracionItems = RestauracionItemsViewModel1, DeliveryAddress = "Albacete", Email = customer.Email, Phone = customer.PhoneNumber, Prefix = "+34" };
            string expetedErrorMessage1 = "No hay suficientes discos titulados Scorpion";


            //Second error: Create_Post_WithQuantity0ForPurchase

            //Input values
            //IList<RestauracionItemViewModel> restauracionItemsViewModel2 = new RestauracionItemViewModel[1] { new RestauracionItemViewModel { Quantity = 0, DiscoID = disco.DiscoId, Title = disco.Titulo, Genre = disco.Genero.Nombre, PriceDeRestauracion = disco.PrecioDeRestauracion } };
            //RestauracionCreateViewModel restauracion2 = new() { Name = customer.Name, FirstSurname = customer.Apellido1, SecondSurname = customer.Apellido2, RestauracionItems = restauracionItemsViewModel2, DeliveryAddress = "Albacete", PayPal = payment1 };

            //expected values
            //IList<RestauracionItemViewModel> expectedPurchaseItemsViewModel2 = new RestauracionItemViewModel[1] { new RestauracionItemViewModel { Quantity = 0, DiscoID = disco.DiscoId, Title = disco.Titulo, Genre = disco.Genero.Nombre, PriceDeRestauracion = disco.PrecioDeRestauracion} };
            //RestauracionCreateViewModel expectedRestauracionVM2 = new() { Name = customer.Name, FirstSurname = customer.Apellido1, SecondSurname = customer.Apellido2, RestauracionItems = expectedPurchaseItemsViewModel2, DeliveryAddress = "Albacete", PayPal = payment1 };
            //string expetedErrorMessage2 = "Please select at least a movie to be bought or cancel your purchase";

            var allTests = new List<object[]>
            {                  //Input values                                       // expected values
                new object[] { restauracion1, expectedRestauracionVM1, expetedErrorMessage1},
                //new object[] { restauracion2, restauracionItemsViewModel2, null , payment1, expectedRestauracionVM2, expetedErrorMessage2 }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForRestaurarCreatePost_WithErrors))]
        public void Create_Post_WithErrors(RestauracionCreateViewModel restauracion, RestauracionCreateViewModel expectedRestauracionVM, string errorMessage)
        {
            using (context)
            {
                // Arrange
                var controller = new RestauracionesController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = restaurarContext;

                // Act
                var result = controller.CreatePost(restauracion);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                RestauracionCreateViewModel currentRestauracion = viewResult.Model as RestauracionCreateViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First(); ;
                Assert.Equal(expectedRestauracionVM, currentRestauracion);
                Assert.Equal(errorMessage, error.ErrorMessage);


            }

        }

        public static IEnumerable<object[]> TestCasesForPurchasesCreatePost_WithoutErrors()
        {
            //Substitución similar a la vista anteriormente.
            //Same substitution as the former two tests. 

            //Purchase with CreditCard
            Restauracion expectedRestauracion1 = UtilitiesForRestauraciones.GetRestauracion(0, 1).First();
            Administrador expectedCustomer1 = expectedRestauracion1.Administrador;
            var expectedPayment1 = expectedRestauracion1.MetodoDePago as TarjetaDeCredito;
            ItemRestauracion expectedRestauracionItem1 = expectedRestauracion1.ItemRestauracion.First();
            int expectedQuantityForRestauracion1 = UtilitiesForDiscos.GetDiscos(0, 1).First().CantidadRestauracion - expectedRestauracionItem1.CantidadRestauracion;
            IList<RestauracionItemViewModel> RestauracionItemsViewModel1 = new RestauracionItemViewModel[1] { new RestauracionItemViewModel {
                    Quantity = expectedRestauracionItem1.CantidadRestauracion, DiscoID = expectedRestauracionItem1.DiscoId,
                    Title=expectedRestauracionItem1.Disco.Titulo, Genre=expectedRestauracionItem1.Disco.Genero.Nombre,
                    PriceDeRestauracion=expectedRestauracionItem1.Disco.PrecioDeRestauracion} };

            RestauracionCreateViewModel restauracion1 = new()
            {
                Name = expectedCustomer1.Name,
                FirstSurname = expectedCustomer1.Apellido1,
                SecondSurname = expectedCustomer1.Apellido2,
                RestauracionItems = RestauracionItemsViewModel1,
                DeliveryAddress = expectedRestauracion1.Direccion,
                PaymentMethod = "CreditCard",
                CreditCardNumber = expectedPayment1.NumeroTarjeta,
                CCV = expectedPayment1.CCV,
                ExpirationDate = expectedPayment1.FechaCaducidad
            };

            //Payment with Paypal
            Restauracion expectedRestauracion2 = UtilitiesForRestauraciones.GetRestauracion(1, 1).First();
            expectedRestauracion2.RestauracionId = 1;
            expectedRestauracion2.ItemRestauracion.First().Id = 1;
            expectedRestauracion2.ItemRestauracion.First().RestauracionId = 1;
            ItemRestauracion expectedRestauracionItem2 = expectedRestauracion2.ItemRestauracion.First();
            int expectedQuantityForRestauracion2 = UtilitiesForDiscos.GetDiscos(1, 1).First().CantidadRestauracion - expectedRestauracionItem2.CantidadRestauracion;
            var expectedPayment2 = expectedRestauracion2.MetodoDePago as PayPal;
            expectedPayment2.ID = 1;
            Administrador expectedCustomer2 = expectedRestauracion2.Administrador;

            IList<RestauracionItemViewModel> RestauracionItemsViewModel2 = new RestauracionItemViewModel[1] { new RestauracionItemViewModel {
                    Quantity = expectedRestauracionItem2.CantidadRestauracion, DiscoID = expectedRestauracionItem2.DiscoId,
                    Title=expectedRestauracionItem2.Disco.Titulo, Genre=expectedRestauracionItem2.Disco.Genero.Nombre,
                    PriceDeRestauracion=expectedRestauracionItem2.Disco.PrecioDeRestauracion} };
            RestauracionCreateViewModel restauracion2 = new()
            {
                Name = expectedCustomer2.Name,
                FirstSurname = expectedCustomer2.Apellido1,
                SecondSurname = expectedCustomer2.Apellido2,
                RestauracionItems = RestauracionItemsViewModel2,
                DeliveryAddress = expectedRestauracion2.Direccion,
                PaymentMethod = "PayPal",
                Phone = expectedPayment2.NumTelefono,
                Prefix = expectedPayment2.Prefijo,
                Email = expectedPayment2.CorreoElectronico
            };


            var allTests = new List<object[]>
            {                  //Input values                                              // expected values
                new object[] { restauracion1, expectedRestauracion1, expectedQuantityForRestauracion1},
                new object[] { restauracion2, expectedRestauracion2, expectedQuantityForRestauracion2 }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForPurchasesCreatePost_WithoutErrors))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Post_WithoutErrors(RestauracionCreateViewModel restauracion, Restauracion expectedRestauracion, int expectedQuantityForRestauracion)
        {
            using (context)
            {

                // Arrange
                var controller = new RestauracionesController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = restaurarContext;

                // Act
                var result = controller.CreatePost(restauracion);

                //Assert
                //we should check it is redirected to details
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                Assert.Equal("Details", viewResult.ActionName);

                //we should check the purchase has been created in the database
                var actualRestauracion = context.restauracion.Include(p => p.ItemRestauracion).
                                    FirstOrDefault(p => p.RestauracionId == expectedRestauracion.RestauracionId);
                Assert.Equal(expectedRestauracion, actualRestauracion);

                //And that the quantity for purchase of each associated movie has been modified accordingly 
                Assert.Equal(expectedQuantityForRestauracion,
                context.Discos.First(m => m.DiscoId == expectedRestauracion.ItemRestauracion.First().DiscoId).CantidadRestauracion);


            }

        }



    }
}