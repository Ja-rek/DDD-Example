using Domain.Invoices;
using System.Globalization;

namespace Application.Invoices.Queries;

public class InvoiceDto(Invoice Invoice)
{
    public Guid Id { get; } = Invoice.Id;
    public Guid ClientId { get; } = Invoice.ClientId;
    public string CreationDate { get; } = Invoice.CreationDate.ToString(new CultureInfo("pl-PL"));
    public IEnumerable<InvoiceItem> Items { get; } = Invoice.Items;
}
