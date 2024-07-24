using Domain.Operations;

namespace Domain.Invoices;

public class Invoice(Guid clientId)
{
    public Guid Id { get; } = Guid.NewGuid();
    public Guid ClientId { get;} = clientId != default 
        ? clientId 
        : throw new ArgumentException($"{clientId} must to have a value.");
    public DateTime CreationDate { get; } = DateTime.UtcNow;
    public List<InvoiceItem> Items { get; } = new List<InvoiceItem>();

    public void AddInvoiceItemWithContinuousRenditionPeriod(Operation operation, DateTime operationStart)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));

        var item = InvoiceItem(operation, operationStart);

        Items.Add(item);
    }

    private InvoiceItem InvoiceItem(Operation operation, DateTime operationStart)
    {
        var days = CalculateRenditionPeriodDays(operationStart, operation.OperationDate);
        var value = CalculateServiceValue(operation, days);
        var isPaused = operation.OperationType == OperationType.PauseService;

        return new InvoiceItem(operation.ServiceId,
            operationStart,
            operation.OperationDate,
            value,
            isPaused);
    }

    private int CalculateRenditionPeriodDays(DateTime start, DateTime end)
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
}
