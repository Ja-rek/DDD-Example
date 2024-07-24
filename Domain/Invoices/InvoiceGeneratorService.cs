using Domain.Operations;

namespace Domain.Invoices;

public class InvoiceGeneratorService(IInvoiceRepository invoiceRepository)
{
    private readonly IInvoiceRepository invoiceRepository = invoiceRepository;

    public Invoice? TryGenerateInvoice(Guid clientId, IEnumerable<Operation> operations, int month, int year)
    {
        if(!operations.Any())
        {
            return null;
        }

        ValidateLastOperationIsEndService(operations);
        EnsureNoExistingInvoice(clientId, month, year);

        var invoice = new Invoice(clientId);
        var currentStart = DateTime.MinValue;

        foreach (var operation in operations)
        {
            if (IsServiceStartOrResume(operation))
            {
                currentStart = operation.OperationDate;
            }
            else if (IsServicePauseOrEnd(operation))
            {
                if (currentStart != DateTime.MinValue)
                {
                    invoice.AddItemWithContinuousRenditionPeriod(operation, currentStart);
                    currentStart = DateTime.MinValue;
                }
            }
        }

        return invoice;
    }

    private bool IsServiceStartOrResume(Operation operation)
    {
        return operation.OperationType == OperationType.StartService || operation.OperationType == OperationType.ResumeService;
    }

    private bool IsServicePauseOrEnd(Operation operation)
    {
        return operation.OperationType == OperationType.PauseService || operation.OperationType == OperationType.EndService;
    }

    private void ValidateLastOperationIsEndService(IEnumerable<Operation> operations)
    {
        var lastOperation = operations.Last();
        if (lastOperation.OperationType != OperationType.EndService)
        {
            throw new InvalidOperationException("Cannot generate invoice for client unless the last operation is ending the service.");
        }
    }

    private void EnsureNoExistingInvoice(Guid clientId, int month, int year)
    {
        var existingInvoice = invoiceRepository.GetInvoice(clientId, month, year);
        if (existingInvoice != null)
        {
            throw new InvalidOperationException("Invoice for this client and period already exists.");
        }
    }
}
