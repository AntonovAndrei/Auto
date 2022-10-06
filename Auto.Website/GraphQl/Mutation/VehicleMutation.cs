using Auto.Data;
using Auto.Data.Entities;
using Auto.Website.GraphQl.GraphType;
using GraphQL;
using GraphQL.Types;

namespace Auto.Website.GraphQL.Mutations;

public class VehicleMutation: ObjectGraphType
{
    private readonly IAutoDatabase _db;

    public VehicleMutation(IAutoDatabase db)
    {
        this._db = db;
        
        Field<VehicleGraphType>(
            "createVehicle",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "registration"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "color"},
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "year"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "modelCode"},
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "ownerCode"}
            ),
            resolve: context =>
            {
                var registration = context.GetArgument<string>("registration");
                var color = context.GetArgument<string>("color");
                var year = context.GetArgument<int>("year");
                var modelCode = context.GetArgument<string>("modelCode");
                var ownerCode = context.GetArgument<int>("ownerCode");

                var vehicleModel = db.FindModel(modelCode);
                var vehicleOwner = db.FindOwner(ownerCode);
                var vehicle = new Vehicle
                {
                    Registration = registration,
                    Color = color,
                    Year = year,
                    VehicleModel = vehicleModel,
                    Owner = vehicleOwner,
                    ModelCode = vehicleModel.Code,
                    OwnerId = vehicleOwner.Id,
                };
                _db.CreateVehicle(vehicle);
                return vehicle;
            }
        );
    }
}