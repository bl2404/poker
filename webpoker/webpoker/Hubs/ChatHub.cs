using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using webpoker.Models;

namespace webpoker.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string sender,string message, bool joinGame)
        {
            Table table = Application.Instance.Tables[0];

            if (message == "")
            {
                table.Game = new Game();
            }
            else
            {
                table.Game.NextStep(message);
            }

            if (joinGame == false)
            {
                Application.Instance.Tables[0].Users.Remove(Application.Instance.Tables[0].Users.First(x => x.Name == sender));
            }

            if (!Application.Instance.Tables[0].Users.Contains(Application.Instance.Tables[0].Admin))
                Application.Instance.Tables[0].Admin = Application.Instance.Tables[0].Users[0];

            await Clients.All.SendAsync("ReceiveMessage", sender, table.Game.GetGameInfo());
        }

        public async Task SendName(string username)
        {
            var users = Application.Instance.Tables[0].Users.Select(x => x.GetUserInfo()).ToArray();
            await Clients.All.SendAsync("ReceiveMessage", username, string.Join(";",users)+":");
        }
    }
}
