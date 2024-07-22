using Domain.Invoices;

namespace Application.Invoices.Queries;

public class InvoiceQueryService(IInvoiceRepository invoiceRepository)
{
    private readonly IInvoiceRepository invoiceRepository = invoiceRepository;

    public IEnumerable<InvoiceDto> GetInvoices() => invoiceRepository
        .GetAllInvoices()
        .Select(invoice => new InvoiceDto(invoice));
}
