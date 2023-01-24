using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using Auto.Messages;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Auto.Notifier
{
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
            var amqp = "amqp://user:rabbitmq@localhost:5672";
            using var bus = RabbitHutch.CreateBus(amqp);
            Console.WriteLine("Connected to bus! Listening for NewOwnerMatriculaMessage");
            var subscriberId = $"Auto.Notifier@{Environment.MachineName}";
            await bus.PubSub.SubscribeAsync<NewOwnerMatriculaMessage>(subscriberId, HandleNewOwnerMatriculaMessage);
            Console.ReadLine();
        }

        private static async Task HandleNewOwnerMatriculaMessage(NewOwnerMatriculaMessage nodim)
        {
            var csvRow =
                $"{nodim.Id} : {nodim.FullName}, {nodim.BirthDate},{nodim.ListedAtUtc:d} -" +
                $"{nodim.VehicleCount}";
            Console.WriteLine("123");
            Console.WriteLine(csvRow);
            var json = JsonSerializer.Serialize(nodim, JsonSettings());
            await hub.SendAsync("NotifyWebUsers", "Auto.Notifier",
                json);
        }

        static JsonSerializerOptions JsonSettings() =>
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
    }
}