using Auto.Core.Entities;
using Auto.Data;

namespace Auto.LazyAPI.Mutation;

public class VehicleMutation
{
    private readonly IAutoStorage _db;

    public VehicleMutation(IAutoStorage db)
    {
        _db = db;
    }

    public async Task<Vehicle> CreateVehicle(string registration, string color, int year, string modelCode)
    {
       var vehicleModel = _db.FindModel(modelCode);
        var vehicle = new Vehicle
        {
            Registration = registration,
            Color = color,
            Year = year,
            VehicleModel = vehicleModel,
            ModelCode = vehicleModel.Code
        };
        _db.CreateVehicle(vehicle);
        return vehicle;
    }
}