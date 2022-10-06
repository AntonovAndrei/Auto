using Auto.Data;
using Auto.LazyAPI.Mutation;
using Auto.LazyAPI.Queries;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IAutoStorage, AutoCsvFileStorage>();
builder.Services
    .AddGraphQLServer()
    .AddQueryType<VehicleQuery>()
    .AddMutationType<VehicleMutation>();

var app = builder.Build();

app.MapGraphQL();

app.Run();