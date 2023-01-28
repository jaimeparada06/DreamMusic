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

namespace DreamMusic.UIT.Devoluciones
{
    public class CUDevolver_UIT : IDisposable
    {
        IWebDriver _driver;
        string _URI;

        public CUDevolver_UIT()
        {
            UtilitiesUIT.SetUp_UIT(out _driver, out _URI);
            Initial_step_opening_the_web_page();
        }

        [Fact]
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

        
        public void preconditionlogearse()
        {
            _driver.Navigate().GoToUrl(_URI + "Identity/Account/Login");
            _driver.FindElement(By.Id("Input_Email")).SendKeys("peter@uclm.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("OtherPass12$");
            _driver.FindElement(By.Id("login-submit")).Click();


        }

        public void primer_paso()
        {
            _driver.FindElement(By.Id("DevolucionesController")).Click();
        }

        public void segundo_paso()
        {
            _driver.FindElement(By.Id("SelectDiscosParaDevolucion")).Click();
        }
        public void tercer_paso_filtrar_por_titulo(string filtrotitulo)
        {
            _driver.FindElement(By.Id("discoTitulo")).SendKeys(filtrotitulo);
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

        public void tercer_paso_filtrar_por_artista(string filtroartista)
        {
            _driver.FindElement(By.Id("discoArtista")).SendKeys(filtroartista);
            _driver.FindElement(By.Id("filterbyTituloGenero")).Click();
        }

        public void tercer_paso_filtrar_por_año(int filtroAño)
        {
            _driver.FindElement(By.Id("discoA_o")).SendKeys(filtroAño.ToString());
            _driver.FindElement(By.Id("filterbyTituloGenero")).Click();

        }

        public void tercer_paso_filtrar_por_mes(int filtroMes)
        {
            _driver.FindElement(By.Id("discoMes")).SendKeys(filtroMes.ToString());
            _driver.FindElement(By.Id("filterbyTituloGenero")).Click();

        }

        public void tercer_paso_filtrar_por_dia(int filtroDia)
        {
            _driver.FindElement(By.Id("discoDia")).SendKeys(filtroDia.ToString());
            _driver.FindElement(By.Id("filterbyTituloGenero")).Click();

        }

        public void tercer_paso_select_discos_and_submit()
        {

            _driver.FindElement(By.Id("Disco_1")).Click();
            _driver.FindElement(By.Id("Disco_2")).Click();
            _driver.FindElement(By.Id("nextButton")).Click();

        }

        public void tercer_paso_alternate_not_selecting_discos()
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
        [ClassData(typeof(DevolverDiscosTestDataGeneratorBasicFlow))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_0_1_basic_flow(string deliveryAddress, string quantityMovie1,
            string quantityMovie2, string PaymentMethod, string creditCardNumber, string CCV, string expirationDate,
            string Email, string Prefix, string Phone)
        {
            //Arrange
            string[] expectedText = { "Details - DreamMusic","Details",
                "Devolver","Peter","Jackson","Jackson","FechaDevolucion","Direccion",
                deliveryAddress,
                "PrecioTotal","30","Scorpion","Drake","30","1"};
            //Act
            preconditionlogearse();
            primer_paso();
            segundo_paso();
            tercer_paso_select_discos_and_submit();
            cuarto_paso_rellenar_informacion_y_presionar_create(deliveryAddress,
                quantityMovie1, quantityMovie2, PaymentMethod, creditCardNumber, CCV, expirationDate, Email, Prefix, Phone);

            //Assert
            foreach (string expected in expectedText)
                Assert.Contains(expected, _driver.PageSource);

        }

        
        [Fact]
        public void UC2_2_alternate_flow_1_NoDiscosAvailable()
        {
            //Arrange
            string artistaparafiltrar = "Ed-Sheeran";
            string expectedText = "No hay discos disponibles";

            //Act
            preconditionlogearse();
            primer_paso();
            segundo_paso();
            tercer_paso_filtrar_por_artista(artistaparafiltrar);

            var discosRow = _driver.FindElement(By.Id("NoDiscos"));

            //checks the expected row exists
            Assert.NotNull(discosRow);
            Assert.Equal(expectedText, discosRow.Text);
        }
        

        [Fact]
        public void UC2_3_alternate_flow_2_filteringbyTitle()
        {
            //Arrange
            string tituloparafiltrar = "Scorpion";
            string[] expectedText = { "Scorpion", "Drake", "30", "Rap", "18", "08", "2020" };
            //Act: llama a los métodos que has creado anteriormente
            preconditionlogearse();
            primer_paso();
            segundo_paso();
            tercer_paso_filtrar_por_titulo(tituloparafiltrar);
            //Act
            var discoRow = _driver.FindElements(By.Id("Discos_Titulo_Scorpion"));
            //checks the expected row exists
            Assert.NotNull(discoRow);
            //checks every column has those data expected
            foreach (string expected in expectedText)
                Assert.NotNull(discoRow.First(l => l.Text.Contains(expected)));
        }

        [Fact]
        public void UC2_4_alternate_flow_2_filteringbyGenre()
        {
            //Arrange
            string generoparafiltrar = "Pop";
            string[] expectedText = { "Thriller", "Michael Jackson", "29", "Pop", "01", "01", "2001" };
            //Act: llama a los métodos que has creado anteriormente
            preconditionlogearse();
            primer_paso();
            segundo_paso();
            tercer_paso_filtrar_por_genero(generoparafiltrar);
            //Act
            var discoRow = _driver.FindElements(By.Id("Discos_Titulo_Thriller"));
            //checks the expected row exists
            Assert.NotNull(discoRow);
            //checks every column has those data expected
            foreach (string expected in expectedText)
                Assert.NotNull(discoRow.First(l => l.Text.Contains(expected)));

        }

        [Fact]
        public void UC2_5_alternate_flow_2_filteringbyArtista()
        {
            //Arrange
            string artistaparafiltrar = "Logic";
            string[] expectedText = { "UnderPressure", "Logic", "23", "Rap", "21", "12", "2012" };
            //Act: llama a los métodos que has creado anteriormente
            preconditionlogearse();
            primer_paso();
            segundo_paso();
            tercer_paso_filtrar_por_artista(artistaparafiltrar);
            //Act
            var discoRow = _driver.FindElements(By.Id("Discos_Titulo_UnderPressure"));
            //checks the expected row exists
            Assert.NotNull(discoRow);
            //checks every column has those data expected
            foreach (string expected in expectedText)
                Assert.NotNull(discoRow.First(l => l.Text.Contains(expected)));
        }

        [Fact]
        public void UC2_6_alternate_flow_2_filteringbyAño()
        {
            //Arrange
            int añoparafiltrar = 2012;
            string[] expectedText = { "UnderPressure", "Logic", "23", "Rap", "21", "12", "2012" };
            //Act: llama a los métodos que has creado anteriormente
            preconditionlogearse();
            primer_paso();
            segundo_paso();
            tercer_paso_filtrar_por_año(añoparafiltrar);
            //Act
            var discoRow = _driver.FindElements(By.Id("Discos_Titulo_UnderPressure"));
            //checks the expected row exists
            Assert.NotNull(discoRow);
            //checks every column has those data expected
            foreach (string expected in expectedText)
                Assert.NotNull(discoRow.First(l => l.Text.Contains(expected)));
        }

        [Fact]
        public void UC2_7_alternate_flow_2_filteringbyMes()
        {
            //Arrange
            int mesparafiltrar = 12;
            string[] expectedText = { "UnderPressure", "Logic", "23", "Rap", "21", "12", "2012" };
            //Act: llama a los métodos que has creado anteriormente
            preconditionlogearse();
            primer_paso();
            segundo_paso();
            tercer_paso_filtrar_por_mes(mesparafiltrar);
            //Act
            var discoRow = _driver.FindElements(By.Id("Discos_Titulo_UnderPressure"));
            //checks the expected row exists
            Assert.NotNull(discoRow);
            //checks every column has those data expected
            foreach (string expected in expectedText)
                Assert.NotNull(discoRow.First(l => l.Text.Contains(expected)));
        }

        [Fact]
        public void UC2_8_alternate_flow_2_filteringbydia()
        {
            //Arrange
            int diaparafiltrar = 21;
            string[] expectedText = { "UnderPressure", "Logic", "23", "Rap", "21", "12", "2012" };
            //Act: llama a los métodos que has creado anteriormente
            preconditionlogearse();
            primer_paso();
            segundo_paso();
            tercer_paso_filtrar_por_dia(diaparafiltrar);
            //Act
            var discoRow = _driver.FindElements(By.Id("Discos_Titulo_UnderPressure"));
            //checks the expected row exists
            Assert.NotNull(discoRow);
            //checks every column has those data expected
            foreach (string expected in expectedText)
                Assert.NotNull(discoRow.First(l => l.Text.Contains(expected)));
        }

        [Fact]
        public void UC2_9_alternate_flow_2_filteringbyAñoMesDia()
        {
            //Arrange
            int diaparafiltrar = 21;
            int mesparafiltrar = 12;
            int añoparafiltrar = 2012;
            string[] expectedText = { "UnderPressure", "Logic", "23", "Rap", "21", "12", "2012" };
            //Act: llama a los métodos que has creado anteriormente
            preconditionlogearse();
            primer_paso();
            segundo_paso();
            tercer_paso_filtrar_por_dia(diaparafiltrar);
            tercer_paso_filtrar_por_mes(mesparafiltrar);
            tercer_paso_filtrar_por_año(añoparafiltrar);
            //Act
            var discoRow = _driver.FindElements(By.Id("Discos_Titulo_UnderPressure"));
            //checks the expected row exists
            Assert.NotNull(discoRow);
            //checks every column has those data expected
            foreach (string expected in expectedText)
                Assert.NotNull(discoRow.First(l => l.Text.Contains(expected)));

        }

        [Fact]
        public void UC2_10_alternate_flow_3_NoDiscosAvailable()
        {
            //Arrange
            string expectedText = "Tienes que seleccionar al menos un disco";

            //Act
            preconditionlogearse();
            primer_paso();
            segundo_paso();
            tercer_paso_alternate_not_selecting_discos();

            var discoRow = _driver.FindElement(By.Id("ModelErrors"));
            //checks the expected row exists
            Assert.NotNull(discoRow);
            Assert.Equal(expectedText, discoRow.Text);
        }

        [Theory]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "15", "1", "CreditCard", "1234567890123456", "123", "12/12/2022", "peter@alu.uclm.es", "967", "237600", "No hay suficientes discos titulados Scorpion")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_11_alternate_flow_4_CantidadMayorQueDiscosDisponibles(string deliveryAddress, string quantityMovie1, string quantityMovie2,
            string PaymentMethod, string creditCardNumber, string CCV, string expirationDate,
            string Email, string Prefix, string Phone, string expectedText)
        {

            //Act
            preconditionlogearse();
            primer_paso();
            segundo_paso();
            tercer_paso_select_discos_and_submit();
            cuarto_paso_rellenar_informacion_y_presionar_create(deliveryAddress,quantityMovie1, quantityMovie2, PaymentMethod, creditCardNumber, CCV, expirationDate, Email, Prefix, Phone);

            var movieRow = _driver.FindElement(By.Id("ModelErrors"));

            //checks the expected row exists

            Assert.Equal(expectedText, movieRow.Text);
        }

        [Theory]
        [InlineData("", "2", "2", "CreditCard", "1234567890123456", "123", "12/12/2022", null, null, null, "Por favor, añada su direccion de envio")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "CreditCard", "", "123", "12/12/2022", null, null, null, "Por favor, rellena tu número de tarjeta de crédito para tu reembolso con tarjeta de crédito")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "CreditCard", "1234567890123456", "", "12/12/2022", null, null, null, "Por favor, rellena tu CCV para tu reembolso con tarjeta de crédito")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "CreditCard", "1234567890123456", "123", "", null, null, null, "Por favor, rellena la Fecha De expiración para tu reembolso con tarjeta de crédito")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "PayPal", null, null, null, "", "967", "673240", "Por favor, rellena con tu Email para tu reembolso con PayPal")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "PayPal", null, null, null, "peter@uclm.com", "", "673240", "Por favor, rellena con tu prefijo para tu reembolso con PayPal")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "PayPal", null, null, null, "peter@uclm.com", "967", "", "Por favor, rellena con tu teléfono para tu reembolso con PayPal")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "0", "0", "CreditCard", "1234567890123456", "123", "12/12/2022", null, null, null, "Por favor, selecciona una cantidad mayor que 0 para al menos un disco")]
        public void UC2_12_13_14_15_16_17_18_19_alternate_flow_5_TestingErrorsDatos(string deliveryAddress, string quantityMovie1, string quantityMovie2,
            string PaymentMethod, string creditCardNumber, string CCV, string expirationDate,
            string Email, string Prefix, string Phone, string expectedText)
        {

            //Act
            preconditionlogearse();
            primer_paso();
            segundo_paso();
            tercer_paso_select_discos_and_submit();
            cuarto_paso_rellenar_informacion_y_presionar_create(deliveryAddress, quantityMovie1, quantityMovie2, PaymentMethod, creditCardNumber, CCV, expirationDate, Email, Prefix, Phone);

            //checks the expected row exists

            Assert.Contains(expectedText, _driver.PageSource);
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_20_no_logueado()
        {
            //Arrange
            string expectedText = "Use a local account to log in.";

            //Act
            primer_paso();
            segundo_paso();
            //Assert
            Assert.Contains(expectedText, _driver.PageSource);
        }





        public void Dispose()
        {
            _driver.Close();
            _driver.Dispose();
            GC.SuppressFinalize(this);
 
        }


    }
}
