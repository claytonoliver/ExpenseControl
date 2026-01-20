using ExpenseControl.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseControl.Application;

/// <summary>
/// Extensão para registro dos serviços da camada Application.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        // Registra MediatR e seus handlers
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        // Registra todos os validators do FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        // Registra o behavior de validação no pipeline
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
