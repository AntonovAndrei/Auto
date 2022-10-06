using Auto.API.GraphQL.GraphTypes;
using Auto.Core.Entities;
using Auto.Data;
using GraphQL;
using GraphQL.Types;

namespace Auto.API.GraphQL.Mutations;

public class VehicleMutation: ObjectGraphType
{
    private readonly IAutoStorage _db;

    public VehicleMutation(IAutoStorage db)
    {
        this._db = db;
        
        Field<VehicleGraphType>(
            "createVehicle",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "registration"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "color"},
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "year"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "modelCode"}
            ),
            resolve: context =>
            {
                var registration = context.GetArgument<string>("registration");
                var color = context.GetArgument<string>("color");
                var year = context.GetArgument<int>("year");
                var modelCode = context.GetArgument<string>("modelCode");

                var vehicleModel = db.FindModel(modelCode);
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
        );
    }
}