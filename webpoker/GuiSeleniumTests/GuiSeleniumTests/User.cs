using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace GuiSeleniumTests
{
    public class User
    {
        private string infoXPath = "/html/body/div/main/div/div[7]/div";
        private ChromeDriver driver;

        public User(string name) 
        {
            var dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent;
            driver = new ChromeDriver(dir.ToString());

            driver.Navigate().GoToUrl("https://localhost:44342/");
            driver.FindElement(By.Id("username")).SendKeys(name);
            driver.FindElement(By.Id("createUser")).Click();
            Thread.Sleep(1000);
        }

        public void CreateTable(string name)
        {
            driver.FindElement(By.XPath("/html/body/div/main/p/a")).Click();
            driver.FindElement(By.XPath("/html/body/div/main/div[1]/div/form/div[1]/input")).SendKeys("table1");
            driver.FindElement(By.XPath("/html/body/div/main/div[1]/div/form/div[2]/input")).Click();
            driver.FindElement(By.XPath("/html/body/div/main/table/tbody[2]/tr/td/a")).Click();
        }

        public void JoinTable(string name)
        {
            driver.FindElement(By.XPath("/html/body/div/main/table/tbody[2]/tr/td/a")).Click();
        }


        public void ClickGoButton()
        {
            driver.FindElement(By.Id("gobutton")).Click();
            Thread.Sleep(1000);
        }

        public void Przebij(int number)
        {
            driver.FindElement(By.Id("valueinput")).SendKeys(number.ToString());
        }

        public string GetInfo()
        {
            return driver.FindElement(By.XPath(infoXPath)).Text;
        }

        public void Close()
        {
            driver.Close();
        }
    }
}
