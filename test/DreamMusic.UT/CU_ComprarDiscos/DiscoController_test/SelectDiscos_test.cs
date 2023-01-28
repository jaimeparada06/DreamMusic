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

namespace DreamMusic.UT.CU_ComprarDiscos.ComprasController_test
{
    public class SelectDiscos_test
    {

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext purchaseContext;
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
            purchaseContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            purchaseContext.User = identity;
        }

        public static IEnumerable<object[]> TestCasesForSelectDiscosParaComprar_get()
        {
            var allTests = new List<object[]>
            {
                new object[] { UtilitiesForDiscos.GetDiscos(0,4), UtilitiesForDiscos.GetGeneros(0,4), null, null,null},
                new object[] { UtilitiesForDiscos.GetDiscos(0,1), UtilitiesForDiscos.GetGeneros(0,4), "Scorpion", null,null},
                new object[] { UtilitiesForDiscos.GetDiscos(0,2), UtilitiesForDiscos.GetGeneros(0,4), null, "Rap",null},
                new object[] { UtilitiesForDiscos.GetDiscos(2,1), UtilitiesForDiscos.GetGeneros(0,4), null, null, "Queen"},
            };
            return allTests;
        }
 
        [Theory]
        [MemberData(nameof(TestCasesForSelectDiscosParaComprar_get))]
        public async Task SelectDiscosParaComprar_Get(List<Disco> expectedDiscos, List<Genero> expectedGeneros,  string filterTitulo, string filterGenero, string filterArtista)
        {
            using (context)
            {
                
                // Arrange
                var controller = new DiscosController(context);
                controller.ControllerContext.HttpContext = purchaseContext;
                var expectedGenresSelectList = new SelectList(expectedGeneros.Select(g => g.Nombre).ToList());

                // Act
                var result = controller.SelectDiscosParaComprar(filterTitulo, filterGenero, filterArtista);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectDiscosParaComprarViewModel model = viewResult.Model as SelectDiscosParaComprarViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                // You must implement Equals in Discos, otherwise Assert will fail
                Assert.Equal(expectedDiscos, model.Discos);

                //check that both collections (expected and result) have the same names of Genre
                Assert.Equal(expectedGenresSelectList, model.Generos, Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                
            }
        }

                [Fact]
                public void SelectDiscosParaComprar_Post_DiscosNoSeleccionados()
                {
                    using (context)
                    {

                        // Arrange
                        var controller = new DiscosController(context);
                        controller.ControllerContext.HttpContext = purchaseContext;
                        var expectedGeneros = new SelectList(UtilitiesForDiscos.GetGeneros(0, 4).Select(g => g.Nombre).ToList());
                        //var expectedGeneros = UtilitiesForDiscos.GetGeneros(0, 4).Select(g => new { nameofGenre = g.Nombre });
                        var expectedDiscos = UtilitiesForDiscos.GetDiscos(0, 4);

                        SelectedDiscosParaComprarViewModel selected = new SelectedDiscosParaComprarViewModel { IdsToAdd = null };

                        // Act
                        var result = controller.SelectDiscosParaComprar(selected);

                        //Assert
                        var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                        SelectDiscosParaComprarViewModel model = viewResult.Model as SelectDiscosParaComprarViewModel;

                
                        // Check that both collections (expected and result returned) have the same elements with the same name
                        Assert.Equal(expectedDiscos, model.Discos);
                        Assert.Equal(expectedGeneros, model.Generos, Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));

            }
        }

                [Fact]
                public void SelectDiscosParaComprar_Post_DiscosSeleccionados()
                {
                    using (context)
                    {

                        // Arrange
                        var controller = new DiscosController(context);
                        controller.ControllerContext.HttpContext = purchaseContext;

                        String[] ids = new string[1] { "1" };
                        SelectedDiscosParaComprarViewModel discos = new SelectedDiscosParaComprarViewModel { IdsToAdd = ids };

                        // Act
                        var result = controller.SelectDiscosParaComprar(discos);

                        //Assert
                        var viewResult = Assert.IsType<RedirectToActionResult>(result);
                        var currentDiscos = viewResult.RouteValues.Values.First();
                        Assert.Equal(discos.IdsToAdd, currentDiscos);

                    }
                }


    }
}
