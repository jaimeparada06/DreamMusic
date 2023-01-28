﻿using Microsoft.AspNetCore.Mvc;
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
using DreamMusic.UT.CU_DevolverDiscos.DiscoController_test;

namespace DreamMusic.UT.CU_DevolverDiscos.DevolucionesController_test
{
   public class Details_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext devolucionContext;

        public Details_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            UtilitiesForDevoluciones.InitializeDbPurchasesForTests(context);


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new(user);
            devolucionContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            devolucionContext.User = identity;

        }

        public static IEnumerable<object[]> TestCasesFor_Devoluciones_notfound_withoutId()
        {
            var allTests = new List<object[]>
            {
                new object[] {null },
                new object[] {100},
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_Devoluciones_notfound_withoutId))]
        public async Task Details_Devolucion_notfound(int? id)
        {
            // Arrange
            using (context)
            {
                var controller = new DevolucionesController(context);
                controller.ControllerContext.HttpContext = devolucionContext;


                // Act
                var result = await controller.Details(id);

                //Assert
                var viewResult = Assert.IsType<NotFoundResult>(result);

            }
        }

        [Fact]
        public async Task Details_Devolucion_found()
        {
            // Arrange
            using (context)
            {
                var expectedPurchase = UtilitiesForDevoluciones.GetDevolucion(0, 1).First();
                var controller = new DevolucionesController(context);
                controller.ControllerContext.HttpContext = devolucionContext;

                // Act
                var result = await controller.Details(expectedPurchase.DevolucionId);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);

                var model = viewResult.Model as Devolucion;
                Assert.Equal(expectedPurchase, model);

            }
        }
    }
}