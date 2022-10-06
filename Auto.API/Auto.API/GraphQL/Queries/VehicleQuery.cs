using Auto.API.GraphQL.GraphTypes;
using Auto.Core.Entities;
using Auto.Data;
using GraphQL;
using GraphQL.Types;

namespace Auto.API.GraphQL.Queries;

public class VehicleQuery : ObjectGraphType {
    private readonly IAutoStorage _db;

    public VehicleQuery(IAutoStorage db) {
        this._db = db;

        Field<ListGraphType<VehicleGraphType>>("Vehicles", "Query to retrieve all Vehicles",
            resolve: GetAllVehicles);

        Field<VehicleGraphType>("Vehicle", "Query to retrieve a specific Vehicle",
            new QueryArguments(MakeNonNullStringArgument("registration", "The registration (licence plate) of the Vehicle")),
            resolve: GetVehicle);

        Field<ListGraphType<VehicleGraphType>>("VehiclesByColor", "Query to retrieve all Vehicles matching the specified color",
            new QueryArguments(MakeNonNullStringArgument("color", "The name of a color, eg 'blue', 'grey'")),
            resolve: GetVehiclesByColor);
    }

    private QueryArgument MakeNonNullStringArgument(string name, string description) {
        return new QueryArgument<NonNullGraphType<StringGraphType>> {
            Name = name, Description = description
        };
    }

    private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> context) => _db.ListVehicles();

    private Vehicle GetVehicle(IResolveFieldContext<object> context) {
        var registration = context.GetArgument<string>("registration");
        return _db.FindVehicle(registration);
    }

    private IEnumerable<Vehicle> GetVehiclesByColor(IResolveFieldContext<object> context) {
        var color = context.GetArgument<string>("color");
        var vehicles = _db.ListVehicles().Where(v => v.Color.Contains(color, StringComparison.InvariantCultureIgnoreCase));
        return vehicles;
    }
}