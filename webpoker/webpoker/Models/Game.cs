using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webpoker.Models
{
    public class Game
    {
        public Game()
        {
            Instance = this;
        }
        public List<User> Users { get; set; }
        public CardSuit CardSuit { get; set; }
        public Game Instance { get; private set; }
    }
}
