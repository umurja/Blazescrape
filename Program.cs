using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using System.Text;


namespace Blazescrape
{
    internal class Program
    {
        public static IWebDriver driver;
        //public static string baseURL;

        static void Main(string[] arg)
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            Console.WriteLine("Going to this url: " + arg[0]);
            driver.Navigate().GoToUrl(arg[0]);

            Thread.Sleep(5000);
            By testTitle = By.XPath("//div[@class='main-entity-title not-editable']");
            By smryMaxUsers = By.XPath("//div[@class='box users']//span[@class='count']");
            By smryThruput = By.XPath("//div[@class='box hits']//span[@class='count']");
            By smryErrors = By.XPath("//div[@class='box errors']//span[@class='count']");
            By smryAvgRespTime = By.XPath("//div[@class='box response-time']//span[@class='count']");
            By smry90PercRespTime = By.XPath("//div[@class='box avg-response-time']//span[@class='count']");
            By smryAvgBandwidth = By.XPath("//div[@class='box bytes']//span[@class='count']");
            By smryDuration = By.XPath("//li[contains(@aria-label, 'Duration')]/div[@class='config-value col-8']");
            By smryStarted = By.XPath("//li[contains(@aria-label, 'Started')]/div[@class='config-value col-8']");
            By smryEnded = By.XPath("//li[contains(@aria-label, 'Ended')]/div[@class='config-value col-8']");
            By smryLocations = By.XPath("//li[contains(@aria-label, 'Locations')]/div[@class='config-value col-8']");

            By OrigTestConfigTab = By.XPath("//a[@title='Original Test Configuration']");
            By ErrorsTab = By.XPath("//a[@title='Errors']");
            By ErrResponseCodeBtn = By.XPath("//button[@value='Response Code']");
            By ErrRespsAccordion = By.XPath("//div[@class='accordion']");

            string headers1 = "Title,MaxUsers,Avg. Throughput,Errors %,Avg. Response Time,90% Response Time,Avg. Bandwidth,Duration,Started1,Started2,Started3,Ended1,Ended2,Ended3,Locations1,Locations2,Locations3,Locations4,Locations5";
            StringBuilder first = new StringBuilder();


            first.Append(driver.FindElement(testTitle).Text + ",");

            first.Append(driver.FindElement(smryMaxUsers).Text + ",");
            first.Append(driver.FindElement(smryThruput).Text + ",");
            first.Append(driver.FindElement(smryErrors).Text + ",");
            first.Append(driver.FindElement(smryAvgRespTime).Text + ",");
            first.Append(driver.FindElement(smry90PercRespTime).Text + ",");
            first.Append(driver.FindElement(smryAvgBandwidth).Text + ",");
            first.Append(driver.FindElement(smryDuration).Text + ",");
            first.Append(driver.FindElement(smryStarted).Text + ",");
            first.Append(driver.FindElement(smryEnded).Text + ",");
            first.Append(driver.FindElement(smryLocations).Text);

            //errors
            driver.FindElement(ErrorsTab).Click();
            Thread.Sleep(2000);
            driver.FindElement(ErrResponseCodeBtn).Click();
            Thread.Sleep(2000);
            var errs = driver.FindElement(ErrRespsAccordion).Text;

            //orig test config
            driver.FindElement(OrigTestConfigTab).Click();
            Thread.Sleep(5000);

            //string theStringCatcher = "";
            By othead = By.XPath("//div[@class='summary-panel-ntc card']");
            By otfirst = By.XPath("//div[@class='scenario-panel scenario-0-panel accordion']");
            By otsecond = By.XPath("//div[@class='scenario-panel scenario-1-panel accordion']");

            using (var w = new StreamWriter("reportOutput.csv"))
            {
                //var line = string.Format("{0},{1},{2}", headers, first, second);
                w.WriteLine(headers1);
                w.WriteLine(first);
                w.WriteLine(FormatForCSV(driver.FindElement(othead).Text));
                w.WriteLine(FormatForCSV(driver.FindElement(otfirst).Text));
                w.WriteLine(FormatForCSV(driver.FindElement(otsecond).Text));
                w.WriteLine("Errors below...");
                w.WriteLine(FormatForCSV(errs));

                w.Flush();
            }

            //end do the work
            driver.Dispose();
        }

        public int GetNumScenarios()
        {
            int retval = 0;
            retval = driver.FindElements(By.XPath("//div[@class='scenario-panel-summary']")).Count;
            return retval;
        }

        public static string FormatForCSV(string cleanThisUp)
        {
            string retval = "";
            retval = cleanThisUp.Replace("\r\n", ",");
            return retval;
        }
    }
}
