using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace NunitSelenium.Login
{
    public  class LoginPage
    {
         public IWebDriver driver;

        //Login title
        
        [FindsBy(How = How.XPath, Using = "//*[@id='login']/h2")]
        private IWebElement Logintitle;
        //Username
        [FindsBy(How=How.Id, Using= "username")]
        private IWebElement Username;

        //Password
        [FindsBy(How=How.XPath,Using = "//*[@type='password']")]
        private IWebElement Password;

        //Submit
        [FindsBy(How=How.XPath,Using = "//*[@id='submit']")]
        private IWebElement Submit;

        //Error message
        [FindsBy(How = How.Id, Using = "error")]
        private IWebElement Error;

        public LoginPage(IWebDriver driver) {
            this.driver = driver;
            PageFactory.InitElements(driver,this);
        }



        public IWebElement getUsername()
        {
            return Username;
        }

        public IWebElement getPassword()
        {
            return Password;
        }

        public IWebElement getSubmit()
        {
            return Submit;
        }

        public IWebElement getError()
        {
            return Error;
        }

        public IWebElement getLogintitle()
        {
            return Logintitle;
        }
    }
}
