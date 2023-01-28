using DreamMusic.Controllers;
using DreamMusic.Data;
using DreamMusic.Models;
using DreamMusic.Models.DiscoViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DreamMusic.UT;
using DreamMusic.UT.Controladores;

namespace DreamMusic.UT.CU_RestaurarDiscos.DiscoController_test
{
    public class SelectDiscos_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext restaurarContext;
        public SelectDiscos_test()
        {
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();
            // Seed the database with test data.
            // Añade datos a la base de datos temporal, InMemory
            UtilitiesForDiscos.InitializeDbDiscosForTests(context);

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            restaurarContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            restaurarContext.User = identity;
        }

        public static IEnumerable<object[]> TestCasesForSelectDiscosParaRestauracion_get()
        {
            var allTests = new List<object[]>
            {
                new object[] { UtilitiesForDiscos.GetDiscos(0,4), null, null,null, null,null,null},
                new object[] { UtilitiesForDiscos.GetDiscos(0,1), "Scorpion", null,null,null,null,null},
                new object[] { UtilitiesForDiscos.GetDiscos(0,2), null, 2020,null ,null,null,null},
                new object[] { UtilitiesForDiscos.GetDiscos(2,1), null,null,12,null,null,null},
                new object[] { UtilitiesForDiscos.GetDiscos(3,1), null,null,null,18,null,null},
                new object[] { UtilitiesForDiscos.GetDiscos(2,3), null,null,null,null,"Gregorio",null},
                new object[] { UtilitiesForDiscos.GetDiscos(2,1), null,null,null,null,null,"Diaz"},
            };
            return allTests;
        }
        /*
        [Theory]
        [MemberData(nameof(TestCasesForSelectDiscosParaRestauracion_get))]
        public async Task SelectDiscosParaRestauracion_Get(List<Disco> expectedDiscos, string filterTitulo, int? filterAño, int? filterMes, int? filterDia, string filterNombre, string filterApellido)
        {
            using (context)
            {

                // Arrange
                var controller = new DiscosController(context);
                controller.ControllerContext.HttpContext = restaurarContext;

                // Act
                var result = controller.SelectDiscosParaRestauracion(filterTitulo, filterAño, filterMes, filterDia, filterNombre, filterApellido);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectDiscosParaRestauracionViewModel model = viewResult.Model as SelectDiscosParaRestauracionViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
              
                // You must implement Equals in Discos, otherwise Assert will fail
                
                Assert.Equal(expectedDiscos, model.Discos);
            }
    
        }
        */

        [Fact]
        public void SelectDiscosParaComprar_Post_DiscosNoSeleccionados()
        {
            using (context)
            {

                // Arrange
                var controller = new DiscosController(context);
                controller.ControllerContext.HttpContext = restaurarContext;
                var expectedDiscos = UtilitiesForDiscos.GetDiscos(0, 4);

                SelectedDiscosParaRestauracionViewModel selected = new SelectedDiscosParaRestauracionViewModel { IdsToAdd = null };

                // Act
                var result = controller.SelectDiscosParaRestauracion(selected);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectDiscosParaRestauracionViewModel model = viewResult.Model as SelectDiscosParaRestauracionViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                /*
                Assert.Equal(expectedDiscos, model.Discos);
                */
            }
        }

        [Fact]
        public void SelectDiscosParaRestauracion_Post_DiscosSeleccionados()
        {
            using (context)
            {

                // Arrange
                var controller = new DiscosController(context);
                controller.ControllerContext.HttpContext = restaurarContext;

                String[] ids = new string[1] { "1" };
                SelectedDiscosParaRestauracionViewModel discos = new SelectedDiscosParaRestauracionViewModel { IdsToAdd = ids };

                // Act
                var result = controller.SelectDiscosParaRestauracion(discos);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var currentDiscos = viewResult.RouteValues.Values.First();
                Assert.Equal(discos.IdsToAdd, currentDiscos);

            }
        }


    }
}

