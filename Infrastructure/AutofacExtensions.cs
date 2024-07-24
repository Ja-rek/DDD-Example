using Application.Invoices.Queries;
using Autofac;
using Domain.Invoices;
using Domain.Operations.Specyfications;
using System.Reflection;

namespace Infrastructure;

public static class AutofacExtensions
{
    public static void RegisterServices(this ContainerBuilder builder)
    {
        string[] types = ["Repository", "Service", "Factory"];
        Assembly[] assemblies =
        [
            typeof(InvoiceQueryService).Assembly,
            typeof(InvoiceGeneratorService).Assembly,
            typeof(InvoiceRepository).Assembly
        ];

        foreach (var assembly in assemblies)
        {
            foreach (var type in types)
            {
                RegisterServices(builder, assembly, type);
            }
        }

        RegisterSpecification(builder);
    }

    private static void RegisterSpecification(ContainerBuilder builder)
    {
        var assembly = typeof(InvoiceGeneratorService).Assembly;
        const string key = "a";

        builder.RegisterAssemblyTypes(assembly)
               .Where(t => t.Name.EndsWith("Specification")
                    && !t.IsAbstract
                    && t.Name != nameof(CanAddOperationSpecification))
               .Keyed<IAddOperationSpecification>(key)
               .AsImplementedInterfaces();

        builder.Register(ctx => new CanAddOperationSpecification(ctx.ResolveKeyed<IEnumerable<IAddOperationSpecification>>(key)))
               .AsImplementedInterfaces();
    }

    private static void RegisterServices(ContainerBuilder builder, Assembly assembly, string suffix)
    {
        builder.RegisterAssemblyTypes(assembly)
               .Where(t => t.Name.EndsWith(suffix) && !t.IsAbstract)
               .AsImplementedInterfaces()
               .AsSelf();
    }
}

