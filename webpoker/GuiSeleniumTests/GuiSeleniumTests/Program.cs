using System;
using OpenQA.Selenium.Chrome;
using System.IO;

namespace GuiSeleniumTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir=Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent;
            var driver = new ChromeDriver(dir.ToString());
            driver.Navigate().GoToUrl("https://localhost:44342/");
            driver.Close();
            Console.ReadKey();
        }
    }
}
