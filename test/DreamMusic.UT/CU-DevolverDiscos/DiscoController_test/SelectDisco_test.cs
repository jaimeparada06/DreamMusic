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

namespace DreamMusic.UT.CU_DevolverDiscos.DiscoController_test
{
    public class SelectDisco_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext devolverContext;
        public SelectDisco_test()
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
            devolverContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            devolverContext.User = identity;
        }

        public static IEnumerable<object[]> TestCasesForSelectDiscosParaDevolucion_get()
        {
            var allTests = new List<object[]>
            {
                new object[] { UtilitiesForDiscos.GetDiscos(0,3), UtilitiesForDiscos.GetGeneros(0,3), null, null,null,null,null,null},
                new object[] { UtilitiesForDiscos.GetDiscos(0,1), UtilitiesForDiscos.GetGeneros(0,3), "Scorpion", null,null,null,null,null},
                new object[] { UtilitiesForDiscos.GetDiscos(0,2), UtilitiesForDiscos.GetGeneros(0,3), null, "Rap",null,null,null,null},
                new object[]{ UtilitiesForDiscos.GetDiscos(2,1),  UtilitiesForDiscos.GetGeneros(0,3), null,null,"Michael Jackson",null, null, null},
                new object[]{ UtilitiesForDiscos.GetDiscos(0,1),  UtilitiesForDiscos.GetGeneros(0,3), null,null,null,2020, null, null},
                new object[]{ UtilitiesForDiscos.GetDiscos(0,1),  UtilitiesForDiscos.GetGeneros(0,3), null,null,null,null,08, null},
                new object[]{ UtilitiesForDiscos.GetDiscos(0,1),  UtilitiesForDiscos.GetGeneros(0,3), null,null,null,null, null,18}

            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForSelectDiscosParaDevolucion_get))]
        public async Task SelectDiscosParaDevolver_Get(List<Disco> expectedDiscos, List<Genero> expectedGeneros, string filterTitulo, string filterGenero,string filterArtista, int? filterAño, int? filterMes, int? filterDia)
        {
            using (context)
            {
                // Arrange

                var controller = new DiscosController(context);
                controller.ControllerContext.HttpContext = devolverContext;
                var expectedGenerosSelectList = new SelectList(expectedGeneros.Select(g => g.Nombre).ToList());
                // Act

                var result = controller.SelectDiscosParaDevolucion(filterTitulo, filterGenero,filterArtista,filterAño,filterMes, filterDia);
                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectDiscosParaDevolucionViewModel model = viewResult.Model as SelectDiscosParaDevolucionViewModel;
                
                // Check that both collections (expected and result returned) have the same elements with the same name
                // You must implement Equals in Discos, otherwise Assert will fail
                
                Assert.Equal(expectedDiscos, model.Discos);
                // We need to use Comparer to compare both collections
                Assert.Equal(expectedGenerosSelectList, model.Generos, Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                
            }



        }

        
        [Fact]
        public void SelectDiscosParaDevolucion_Post_DiscosNoSeleccionados()
        {
            using (context)
            {

                // Arrange
                var controller = new DiscosController(context);
                controller.ControllerContext.HttpContext = devolverContext;
                var expectedGeneros = new SelectList(UtilitiesForDiscos.GetGeneros(0, 3).Select(g => g.Nombre).ToList());
                var expectedDiscos = UtilitiesForDiscos.GetDiscos(0, 3);
                var expectedCompras = UtilitiesForDiscos.GetCompras(0, 3);
                var expectedItemDeCompras = UtilitiesForDiscos.GetItemDeCompras(0, 3);


                SelectedDiscosParaDevolucionViewModel selected = new SelectedDiscosParaDevolucionViewModel { IdsToAdd = null };

                // Act
                var result = controller.SelectDiscosParaDevolucion(selected);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectDiscosParaDevolucionViewModel model = viewResult.Model as SelectDiscosParaDevolucionViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                Assert.Equal(expectedGeneros, model.Generos, Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                Assert.Equal(expectedDiscos, model.Discos);
                
                //Assert.Equal(expectedItemDeCompras, model.ItemDeCompras);
                //Assert.Equal(expectedCompras, model.Compras);
                

            }
        }

        [Fact]
        public void SelectDiscosParaDevolucion_Post_DiscosSeleccionados()
        {
            using (context)
            {

                // Arrange
                var controller = new DiscosController(context);
                controller.ControllerContext.HttpContext = devolverContext;

                String[] ids = new string[1] { "1" };
                SelectedDiscosParaDevolucionViewModel discos = new SelectedDiscosParaDevolucionViewModel { IdsToAdd = ids };

                // Act
                var result = controller.SelectDiscosParaDevolucion(discos);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var currentDiscos = viewResult.RouteValues.Values.First();
                Assert.Equal(discos.IdsToAdd, currentDiscos);

            }
        }
        


    }
}
