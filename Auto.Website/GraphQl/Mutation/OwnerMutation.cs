using Auto.Data;
using System;
using GraphQL;
using GraphQL.Types;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auto.Website.GraphQl.GraphType;
using Auto.Data.Entities;

namespace Auto.Website.GraphQl.Mutation
{
    public class OwnerMutation : ObjectGraphType
    {
        private readonly IAutoDatabase _db;

        public OwnerMutation(IAutoDatabase db)
        {
            _db = db;

            Field<OwnerGraphType>(
                "createOwner",
                arguments: new QueryArguments(
                        new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "fullname" },
                        new QueryArgument<NonNullGraphType<DateTimeGraphType>> { Name = "birthdate" },
                        new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" },
                        new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "vehicleid" }
                    ),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    var vehicleId = context.GetArgument<string>("vehicleid");
                    var birthDate = context.GetArgument<DateTime>("birthdate");
                    var fullname = context.GetArgument<string>("fullname");

                    var ownerVehicle = db.FindVehicle(vehicleId);
                    var owner = new Owner
                    {
                        Id = id,
                        VehicleId = vehicleId,
                        BirthDate = birthDate,
                        FullName = fullname,
                        Vehicle = ownerVehicle
                    };
                    _db.CreateOwner(owner);
                    return owner;
                }
            );

        }
    }
}
