using Domain.Invoices;

namespace Infrastructure;

public class InvoiceRepository : IInvoiceRepository
{
    private static readonly List<Invoice> invoices = new List<Invoice>();

    public void Save(Invoice invoice)
    {
        invoices.Add(invoice);
    }

    public Invoice? GetInvoice(Guid clientId, int month, int year)
    {
        return invoices.FirstOrDefault(inv => inv.ClientId == clientId && inv.CreationDate.Month == month && inv.CreationDate.Year == year);
    }

    public IEnumerable<Invoice> GetInvoices(Guid clientId, int month, int year)
    {
        return invoices.Where(invoice => invoice.ClientId == clientId 
            && invoice.CreationDate.Month == month 
            && invoice.CreationDate.Year == year);
    }
}
