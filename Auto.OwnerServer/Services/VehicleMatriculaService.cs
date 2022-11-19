using Auto.Data;
using Grpc.Core;

namespace Auto.OwnerServer.Services;

public class VehicleMatriculaService : Matricula.MatriculaBase
{
    private readonly ILogger<VehicleMatriculaService> _logger;
    private readonly IAutoDatabase _db;

    public VehicleMatriculaService(ILogger<VehicleMatriculaService> logger,
        IAutoDatabase db)
    {
        _logger = logger;
        _db = db;
    }

    public override Task<OwnerReply> GetMatricula(OwnerRequest request, 
        ServerCallContext context)
    {
        _logger.LogInformation($"Start GetOwnerVehicle() method in OwnerVehicleService class {request.FullName} {request.BirthDate.ToDateTime()}");
        var owner = new Data.Entities.Owner()
        {
            FullName = request.FullName,
            BirthDate = request.BirthDate.ToDateTime()
        };
        var vehicleNumber = _db.GetVehicleNumber(owner);

        if (string.IsNullOrEmpty(vehicleNumber))
            vehicleNumber = "Not Found";
        
        return Task.FromResult(new OwnerReply() { VehicleNumber = vehicleNumber });
    }
}