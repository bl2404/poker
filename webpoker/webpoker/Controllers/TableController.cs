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
using Microsoft.AspNetCore.Http.Extensions;

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
            if (!Application.Instance.Tables.Any(x => x.Name == table.Name))
            {
                Application.Instance.Tables.Add(table);
            }

            return RedirectToAction("Tables");
        }

        public IActionResult Tables()
        {
            return View("Tables",Application.Instance.Tables);
        }

        public ActionResult JoinTable(string tableName)
        {
            var table = Application.Instance.Tables.First(x => x.Name == tableName);
            var userlist = table.Users;
            var clientName = HttpContext.Session.GetString("username");

            if (Application.Instance.Tables.Any(x => x.Users.Any(y => y.Name == clientName)))
                return RedirectToAction("Tables");
            if(table.Game!=null && table.Game.Finish==false)
                return RedirectToAction("Tables");

            table.Users.Add(Application.Instance.AllUsers.First(x => x.Name == clientName));
            return RedirectToAction("Game", new { @id = tableName  }) ;
        }

        public IActionResult Game()
        {
            var tableName = HttpContext.Request.GetDisplayUrl().Split('/').Last();
            var table = Application.Instance.Tables.First(x => x.Name == tableName);

            return View(table);
        }

        public IActionResult Back()
        {
            return RedirectToAction("Tables");
        }
    }
}
