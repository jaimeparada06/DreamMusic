using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using DreamMusic.Controllers;
using DreamMusic.Data;
using DreamMusic.Models;
using DreamMusic.UT.Controladores;
using DreamMusic.UT.CU_RestaurarDiscos.DiscoController_test;

namespace DreamMusic.UT.CU_RestaurarDiscos.RestauracionesController_test
{
    public class Details_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext restauracionContext;

        public Details_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            UtilitiesForRestauraciones.InitializeDbPurchasesForTests(context);


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new("elena@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new(user);
            restauracionContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            restauracionContext.User = identity;

        }

        public static IEnumerable<object[]> TestCasesFor_Restauraciones_notfound_withoutId()
        {
            var allTests = new List<object[]>
            {
                new object[] {null },
                new object[] {100},
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_Restauraciones_notfound_withoutId))]
        public async Task Details_Restauracion_notfound(int? id)
        {
            // Arrange
            using (context)
            {
                var controller = new RestauracionesController(context);
                controller.ControllerContext.HttpContext = restauracionContext;


                // Act
                var result = await controller.Details(id);

                //Assert
                var viewResult = Assert.IsType<NotFoundResult>(result);

            }
        }

        [Fact]
        public async Task Details_Restauracion_found()
        {
            // Arrange
            using (context)
            {
                var expectedPurchase = UtilitiesForRestauraciones.GetRestauracion(0, 1).First();
                var controller = new RestauracionesController(context);
                controller.ControllerContext.HttpContext = restauracionContext;

                // Act
                var result = await controller.Details(expectedPurchase.RestauracionId);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);

                var model = viewResult.Model as Restauracion;
                Assert.Equal(expectedPurchase, model);

            }
        }
    }
}
