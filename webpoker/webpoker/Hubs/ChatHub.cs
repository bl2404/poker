using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Timers;
using webpoker.Models;
using webpoker.GameModels;
using System.Diagnostics;

namespace webpoker.Hubs
{
    public class ChatHub : Hub
    {
        public ChatHub()
        {
            //Instance = this;
            Application.Instance.ChatHub = this;
        }
        public void SendMessage(string sender,string message, bool joinGame)
        {
            Table table = Application.Instance.Tables.First(x => x.Users.Any(u => u.Name == sender));

            if (message == "")
            {
                table.Game = new Game(table);
            }
            else
            {
                if(table.Game!=null)
                    table.Game.NextStep(message);
            }

            if (joinGame == false)
            {
                table.Users.Remove(table.Users.First(x => x.Name == sender));
            }

            Clients.Group(table.Name).SendAsync("ReceiveMessage", sender, table.Game.GetGameInfo());
        }

        public async Task SendName(string username)
        {
            Table table = Application.Instance.Tables.First(x => x.Users.Any(u => u.Name == username));
            await JoinGroup(table.Name).ConfigureAwait(false);
            var users = table.Users.Select(x => x.GetUserInfo()).ToArray();
            await Clients.Group(table.Name).SendAsync("ReceiveMessage", username, string.Join(";",users)+":");
        }

        private Task JoinGroup(string name)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, name);
        }


        //public static ChatHub Instance { get; private set; }
    }
}
