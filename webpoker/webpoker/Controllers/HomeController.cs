using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webpoker.Models;
using System.Web;
using Microsoft.AspNetCore.Http;


namespace webpoker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(Application.Instance.Games[0]);
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        public IActionResult SubmitName(User user)
        {
            Application.Instance.AllUsers.Add(user);
            HttpContext.Session.SetString("username", user.Name);
            user.Wallet = new Random().Next(50,100);
            if (user.Name=="abc")
            {
                Game game = new Game();
                game.Admin = user;
                game.Name = "stol1";

                User user1 = new User();
                user1.Name = "janusz";
                user1.Wallet = 777;

                game.Users.Add(user);
                game.Users.Add(user1);
            }
            return RedirectToAction("Table");
        }

        public IActionResult Table()
        {
            return View(Application.Instance.Games);
        }

        public ActionResult JoinTable()
        {
            Application.Instance.Games[0].Users.Add(Application.Instance.AllUsers.First//zrobic [0]
                (x => x.Name == HttpContext.Session.GetString("username")));
            return RedirectToAction("Index");
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public IActionResult GoButton()
        {
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
