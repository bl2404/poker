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
            Users = new List<User>();
            //Application.Instance.Games.Add(this);

        }
        public List<User> Users { get; set; }
        public CardSuit CardSuit { get; set; }
        public string Name { get; set; }
        public User Admin { get; set; }
        public User CurrentUser { get; set; }
    }
}
