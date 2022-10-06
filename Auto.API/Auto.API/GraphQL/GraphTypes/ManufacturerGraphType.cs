using Auto.Core.Entities;
using GraphQL.Types;

namespace Auto.API.GraphQL.GraphTypes;

public sealed class ManufacturerGraphType : ObjectGraphType<Manufacturer> {
    public ManufacturerGraphType() {
        Name = "manufacturer";
        Field(c => c.Name).Description("The name of the manufacturer, e.g. Tesla, Volkswagen, Ford");
    }
}