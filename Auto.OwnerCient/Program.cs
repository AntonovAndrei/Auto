
using Auto.OwnerServer;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

namespace Auto.OwnercCient; // Note: actual namespace depends on the project name.

internal class Program
{
    static void Main(string[] args)
    {
        using var channel = GrpcChannel.ForAddress("https://localhost:7145");
        var grpcClient = new Matricula.MatriculaClient(channel);
        Console.WriteLine("Нажмите любую кнопку для отправки grpc запроса:");
        while (true)
        {
            Console.ReadKey(true);
            var request = new OwnerRequest()
            {
                FullName = "Harry James Potter",
                BirthDate = Timestamp.FromDateTimeOffset(new DateTime(2002,11,25, 20, 34, 34))
            };

            Console.WriteLine(request.FullName + " " + request.BirthDate);
            var reply = grpcClient.GetMatricula(request);
            Console.WriteLine(reply.VehicleNumber);
        }
    }
}
