using AventStack.ExtentReports;
using NunitSelenium.Utilities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NunitSelenium.Logout
{
    public  class LogoutTest :Baseclass
    {
        [Test]
        public void Logout()
        {
            string username = getJSon().dataextract("Validuser.username");
            string password = getJSon().dataextract("Validuser.password");
            string expResult = getJSon().dataextract("successText");
            string Title = getJSon().dataextract("Logintitle");
            pageObjectManager.getLoginpage().getUsername().SendKeys(username);
            test.Log(Status.Pass, "Entered  Username");
            pageObjectManager.getLoginpage().getPassword().SendKeys(password);
            test.Log(Status.Pass, "Entered  Password");
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollBy(0,500);");
            pageObjectManager.getLoginpage().getSubmit().Click();
            test.Log(Status.Pass, "User submitted the page");
            string successstext = pageObjectManager.GetLogoutPage().GetSuccess().Text;
            Assert.AreEqual(expResult, successstext);
            test.Log(Status.Pass, "User successfully login in with text   " + successstext);
            pageObjectManager.GetLogoutPage().GetLogoutCTA().Click();
            test.Log(Status.Pass, "User clicked Logout button");
            string Logintitle = pageObjectManager.getLoginpage().getLogintitle().Text;
            Assert.AreEqual(Title, Logintitle);
            test.Log(Status.Pass, "User successfully login out and back to   " + Logintitle +" Page");
        }
    }
}
