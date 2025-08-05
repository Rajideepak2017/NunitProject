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
        // public IWebDriver driver;
        public ThreadLocal<IWebDriver> driver = new ();
        public PageObjectManager pageObjectManager;
        public static AsyncLocal<ExtentTest> test = new AsyncLocal<ExtentTest>();

        public static ExtentReports extent = new ExtentReports ();




        [OneTimeSetUp]
        public void Setup()
        {
            
            string workingdirectory = Environment.CurrentDirectory;
            string parentdirectory = Directory.GetParent(workingdirectory).Parent.Parent.FullName;

            TestContext.WriteLine("Report path: " + parentdirectory);
            // Get the current month & year
            var monthYear = DateTime.Now.ToString("yyyy-MM");
            var reportFolder = Path.Combine(parentdirectory, "Reports", monthYear); 

            Directory.CreateDirectory(reportFolder); 
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string reportpath = Path.Combine(reportFolder, $"index_{timestamp}.html");
            // string reportpath = parentdirectory + $"//index_{timestamp}.html";
            var report = new ExtentSparkReporter(reportpath);
            
            TestContext.WriteLine("Report path: " + reportpath);
            extent.AttachReporter(report);
            extent.AddSystemInfo("Env", "Prodo Portal site");
           

        }
        [SetUp]
        public void Startbrowser()
        {
            try
            {
                if (extent == null)
                {
                    throw new Exception("ExtentReports is not initialized.");
                }

                string testName = TestContext.CurrentContext.Test.Name;
                test.Value = extent.CreateTest(testName);

                new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
                driver.Value = new ChromeDriver();
                driver.Value.Manage().Window.Maximize();

                string siteUrl = ConfigurationManager.AppSettings["URL"];
                if (string.IsNullOrEmpty(siteUrl))
                {
                    throw new Exception("Site URL is not configured.");
                }

                driver.Value.Navigate().GoToUrl(siteUrl);
                pageObjectManager = new PageObjectManager(driver.Value);
            }
            catch (Exception ex)
            {
                TestContext.WriteLine("Startbrowser failed: " + ex.Message);
                throw;
            }
        }



        public IWebDriver getdriver()
        {

            return driver.Value;
        }

        public void Wait(IWebElement element)
        {
            WebDriverWait wait = new WebDriverWait(driver.Value, TimeSpan.FromSeconds(2000));
            wait.Until(ExpectedConditions.ElementToBeClickable(element)).Click();
        }

        public static JsonReader getJSon()
        {
            return new JsonReader();
        }



        [TearDown]
        public void Stopbrowser()
        {
            try
            {
                var status = TestContext.CurrentContext.Result.Outcome.Status;
                var stacktrace = TestContext.CurrentContext.Result.StackTrace;

                if (status == TestStatus.Failed)
                {
                    if (driver.IsValueCreated && driver.Value != null && test.Value != null)
                    {
                        DateTime time = DateTime.Now;
                        string fileName = "Screenshot_" + time.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
                        var media = screenresponse(driver.Value, fileName);
                        test.Value.Fail("Test Failed", media);
                        test.Value.Log(Status.Fail, "Test failed with logtrace: " + stacktrace);
                    }
                }
                else if (status == TestStatus.Passed  && test.Value != null)
                {
                    test.Value.Pass("Test Passed");
                }

                if (extent != null)
                {
                    extent.Flush();
                }
              


                if (driver.IsValueCreated && driver.Value != null)
                {
                    driver.Value.Quit();
                    driver.Value.Dispose();
                }

            }
            catch (Exception ex)
            {
                TestContext.WriteLine("Stopbrowser failed: " + ex.Message);
            }
        }


        [OneTimeTearDown]
        public void Cleanup()
        {
            if ( extent!= null)
            {
                extent.Flush();
              
            }

           

            if (driver.IsValueCreated && driver.Value != null)
            {
                driver.Value.Quit();
                driver.Value.Dispose();
            }

            
            
            driver.Dispose();
        }



        public Media screenresponse(IWebDriver driver, string screenName)
        {
            if (driver == null)
            {
                TestContext.WriteLine("Driver is null, cannot capture screenshot.");
                return null;
            }

            try
            {
                ITakesScreenshot ts = (ITakesScreenshot)driver;
                var screenshot = ts.GetScreenshot().AsBase64EncodedString;
                return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot, screenName).Build();
            }
            catch (Exception ex)
            {
                TestContext.WriteLine("Screenshot capture failed: " + ex.Message);
                return null;
            }
        }

    }
}
