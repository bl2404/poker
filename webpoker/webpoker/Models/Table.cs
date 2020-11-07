using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webpoker.GameModels;
using webpoker.Hubs;

namespace webpoker.Models
{
    public class Table
    {
        public Table()
        {
            Users = new List<User>();
        }

        public List<User> Users { get; set; }
        public string Name { get; set; }
        public Game Game { get; set; }
    }
}
