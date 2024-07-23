using Application.Invoices.Commands;
using Application.Invoices.Queries;
using Application.Operations;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

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

app.MapGet("/invoice", (InvoiceQueryService invoiceQueryService, GetInvoicesQuery query) => invoiceQueryService.GetInvoices(query));

app.MapPost("/operation", (OperationCommandService operationCommandService, [FromBody]AddOperationCommand cmd) =>
{
    operationCommandService.AddOperation(cmd);
});


app.Run();
