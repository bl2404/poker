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
    public class UserController: Controller
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
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

            HttpContext.Session.SetString("username", user.Name);
            user.Wallet = 100;

            return RedirectToAction("Tables","Table");
        }
    }
}
