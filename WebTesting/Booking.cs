using System;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace WebTesting
{

    [TestFixture]
    class Booking
    {        
        private IWebDriver _webDriver = new OperaDriver();
        private WebDriverWait _webDriverWait = null;
        private const string Reference = "https://www.booking.com";

        [SetUp]
        public void Initialize() {
            
            _webDriver.Navigate().GoToUrl(Reference);
            _webDriver.Manage().Window.Maximize();
            _webDriverWait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(3.0));

            Console.WriteLine("Directory: " + Environment.CurrentDirectory);
        }

        [Test]
        public void ChangeCulture() {
        
            _webDriver.FindElement(By.CssSelector(".bui-avatar__image")).Click();            
            _webDriverWait.Until(ExpectedConditions.ElementIsVisible(By.LinkText("Українська"))).Click();
            _webDriverWait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".bui-button__text"))).Click();
            _webDriverWait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'UAH')]"))).Click();
        }


        [Test]
        public void TicketsCheck() {

            By ticketsHeader = By.XPath("//header[contains(@class,'bui-header bui-header--logo-large bui-u-hidden-print')]//li[2]//a[1]//span[2]");
            _webDriver.FindElement(ticketsHeader).Click();
        }

        [Test]
        public void AccountAccess()
        {
            By personalAccountHeader = By.XPath("//div[@class='bui-avatar bui-avatar--text bui-avatar--accent bui-avatar--outline-accent']//img[@class='bui-avatar__image']");
            By registrationHeader = By.XPath("//header[@class='bui-header bui-header--logo-large bui-u-hidden-print']//div[6]//a[1]//span[1]");

            try
            {
                _webDriver.FindElement(personalAccountHeader).Click();
            }
            catch (NoSuchElementException)
            {               
                _webDriver.FindElement(registrationHeader).Click();
            }
        }

        [Test]
        public void FilterCheck() {

            FilterPage filterPage = new FilterPage(_webDriver);
            filterPage.TypeCity("Витебск").TypeDate(2).TypePeople(2, 1, 1);
        }

      
        [TearDown]
        public void ClosePage() {

            //_webDriver.Close();
        }


    }
}
