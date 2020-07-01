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
    }
}
