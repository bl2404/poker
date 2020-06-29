using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webpoker.Game
{
    public class Game
    {
        public Game()
        {
            Instance = this;
            Application.Instance.Games.Add(this);
        }
        public Models.Table Table { get; set; }

        public static Game Instance { get; private set; }
    }
}
