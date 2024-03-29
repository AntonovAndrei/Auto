﻿using Microsoft.AspNetCore.SignalR;

namespace Auto.SignalRWeb.Hubs;

public class AutoHub : Hub
{
    public async Task NotifyWebUsers(string user, string message)
    {
        await Clients.All.SendAsync("DisplayNotification", user, message);
    }
}