using Domain.Invoices;
using Domain.Operations;

namespace Application.Invoices.Commands;

public class InvoiceCommandService(IOperationRepository operationRepository,
    InvoiceGeneratorService invoiceGenerator,
    IInvoiceRepository invoiceRepository)
{
    private readonly IOperationRepository operationRepository = operationRepository;
    private readonly InvoiceGeneratorService invoiceGenerator = invoiceGenerator;
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

            var invoice = invoiceGenerator.TryGenerateInvoice(clientId,
                operations,
                Month,
                Year);

            if (invoice != null)
            {
                invoiceRepository.Save(invoice);
            }
        }
    }
}
