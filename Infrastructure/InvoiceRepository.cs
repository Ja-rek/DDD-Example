using Domain.Invoices;

namespace Infrastructure;

public class InvoiceRepository : IInvoiceRepository
{
    private static readonly List<Invoice> _invoices = new List<Invoice>();

    public void Save(Invoice invoice)
    {
        _invoices.Add(invoice);
    }

    public Invoice? GetInvoice(Guid clientId, int month, int year)
    {
        return _invoices.FirstOrDefault(inv => inv.ClientId == clientId && inv.CreationDate.Month == month && inv.CreationDate.Year == year);
    }

    public IEnumerable<Invoice> GetAllInvoices()
    {
        return _invoices;
    }
}
