using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
//Necesario para obtener Find dentro de las ICollection o IList
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace DreamMusic.UIT.Compras
{
    public class CUComprar_UIT : IDisposable
    {
        IWebDriver _driver;
        string _URI;

        public CUComprar_UIT()
        {
            UtilitiesUIT.SetUp_UIT(out _driver, out _URI);
            Initial_step_opening_the_web_page();
        }

        public void Dispose()
        {
            _driver.Close();
            _driver.Dispose();
            GC.SuppressFinalize(this);

        }

        //[Fact]
        public void Initial_step_opening_the_web_page()
        {
            //Arrange
            string expectedTitle = "Home Page - DreamMusic";
            string expectedText = "Register";

            //Act
            //El navegador cargará la URI indicada
            _driver.Navigate().GoToUrl(_URI);

            //Assert
            //Comprueba que el título coincide con el esperado
            Assert.Equal(expectedTitle, _driver.Title);
            //Comprueba si la página contiene el string indicado
            Assert.Contains(expectedText, _driver.PageSource);
        }

        //[Fact]
        public void precondicion_logearse()
        {
            _driver.Navigate().GoToUrl(_URI + "Identity/Account/Login");

            _driver.FindElement(By.Id("Input_Email")).SendKeys("peter@uclm.com");

            _driver.FindElement(By.Id("Input_Password")).SendKeys("OtherPass12$");

            _driver.FindElement(By.Id("login-submit")).Click();
        }

        public void primer_paso()
        {
            _driver.FindElement(By.Id("ComprasController")).Click();
        }

        public void segundo_paso()
        {
            _driver.FindElement(By.Id("SelectDiscosParaComprar")).Click();
        }

        public void tercer_paso_filtrar_por_titulo(string titleFilter)
        {
            _driver.FindElement(By.Id("discoTitulo")).SendKeys(titleFilter);
            _driver.FindElement(By.Id("filterbyTituloGenero")).Click();
        }

        public void tercer_paso_filtrar_por_genero(string genreSelected)
        {
            var genre = _driver.FindElement(By.Id("discoGeneroSeleccionado"));
            //create select element object
            SelectElement selectElement = new SelectElement(genre);
            //select Action from the dropdown menu
            selectElement.SelectByText(genreSelected);
            _driver.FindElement(By.Id("filterbyTituloGenero")).Click();
        }
        
        public void tercer_paso_filtrar_por_artista(string artistaSeleccionado)
        {
            _driver.FindElement(By.Id("discoArtista")).SendKeys(artistaSeleccionado);
            _driver.FindElement(By.Id("filterbyTituloGenero")).Click();
        }

        public void tercer_paso_seleccionar_discos_and_submit()
        {

            _driver.FindElement(By.Id("Disco_1")).Click();
            _driver.FindElement(By.Id("Disco_2")).Click();
            _driver.FindElement(By.Id("nextButton")).Click();

        }

        public void tercer_paso_no_seleccionar_discos()
        {

            _driver.FindElement(By.Id("nextButton")).Click();

        }

        private void cuarto_paso_rellenar_informacion_y_presionar_create(string deliveryAddress, string quantityMovie1,
            string quantityMovie2, string PaymentMethod, string creditCardNumber, string CCV, string expirationDate,
            string Email, string Prefix, string Phone)
        {

            _driver.FindElement(By.Id("DeliveryAddress")).SendKeys(deliveryAddress);

            _driver.FindElement(By.Id("Movie_Quantity_1")).Clear();
            _driver.FindElement(By.Id("Movie_Quantity_1")).SendKeys(quantityMovie1);

            _driver.FindElement(By.Id("Movie_Quantity_2")).Clear();
            _driver.FindElement(By.Id("Movie_Quantity_2")).SendKeys(quantityMovie2);

            if (PaymentMethod.Equals("CreditCard"))
            {
                _driver.FindElement(By.Id("r11")).Click();

                _driver.FindElement(By.Id("CreditCardNumber")).SendKeys(creditCardNumber);

                _driver.FindElement(By.Id("CCV")).SendKeys(CCV);

                _driver.FindElement(By.Id("ExpirationDate")).Clear();
                _driver.FindElement(By.Id("ExpirationDate")).SendKeys(expirationDate);
            }
            else
            {
                _driver.FindElement(By.Id("r12")).Click();

                _driver.FindElement(By.Id("Email")).SendKeys(Email);

                _driver.FindElement(By.Id("Prefix")).SendKeys(Prefix);

                _driver.FindElement(By.Id("Phone")).SendKeys(Phone);
            }
            _driver.FindElement(By.Id("CreateButton")).Click();
        }


        [Theory]
        [ClassData(typeof(ComprarDiscosTestDataGeneratorBasicFlow))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_0_1_basic_flow(string deliveryAddress, string quantityMovie1,
            string quantityMovie2, string PaymentMethod, string creditCardNumber, string CCV, string expirationDate,
            string Email, string Prefix, string Phone)
        {
            //Arrange
            string[] expectedText = { "Details - DreamMusic","Details",
                "Compra","Peter","Jackson","FechaCompra","Direccion",
                deliveryAddress,
                "PrecioTotal","30","Scorpion","Drake","30", "2"};
            //Act
            precondicion_logearse();
            primer_paso();
            segundo_paso();
            tercer_paso_seleccionar_discos_and_submit();
            cuarto_paso_rellenar_informacion_y_presionar_create(deliveryAddress,
                quantityMovie1, quantityMovie2, PaymentMethod, creditCardNumber, CCV, expirationDate, Email, Prefix, Phone);

            //Assert
            foreach (string expected in expectedText)
                Assert.Contains(expected, _driver.PageSource);

        }


        [Theory]
        [InlineData("Hola", "Logic", "23", "Rap", "Title")]
        [InlineData("Scorpion", "Hola", "30", "Rap", "Artista")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_2_3_4_alternate_flow_1_noencuentrafiltros(string title, string artista, string price, string genre, string filter)
        {
            //Arrange
            string expectedText = "No hay discos disponibles";

            //Act
            precondicion_logearse();
            primer_paso();
            segundo_paso();
            if (filter.Equals("Title"))
                tercer_paso_filtrar_por_titulo(title);
            else
                tercer_paso_filtrar_por_artista(artista);

            var movieRow = _driver.FindElements(By.Id("NoDiscos"));

            //checks the expected row exists
            Assert.NotNull(movieRow);

            //checks every column has the data as expected
            Assert.NotNull(movieRow.First(l => l.Text.Contains(expectedText)));
        }



        [Theory]
        [InlineData("UnderPressure","Logic" ,"23", "Rap", "Title")]
        [InlineData("Scorpion","Drake" ,"30", "Rap", "Genre")]
        [InlineData("Scorpion", "Drake", "30", "Rap", "Artista")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_5_6_7_alternate_flow_3_filteringbyTitle(string title, string artista, string price, string genre, string filter)
        {
            //Arrange
            string[] expectedText = { title, artista ,price, genre };

            //Act
            precondicion_logearse();
            primer_paso();
            segundo_paso();
            if (filter.Equals("Title"))
                tercer_paso_filtrar_por_titulo(title);
            else if (filter.Equals("Genre"))
                tercer_paso_filtrar_por_genero(genre);
            else
                tercer_paso_filtrar_por_artista(artista);

            var movieRow = _driver.FindElements(By.Id("Disco_Titulo_" + title));

            //checks the expected row exists
            Assert.NotNull(movieRow);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(movieRow.First(l => l.Text.Contains(expected)));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_8_alternate_flow_4_NoMoviesAvailable()
        {
            //Arrange
            string expectedText = "Tienes que seleccionar al menos un disco";

            //Act
            precondicion_logearse();
            primer_paso();
            segundo_paso();
            tercer_paso_no_seleccionar_discos();

            var movieRow = _driver.FindElement(By.Id("ModelErrors"));

            //checks the expected row exists
            Assert.NotNull(movieRow);
            Assert.Equal(expectedText, movieRow.Text);
        }


        [Theory]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "15", "1", "CreditCard", "1234567890123456", "123", "12/12/2022", "peter@alu.uclm.es", "967", "237600", "No hay disponibles los discos titulados Scorpion")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_9_alternate_flow_5_CantidadMayorQueDiscosDisponibles(string deliveryAddress, string quantityMovie1, string quantityMovie2,
            string PaymentMethod, string creditCardNumber, string CCV, string expirationDate,
            string Email, string Prefix, string Phone, string expectedText)
        {

            //Act
            precondicion_logearse();
            primer_paso();
            segundo_paso();
            tercer_paso_seleccionar_discos_and_submit();
            cuarto_paso_rellenar_informacion_y_presionar_create(deliveryAddress,quantityMovie1, quantityMovie2, PaymentMethod, creditCardNumber, CCV, expirationDate, Email, Prefix, Phone);

            var movieRow = _driver.FindElement(By.Id("ModelErrors"));

            //checks the expected row exists

            Assert.Equal(expectedText, movieRow.Text);
        }


        [Theory]
        [InlineData("", "2", "2", "CreditCard", "1234567890123456", "123", "12/12/2022", null, null, null, "Por favor, añada una dirección de envío")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "CreditCard", "", "123", "12/12/2022", null, null, null, "Por favor, rellena tu número de tarjeta de crédito para tu pago con tarjeta de crédito")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "CreditCard", "1234567890123456", "", "12/12/2022", null, null, null, "Por favor, rellena tu CCV para tu pago con tarjeta de crédito")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "CreditCard", "1234567890123456", "123", "", null, null, null, "Por favor, rellena la Fecha De expiración para tu pago con tarjeta de crédito")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "PayPal", null, null, null, "", "967", "673240", "Por favor, rellena con tu Email para tu pago con PayPal")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "PayPal", null, null, null, "peter@uclm.com", "", "673240", "Por favor, rellena con tu prefijo para tu pago con PayPal")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "PayPal", null, null, null, "peter@uclm.com", "967", "", "Por favor, rellena con tu teléfono para tu pago con PayPal")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "0", "0", "CreditCard", "1234567890123456", "123", "12/12/2022", null, null, null, "Por favor, selecciona una cantidad mayor que 0 para al menos un disco")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_10_UC1_10_15_alternate_flow_6_TestingErrorsDatos(string deliveryAddress, string quantityMovie1, string quantityMovie2,
            string PaymentMethod, string creditCardNumber, string CCV, string expirationDate,
            string Email, string Prefix, string Phone, string expectedText)
        {

            //Act
            precondicion_logearse();
            primer_paso();
            segundo_paso();
            tercer_paso_seleccionar_discos_and_submit();
            cuarto_paso_rellenar_informacion_y_presionar_create(deliveryAddress, quantityMovie1, quantityMovie2, PaymentMethod, creditCardNumber, CCV, expirationDate, Email, Prefix, Phone);

            //checks the expected row exists

            Assert.Contains(expectedText, _driver.PageSource);
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_16_no_logueado()
        {
            //Arrange
            string expectedText = "Use a local account to log in.";

            //Act
            primer_paso();
            segundo_paso();
            //Assert
            Assert.Contains(expectedText, _driver.PageSource);

        }


    }
}
