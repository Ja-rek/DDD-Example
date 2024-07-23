using Domain.Operations;

namespace Domain.Invoices;

public class InvoiceCalculatorService(IInvoiceRepository invoiceRepository)
{
    private readonly IInvoiceRepository invoiceRepository = invoiceRepository;

    public Invoice Calculate(Guid clientId, IEnumerable<Operation> operations, int month, int year)
    {
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
                    AddInvoiceItem(invoice, operation, currentStart);
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

    private void AddInvoiceItem(Invoice invoice, Operation operation, DateTime currentStart)
    {
        var days = CalculateServiceDays(currentStart, operation.OperationDate);
        var value = CalculateServiceValue(operation, days);
        var isPaused = operation.OperationType == OperationType.PauseService;
        var item = new InvoiceItem(operation.ServiceId, currentStart, operation.OperationDate, value, isPaused);
        invoice.AddItem(item);
    }

    private int CalculateServiceDays(DateTime start, DateTime end)
    {
        if (start > end)
        {
            throw new ArgumentException($"Argument {nameof(start)} must be earlier than {nameof(end)}.");
        }

        return (end - start).Days + 1;
    }

    private decimal CalculateServiceValue(Operation operation, int days)
    {
        if (days == 0)
        {
            throw new ArgumentException($"Argument {nameof(days)} must be greater than 0.");
        }

        return operation.PricePerDay.GetValueOrDefault() * operation.Quantity * days;
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
