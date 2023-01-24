
using Auto.Messages;
using Auto.OwnerServer;
using EasyNetQ;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

namespace Auto.OwnercCient; // Note: actual namespace depends on the project name.

internal class Program
{
    private static Matricula.MatriculaClient grpcClient;
    private static IBus bus;
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting Auto.OwnerClient");
        var amqp = "amqp://user:rabbitmq@localhost:5672";
        bus = RabbitHutch.CreateBus(amqp);
        
        Console.WriteLine("Connected to bus; Listening for newVehicleMessages");
        var grpcAddress = "https://localhost:7145";
        using var channel = GrpcChannel.ForAddress(grpcAddress);
        grpcClient = new Matricula.MatriculaClient(channel);
        Console.WriteLine($"Connected to gRPC on {grpcAddress}!");
            
        var subscriberId = $"Auto.PricingClient@{Environment.MachineName}";
        await bus.PubSub.SubscribeAsync<NewOwnerMessage>(subscriberId, HandleNewOwnerMessage);
            
        Console.WriteLine("Press Enter to exit");
        Console.ReadLine();

    }
    
    private static async Task HandleNewOwnerMessage(NewOwnerMessage message) {
        Console.WriteLine($"new owner; {message.Id} - {message.FullName}");
        var ownerRequest = new OwnerRequest() {
            FullName = message.FullName,
            BirthDate = Timestamp.FromDateTime(message.BirthDate)
        };
        var priceReply = await grpcClient.GetMatriculaAsync(ownerRequest);
        Console.WriteLine($"Owner {message.FullName} have {priceReply.VehicleNumber} vehicles");
        var newOwnerMatriculaMessage = new NewOwnerMatriculaMessage(message, Int32.Parse(priceReply.VehicleNumber));
        await bus.PubSub.PublishAsync(newOwnerMatriculaMessage);
    }
}
