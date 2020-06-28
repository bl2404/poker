using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webpoker.Models
{
    public class TableModel
    {
        //public TableModel(User user)
        //{
        //    Users = new List<User>();
        //    Name = user.Name + "'s table";
        //    Users.Add(user);
        //}

        //public void JoinTable(User user)
        //{
        //    Users.Add(user);
        //}


        public string Name { get; set; }
        public List<User> Users { get; set; }
    }
}
