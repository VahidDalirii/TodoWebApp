using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Xunit;

namespace ToDoWebApp.AutomatedUITests
{
    public class AutomatedUITests : IDisposable
    {
        private readonly IWebDriver _driver;
        public AutomatedUITests()
        {
            _driver = new ChromeDriver();
        }
        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        [Fact]
        public void Create_WhenExecuted_ReturnsCreateView()
        {
            _driver.Navigate()
                .GoToUrl("https://localhost:44345/Home/Create");

            Assert.Equal("Create - ToDoWebApp", _driver.Title);
            Assert.Contains("Create A To Do Task", _driver.PageSource);
        }

        [Fact]
        public void Create_WrongModelData_ReturnsErrorMessage()
        {
            _driver.Navigate()
                .GoToUrl("https://localhost:44345/Home/Create");

            _driver.FindElement(By.Id("Title"))
                .SendKeys("Test Title");

            _driver.FindElement(By.Id("Description"))
                .SendKeys("Test Description");
            
            _driver.FindElement(By.Id("Create"))
                .Click();

            var errorMessage = _driver.FindElement(By.Id("Date-error")).Text;

            Assert.Equal("The Date field is required.", errorMessage);
        }

        [Fact]
        public void Premium_WhenExecuted_ReturnsMessageToLogin()
        {
            //Log in first
            _driver.Navigate()
               .GoToUrl("https://localhost:44345/Identity/Account/Login");

            _driver.FindElement(By.Id("Email"))
                .SendKeys("vahiddd@gmail.com");

            _driver.FindElement(By.Id("Password"))
                .SendKeys("Vahid!123");

            _driver.FindElement(By.Id("Login"))
               .Click();

            //Then go back to home and Check the page's source
            _driver.Navigate()
                .GoToUrl("https://localhost:44345/Home/Premium");

            Assert.Equal("Premium - ToDoWebApp", _driver.Title);
            Assert.Contains("The total cost for using this app is 150 SEK", _driver.PageSource);
            Assert.Contains("check in the box if you are a student or pensioner, and you will receive a 50% discount", _driver.PageSource);
            Assert.Contains("Get Premium Version", _driver.PageSource);
        }

        [Fact]
        public void Premium_WrongModelData_ReturnsErrorMessage()
        {
            //Log in first
            _driver.Navigate()
               .GoToUrl("https://localhost:44345/Identity/Account/Login");

            _driver.FindElement(By.Id("Email"))
                .SendKeys("vahiddd@gmail.com");

            _driver.FindElement(By.Id("Password"))
                .SendKeys("Vahid!123");

            _driver.FindElement(By.Id("Login"))
               .Click();

            //Then go back to home and fills some of text boxes
            _driver.Navigate()
                .GoToUrl("https://localhost:44345/Home/Premium");

            _driver.FindElement(By.Id("CardNumber"))
                .SendKeys("1234567898765432");

            _driver.FindElement(By.Id("ExpiryDate"))
                .SendKeys("10/06/2022");

            _driver.FindElement(By.Id("NameOnCard"))
                .SendKeys("Test Name");

            _driver.FindElement(By.Id("Submit"))
                .Click();

            var errorMessage = _driver.FindElement(By.Id("CVV-error")).Text;

            Assert.Equal("This field is required.", errorMessage);
        }
    }
}
