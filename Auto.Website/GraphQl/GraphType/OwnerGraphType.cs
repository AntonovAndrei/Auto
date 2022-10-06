using Auto.Data.Entities;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Website.GraphQl.GraphType
{
    public class OwnerGraphType : ObjectGraphType<Owner>
    {
        public OwnerGraphType()
        {
            Name = "owner";
            Field(o => o.Vehicle, nullable: false, type: typeof(VehicleGraphType)).Description("Owners vehicle");
            Field(o => o.VehicleId);
            Field(o => o.Id);
            Field(o => o.FullName);
            Field(o => o.BirthDate);
        }
    }
}
