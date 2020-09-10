using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    [Authorize(Policy="ApiUser")]
    public class MessagesHub : Hub
    {
        public async Task SendMessage(string userId, string message)
        {
            var userName = Context.User.Identity.Name;

            await Clients.User(userId).SendAsync("Receive", message, userName);
        }
    }
}
