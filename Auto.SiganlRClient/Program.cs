
using Microsoft.AspNetCore.SignalR.Client;

namespace Auto.SignalRClient;

internal class Program
{
    const string SIGNALR_HUB_URL = "https://localhost:7143/hub";
    private static HubConnection hub;
    
    static async Task Main(string[] args)
    {
        hub = new HubConnectionBuilder().WithUrl(SIGNALR_HUB_URL).Build();
        await hub.StartAsync();
        Console.WriteLine("Hub started!");
        Console.WriteLine("Press any key to send a message (Ctrl-C to quit)");
        var i = 0;
        while (true)
        {
            var input = Console.ReadLine();
            var message = $"Message #{i++} from Auto.Notifier {input}";
            await hub.SendAsync("NotifyWebUsers", "Auto.Notifier", message);
            Console.WriteLine($"Sent: {message}");
        }

    }
}