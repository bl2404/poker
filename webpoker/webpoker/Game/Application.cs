using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webpoker.Game
{
    public class Application
    {
        public Application()
        {
            Games = new List<Game>();
            AllUsers = new List<Models.User>();
            Instance = this;
        }
        public List<Game> Games { get; set; }
        public List<Models.User> AllUsers { get; set; }
        public static Application Instance { get; set; }
    }
}
