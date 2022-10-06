using Auto.Data;
using Auto.Website.GraphQl.Mutation;
using Auto.Website.GraphQl.Queries;
using GraphQL.Introspection;
using GraphQL.Types;

namespace Auto.Website.GraphQl.Schemas
{
    public class AutoSchema : Schema
    {
        public AutoSchema(IAutoDatabase db)
        {
            Query = new OwnerQuery(db);
            Mutation = new OwnerMutation(db);
        }
    }
}
