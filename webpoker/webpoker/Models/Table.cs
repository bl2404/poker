using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webpoker.Models
{
    public class Table
    {
        public Table()
        {
            Users = new List<User>();
            //Application.Instance.Games.Add(this);

        }
        public List<User> Users { get; set; }
        public string Name { get; set; }
        public User Admin { get; set; }
        public Game Game { get; set; }
    }
}
