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
        public async Task SendMessage(string sender,string message)
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

            await Clients.All.SendAsync("ReceiveMessage", sender, table.Game.GetGameInfo());
        }

        public async Task SendName(string username)
        {
            var users = Application.Instance.AllUsers.Select(x => x.Name).ToArray();
            await Clients.All.SendAsync("ReceiveName", username, users);
        }
    }
}
