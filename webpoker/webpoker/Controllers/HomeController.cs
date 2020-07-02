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
            if (!Application.Instance.AllUsers.Any(x => x.Name == user.Name))
            {
                Application.Instance.AllUsers.Add(user);
            }
            if (user.Name == "abc")
            {
                Application.Instance.Games[0].Admin = user;
            }

            HttpContext.Session.SetString("username", user.Name);
            user.Wallet = new Random().Next(50,100);

            return RedirectToAction("Table");
        }

        public IActionResult Table()
        {
            return View(Application.Instance.Games);
        }

        public ActionResult JoinTable()
        {
            var userlist = Application.Instance.Games[0].Users;
            var clientName = HttpContext.Session.GetString("username");
            if (!userlist.Any(x => x.Name == clientName))
            {
                Application.Instance.Games[0].Users.Add(Application.Instance.AllUsers.First
                    (x => x.Name == HttpContext.Session.GetString("username")));
            }

            return RedirectToAction("Index");
        }

        public ActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
