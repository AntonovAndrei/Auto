using Auto.Data;
using Auto.Data.Entities;
using Auto.Website.GraphQl.GraphType;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Website.GraphQl.Queries
{
    public class OwnerQuery : ObjectGraphType
    {
        private readonly IAutoDatabase _db;

        public OwnerQuery(IAutoDatabase db)
        {
            this._db = db;

            Field<ListGraphType<OwnerGraphType>>("Owners", "Query to retrieve all Owners",
                resolve: GetAllOwners);

            Field<OwnerGraphType>("Owner", "Query to retrieve a specific Owner",
                new QueryArguments(MakeNonNullStringArgument("id", "The owner identificator")),
                resolve: GetOwner);

            Field<ListGraphType<VehicleGraphType>>("OwnerByFullName", "Query to retrieve all Vehicles matching the specified color",
            new QueryArguments(MakeNonNullStringArgument("fullname", "The fullname of person who have vehicle")),
            resolve: GetOwnerByFullName);
        }

        private QueryArgument MakeNonNullStringArgument(string name, string description)
        {
            return new QueryArgument<NonNullGraphType<StringGraphType>>
            {
                Name = name,
                Description = description
            };
        }

        private IEnumerable<Owner> GetAllOwners(IResolveFieldContext<object> context) => _db.ListOwners();

        private Owner GetOwner(IResolveFieldContext<object> context)
        {
            var id = context.GetArgument<int>("id");
            return _db.FindOwner(id);
        }

        private IEnumerable<Owner> GetOwnerByFullName(IResolveFieldContext<object> context)
        {
            var fullName = context.GetArgument<string>("fullname");
            var owners = _db.ListOwners().Where(v => v.FullName.Contains(fullName, StringComparison.InvariantCultureIgnoreCase));
            return owners;
        }
    }
}
