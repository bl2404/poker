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
            for (int i = 0; i < 10; i++)
            {
                BidOnLastCard();
            }
            Environment.Exit(0);
        }

        private static void BidOnLastCard()
        {
            User abc = new User("abc");
            User xyz = new User("xyz");
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
