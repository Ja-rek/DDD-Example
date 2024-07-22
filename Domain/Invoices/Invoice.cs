namespace Domain.Invoices;

public class Invoice(Guid clientId)
{
    public Guid Id { get; } = Guid.NewGuid();
    public Guid ClientId { get;} = clientId != default 
        ? clientId 
        : throw new ArgumentException($"{clientId} must to have a value.");
    public DateTime CreationDate { get; } = DateTime.UtcNow;
    public List<InvoiceItem> Items { get; } = new List<InvoiceItem>();

    public void AddItem(InvoiceItem item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        Items.Add(item);
    }
}
