using Auto.Core.Entities;
using Auto.Data;

namespace Auto.LazyAPI.Queries;

public class VehicleQuery {
    private readonly IAutoStorage _db;

    public VehicleQuery(IAutoStorage db) {
        this._db = db;
    }
    
    public IEnumerable<Vehicle> GetVehicles() => _db.ListVehicles();

    public Vehicle GetVehicle(string registration) => _db.FindVehicle(registration);

    public IEnumerable<Vehicle> GetVehiclesByColor(string color) => 
        _db.ListVehicles().Where(v => v.Color.Contains(color, StringComparison.InvariantCultureIgnoreCase));
      
}