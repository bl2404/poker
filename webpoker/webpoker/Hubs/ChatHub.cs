using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace webpoker.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user,string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendName(string username)
        {
            var users = Application.Instance.AllUsers.Select(x => x.Name).ToArray();
            await Clients.All.SendAsync("ReceiveName", username, users);
        }


        public async Task SendStartSignal()
        {
            var game = Application.Instance.Games[0];
            if (game.CurrentUser == null )
            {
                var random = new Random();
                int index=random.Next(0, game.Users.Count-1);
                game.CurrentUser = game.Users[index];

            }
            else if(game.CurrentUser == game.Users.Last())
            {
                game.CurrentUser = game.Users.First();
            }
            else
            {
                game.CurrentUser = Application.Instance.Games[0].Users[Application.Instance.Games[0].Users.IndexOf(game.CurrentUser)+1];
            }


            await Clients.All.SendAsync("ReceiveStartSignal", game.CurrentUser.Name);
        }
    }
}
