using Application.Invoices.Commands;
using Application.Invoices.Queries;
using Application.Operations.Commands;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterServices();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/invoice/calculate", (InvoiceCommandService invoiceCommandService, CalculateInvoicesCommand cmd) => 
    invoiceCommandService.CalculateInvoices(cmd));

app.MapGet("/invoice", (InvoiceQueryService invoiceQueryService, [FromBody]GetInvoicesQuery query) => invoiceQueryService.GetInvoices(query));

app.MapPost("/operation", (OperationCommandService operationCommandService, AddOperationCommand cmd) =>
{
    operationCommandService.AddOperation(cmd);
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();
