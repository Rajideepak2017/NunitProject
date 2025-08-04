using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NunitSelenium.Logout
{
    public class LogoutPage
    {

        //Success Message
        public IWebDriver driver;
        [FindsBy(How = How.XPath, Using = "//*[@class='post-title']")]
        private IWebElement SuccessMSG;

        //Logout button

        
        [FindsBy(How = How.XPath, Using = "//*[@class='post-content']//a")]
        private IWebElement LogoutCTA;

        

        public LogoutPage(IWebDriver driver) { 
        this.driver = driver;
            PageFactory.InitElements(driver, this);
        
        }


        public IWebElement GetSuccess()
        {
            return SuccessMSG;
        }

        public IWebElement GetLogoutCTA()
        {
            return LogoutCTA;
        }
    }
}
