using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webpoker.Models;

namespace webpoker
{
    public class Application
    {
        public Application()
        {
            Games = new List<Game>();
            AllUsers = new List<Models.User>();

            //temporary
            Game game = new Game();
            game.Name = "game1";
            Games.Add(game);
            //

            Instance = this;
        }
        public List<Game> Games { get; set; }
        public List<Models.User> AllUsers { get; set; }
        public static Application Instance { get; set; }
    }
}
