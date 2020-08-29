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
    public class TableController: Controller
    {
        private readonly ILogger<TableController> _logger;

        public TableController(ILogger<TableController> logger)
        {
            _logger = logger;
        }
        public IActionResult CreateTable()
        {
            return View("CreateTable");
        }

        public IActionResult SubmitTableCreation(Table table)
        {
            Application.Instance.Tables.Add(table);
            table.Admin = Application.Instance.AllUsers.First(x => x.Name == HttpContext.Session.GetString("username"));
            return RedirectToAction("Tables");
        }

        public IActionResult Tables()
        {
            return View("Tables",Application.Instance.Tables);
        }

        public ActionResult JoinTable()
        {
            var userlist = Application.Instance.Tables[0].Users;
            var clientName = HttpContext.Session.GetString("username");
            if (!userlist.Any(x => x.Name == clientName))
            {
                Application.Instance.Tables[0].Users.Add(Application.Instance.AllUsers.First
                    (x => x.Name == clientName));
            }
            return RedirectToAction("Game", new { @id = Application.Instance.Tables[0].Name });
        }

        public IActionResult Game(Table table)
        {
            Debug.WriteLine(table.Name);
            return View(Application.Instance.Tables[0]);
        }
    }
}
