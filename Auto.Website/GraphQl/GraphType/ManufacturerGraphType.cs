using Auto.Data.Entities;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Website.GraphQl.GraphType
{
    public class ManufacturerGraphType : ObjectGraphType<Manufacturer>
    {
        public ManufacturerGraphType()
        {
            Name = "manufacturer";
            Field(c => c.Name).Description("The name of the manufacturer, e.g. Tesla, Volkswagen, Ford");
        }
    }
}
