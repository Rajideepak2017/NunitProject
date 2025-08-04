using NunitSelenium.Login;
using NunitSelenium.Logout;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NunitSelenium.Utilities
{
    public class PageObjectManager
    {
        public IWebDriver driver;

        private LoginPage login;
        private LogoutPage logout;


        public PageObjectManager(IWebDriver driver)
        {
            this.driver = driver;   

        }

        public LoginPage getLoginpage()
        {
            login=  new LoginPage(driver);
            return login;
        }

        public LogoutPage GetLogoutPage()
        {
            logout = new LogoutPage(driver);
            return logout;
        }
    }
}
