using ExpenseControl.Application.Behaviors;
using ExpenseControl.Application.Commands.Categories;
using ExpenseControl.Application.Validators;
using ExpenseControl.Domain.Interfaces;
using ExpenseControl.Infrastructure.Context;
using ExpenseControl.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseControl.IOC.Configuration
{
    /// <summary>
    /// Classe de extensão para registrar os serviços da aplicação e infraestrutura no contêiner de injeção de dependência e deixar centralizado.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceCollectionExtensions(this IServiceCollection services, IConfiguration configuration) =>
            services.AddApplication()
            .AddInfrastructure(configuration);

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCategoryCommand).Assembly));

            services.AddValidatorsFromAssembly(typeof(CreateCategoryCommandValidator).Assembly);

            // Registra o behavior de validação no pipeline
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }

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
}
