using AventStack.ExtentReports;
using NunitSelenium.Utilities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NunitSelenium.Login
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class LoginTest : Baseclass
    {

        [Test]
        public void CorrectLogin()
        {
            

            string username = getJSon().dataextract("Validuser.username");
            string password = getJSon().dataextract("Validuser.password");
            string expResult = getJSon().dataextract("successText");
            pageObjectManager.getLoginpage().getUsername().SendKeys(username);
            test.Value.Log(Status.Pass, "Entered  Username");
            pageObjectManager.getLoginpage().getPassword().SendKeys(password);
            test.Value.Log(Status.Pass, "Entered  Password");
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver.Value;
            js.ExecuteScript("window.scrollBy(0,500);");
            pageObjectManager.getLoginpage().getSubmit().Click();
            test.Value.Log(Status.Pass, "User submitted the page");
            string successstext= pageObjectManager.GetLogoutPage().GetSuccess().Text;
            Assert.AreEqual(expResult, successstext);
            test.Value.Log(Status.Pass, "User successfully login in with text   "+ successstext);



        }

        [Test]
        public void IncorrectLogin()
        {


            string Nusername = getJSon().dataextract("Invaliduser.usernameIncorrect");
            string Npassword = getJSon().dataextract("Invaliduser.passwordIncorrect");
            string expResult = getJSon().dataextract("errorText");
            pageObjectManager.getLoginpage().getUsername().SendKeys(Nusername);
            test.Value.Log(Status.Pass, "Entered  Username");
            pageObjectManager.getLoginpage().getPassword().SendKeys(Npassword);
            test.Value.Log(Status.Pass, "Entered  Password");
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver.Value;
            js.ExecuteScript("window.scrollBy(0,500);");
            pageObjectManager.getLoginpage().getSubmit().Click();
            Wait(pageObjectManager.getLoginpage().getError());
            string errortext = pageObjectManager.getLoginpage().getError().Text;
            Assert.AreEqual(expResult, errortext);
            test.Value.Log(Status.Pass, "Page rendered with error message  " + errortext);



        }
    }
}
