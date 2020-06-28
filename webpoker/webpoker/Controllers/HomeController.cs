using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webpoker.Models;

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
            return View();
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        public IActionResult SubmitName(User user)
        {
            return RedirectToAction("Table");
        }

        public IActionResult Table(TableModel table)
        {
            User user1 = new User();
            user1.Name = "abc";
            user1.Wallet = 100;

            User user2 = new User();
            user1.Name = "xyz";
            user1.Wallet = 150;


            //TableModel table = new TableModel();
            table.Users = new List<User>();
            table.Users.Add(user1);
            table.Users.Add(user2);

            table.Name = "stolik1";
            var tables = new List<TableModel>();
            var table2 = new TableModel();
            table2.Name = "stolik2";
            tables.Add(table);
            tables.Add(table2);
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

        public IActionResult InsertStudent(UserAction a)
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
