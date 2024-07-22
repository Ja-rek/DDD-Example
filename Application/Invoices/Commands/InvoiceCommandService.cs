using Domain.Invoices;
using Domain.Operations;

namespace Application.Invoices.Commands;

public class InvoiceCommandService(IOperationRepository operationRepository,
    InvoiceCalculatorService invoiceCalculator,
    IInvoiceRepository invoiceRepository)
{
    private readonly IOperationRepository operationRepository = operationRepository;
    private readonly InvoiceCalculatorService invoiceCalculator = invoiceCalculator;
    private readonly IInvoiceRepository invoiceRepository = invoiceRepository;

    public void CalculateInvoices(CalculateInvoicesCommand cmd)
    {
        var (Month, Year) = cmd;
        var clients = operationRepository.GetAllClientsWithOperations();

        foreach (var clientId in clients)
        {
            var operations = operationRepository.GetOperationsForClientInMonth(clientId,
                Month,
                Year);

            if (!operations.Any())
                continue;

            var invoice = invoiceCalculator.Calculate(clientId,
                operations,
                Month,
                Year);

            invoiceRepository.Save(invoice);
        }
    }
}
