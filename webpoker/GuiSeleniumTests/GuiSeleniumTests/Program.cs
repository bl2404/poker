using System;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuiSeleniumTests
{
    class Program
    {
        static void Main(string[] args)
        {
            SingleGameTest();
            Environment.Exit(0);
        }

        private static void SingleGameTest()
        {
            User abc = new User("abc");
            abc.CreateTable("table1");
            User xyz = new User("xyz");
            xyz.JoinTable("xyz");

            abc.ClickGoButton();

            //enter
            abc.ClickGoButton();
            xyz.ClickGoButton();

            //flop
            abc.ClickGoButton();
            xyz.ClickGoButton();

            //turn
            abc.ClickGoButton();
            xyz.ClickGoButton();

            //river
            abc.ClickGoButton();
            xyz.ClickGoButton();

            abc.Przebij(5);
            abc.ClickGoButton();
            xyz.ClickGoButton();

            Assert.AreEqual("Wait for users and start the game", abc.GetInfo());

            abc.Close();
            xyz.Close();
        }

    }
}
