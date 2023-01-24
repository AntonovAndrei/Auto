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
        
        return Task.FromResult(new OwnerReply() { VehicleNumber = "3" });
    }
}