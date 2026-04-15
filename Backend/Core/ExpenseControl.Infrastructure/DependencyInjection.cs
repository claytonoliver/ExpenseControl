using ExpenseControl.Application.Interfaces;
using ExpenseControl.Infrastructure.Context;
using ExpenseControl.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseControl.Infrastructure;

/// <summary>
/// Extensão para registro dos serviços da camada Infrastructure.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configura o DbContext com SQL Server
        services.AddDbContext<ExpenseControlContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ExpenseControlContext).Assembly.FullName)));

        // Registra os repositories
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        return services;
    }
}
