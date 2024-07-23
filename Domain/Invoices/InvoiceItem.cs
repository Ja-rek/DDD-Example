namespace Domain.Invoices;

public record InvoiceItem
{
    public InvoiceItem(Guid serviceId, DateTime startDate, DateTime endDate, decimal value, bool isPaused)
    {
        ServiceId = serviceId != default 
            ? serviceId 
            : throw new ArgumentException($"{nameof(ServiceId)} must have a value.", nameof(serviceId));

        StartDate = startDate != default 
            ? startDate 
            : throw new ArgumentException($"{nameof(StartDate)} must have a value.", nameof(startDate));

        EndDate = endDate != default 
            ? endDate 
            : throw new ArgumentException($"{nameof(EndDate)} must have a value.", nameof(endDate));

        if (startDate > endDate)
        {
            throw new ArgumentException($"{nameof(startDate)} must be earlier than {nameof(endDate)}.");
        }

        Value = value > 0 
            ? value 
            : throw new ArgumentException($"{nameof(Value)} must be greater than zero.", nameof(value));

        IsPaused = isPaused;
    }

    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public decimal Value { get; }
    public bool IsPaused { get; }
    public Guid ServiceId { get; }
}
