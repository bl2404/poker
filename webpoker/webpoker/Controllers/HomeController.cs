using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webpoker.Models;
using System.Web;

namespace webpoker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private Table game;
        private User loggedUser;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            //game = new Game();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        public IActionResult SubmitName(User user)
        {
            if(user.Name=="abc")
            {
                Table table = new Table();
                table.Admin = user;
                table.Name = "stol1";

                User user1 = new User();
                user1.Name = "janusz";
                user1.Wallet = 777;

                table.Users.Add(user);
                table.Users.Add(user1);


                Game.Game game = new Game.Game();
                game.Table = table;
            }
            return RedirectToAction("Table");
        }

        public IActionResult Table()
        {
            List<Table> tables = new List<Table>();
            tables.Add(Game.Game.Instance.Table);
            return View(tables);
        }

        public ActionResult JoinTable()
        {
            return RedirectToAction("Index");
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public IActionResult GoButton(UserAction a)
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
