using ExpenseControl.Grpc.Services;
using ExpenseControl.IOC.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceCollectionExtensions(builder.Configuration);
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<PersonsGrpcService>();
app.MapGrpcService<CategoriesGrpcService>();
app.MapGrpcService<TransactionsGrpcService>();

app.MapGet("/", () => "gRPC only. Use a gRPC client to communicate with this service.");

app.Run();
