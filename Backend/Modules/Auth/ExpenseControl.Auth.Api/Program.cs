using ExpenseControl.Auth.Api.Services;
using ExpenseControl.Auth.Application.Common;
using ExpenseControl.Auth.Domain.Interfaces;
using ExpenseControl.Auth.Infrastructure.Context;
using ExpenseControl.Auth.Infrastructure.Repositories;
using ExpenseControl.Auth.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("AuthDb")));

builder.Services.AddScoped<IUserRepository,       UserRepository>();
builder.Services.AddScoped<IAccountRepository,    AccountRepository>();
builder.Services.AddScoped<IInvitationRepository, InvitationRepository>();
builder.Services.AddScoped<IJwtService,           JwtService>();
builder.Services.AddScoped<IPasswordHasher,       BcryptPasswordHasher>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(ExpenseControl.Auth.Application.Commands.Users.RegisterUserCommand).Assembly));

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<AuthGrpcService>();
app.MapGet("/", () => "Auth gRPC service.");

app.Run();
