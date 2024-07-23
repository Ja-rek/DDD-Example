namespace Domain.Invoices;

public interface IInvoiceRepository
{
    void Save(Invoice invoice);
    Invoice? GetInvoice(Guid clientId, int month, int year);
    IEnumerable<Invoice> GetInvoices(Guid clientId, int month, int year);
}
