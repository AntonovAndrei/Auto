using Auto.Data.Entities;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Website.GraphQl.GraphType
{
    public class VehicleGraphType : ObjectGraphType<Vehicle>
    {
        public VehicleGraphType()
        {
            Name = "vehicle";
            Field(c => c.VehicleModel, nullable: false, type: typeof(ModelGraphType))
                .Description("The model of this particular vehicle");
            Field(c => c.Registration);
            Field(c => c.Color);
            Field(c => c.Year);
        }
    }
}
