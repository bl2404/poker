using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.IO;
using System.Threading;

namespace GuiSeleniumTests
{
    public class User
    {
        private string infoXPath = "/html/body/div/main/div/div[7]/div";

        public User(string name)
        {
            Name = name;
            var dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent;
            Driver = new ChromeDriver(dir.ToString());
            Driver.Navigate().GoToUrl("https://localhost:44342/");
            Driver.FindElement(By.Id("username")).SendKeys(name);
            Driver.FindElement(By.Id("createUser")).Click();
            Driver.FindElement(By.XPath("/html/body/div/main/table/tbody[2]/tr/td/a")).Click();
            Thread.Sleep(1000);
        }

        public void ClickGoButton()
        {
            Driver.FindElement(By.Id("gobutton")).Click();
            Thread.Sleep(1000);
        }

        public void Przebij(int number)
        {
            Driver.FindElement(By.Id("valueinput")).SendKeys(number.ToString());
        }

        public string GetInfo()
        {
            return Driver.FindElement(By.XPath(infoXPath)).Text;
        }

        public void Close()
        {
            Driver.Close();
        }

        public ChromeDriver Driver { get; private set; }

        public string Name { get; private set; }


    }
}
