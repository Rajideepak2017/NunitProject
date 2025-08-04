using AventStack.ExtentReports;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace NunitSelenium.Utilities
{
    
    public class Baseclass
    {
        public IWebDriver driver;
        public PageObjectManager pageObjectManager;
        public static ExtentReports extent = new ExtentReports();
        public  ExtentTest test;
        [OneTimeSetUp]
        public void Setup()
        {
            string workingdirectory = Environment.CurrentDirectory;
            string parentdirectory = Directory.GetParent(workingdirectory).Parent.Parent.FullName;

            TestContext.WriteLine("Report path: " + parentdirectory);
            // Get the current month & year
            var monthYear = DateTime.Now.ToString("yyyy-MM");
            var reportFolder = Path.Combine(parentdirectory, "Reports", monthYear); // Example: Reports/2025-05
            Directory.CreateDirectory(reportFolder); // Ensure folder exists
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string reportpath = Path.Combine(reportFolder, $"index_{timestamp}.html");
            // string reportpath = parentdirectory + $"//index_{timestamp}.html";
            var report = new ExtentSparkReporter(reportpath);
            
            TestContext.WriteLine("Report path: " + reportpath);
            extent.AttachReporter(report);
            extent.AddSystemInfo("Env", "Prodo Portal site");
           

        }
        [SetUp]
        public void Startbrowser() {
            //var options = new ChromeOptions();
            //options.AddArgument("--window-size=1552,928"); 
            //options.AddArgument("--disable-gpu");
            //options.AddArgument("--no-sandbox");
            string testName = TestContext.CurrentContext.Test.Name;
            test = extent.CreateTest(testName);
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            String Siteurl = ConfigurationManager.AppSettings["URL"];

            if (string.IsNullOrEmpty(Siteurl))
            {
                throw new Exception("Site URL is not configured.");
            }
            driver.Navigate().GoToUrl(Siteurl);
            pageObjectManager= new PageObjectManager(getdriver());
        }

        public IWebDriver getdriver()
        {

            return driver;
        }

        public void Wait(IWebElement element)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2000));
            wait.Until(ExpectedConditions.ElementToBeClickable(element)).Click();
        }

        public static JsonReader getJSon()
        {
            return new JsonReader();
        }
        


        [TearDown]
        public void Stopbrowser()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = TestContext.CurrentContext.Result.StackTrace;
            if (status == TestStatus.Failed)
            {
                DateTime time = DateTime.Now;
                String fileName = "Screenshot_" + time.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
                test.Fail("Test Failed", screenresponse(driver, fileName));
                test.Log(Status.Fail, "Test failed with logtrace" + stacktrace);

            }
            else if (status == TestStatus.Passed)
            {
                TestContext.WriteLine("Test is Passed");
            }
            extent.Flush();
            driver.Dispose();
                 
        }

        public Media screenresponse(IWebDriver driver,String Screeenanme)
        {
            ITakesScreenshot ts =(ITakesScreenshot) driver;
            var screenshot =ts.GetScreenshot().AsBase64EncodedString;
            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot, Screeenanme).Build();
        }
    }
}
