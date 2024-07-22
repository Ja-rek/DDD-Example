using Application.Invoices.Queries;
using Domain.Invoices;
using Domain.Operations.Specyfications;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        string[] types = ["Repository", "Service", "Specification", "Factory"];
        Assembly[] assemblies = 
        [
            typeof(InvoiceQueryService).Assembly, 
            typeof(InvoiceCalculatorService).Assembly, 
            typeof(InvoiceRepository).Assembly 
        ];

        foreach (var asembly in assemblies)
        {
            foreach (var type in types)
            {
                RegisterServices(services, asembly, type);
            }
        }

        services.AddScoped<IAddOperationSpecification, CanAddOperationSpecification>(x => 
            new CanAddOperationSpecification(x.GetRequiredService<IEnumerable<IAddOperationSpecification>>()));

        return services;
    }

    private static void RegisterServices(IServiceCollection services, Assembly assembly, string suffix)
    {
        var types = assembly.GetTypes();
        var implementations = types.Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith(suffix)).ToList();

        foreach (var implementationType in implementations)
        {
            var interfaceType = implementationType.GetInterfaces()
                .FirstOrDefault();
            
            if (implementationType == typeof(CanAddOperationSpecification))
            {
                break;
            }

            if (interfaceType != null)
            {
                services.AddScoped(interfaceType, implementationType);
            }
            else
            {
                services.AddScoped(implementationType);
            }
        }
    }
}
