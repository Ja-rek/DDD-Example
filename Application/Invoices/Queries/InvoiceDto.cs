using Domain.Invoices;

namespace Application.Invoices.Queries;

public class InvoiceDto()
{
    public required Guid Id { get; set; }
    public required Guid ClientId { get; set; }
    public required string CreationDate { get; set; }
    public required IEnumerable<InvoiceItem> Items { get; set; }
}
