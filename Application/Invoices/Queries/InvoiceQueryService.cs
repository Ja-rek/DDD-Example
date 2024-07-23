using Application.Invoices.Commands;
using Domain.Invoices;

namespace Application.Invoices.Queries;

public class InvoiceQueryService(IInvoiceRepository invoiceRepository)
{
    private readonly IInvoiceRepository invoiceRepository = invoiceRepository;

    public IEnumerable<InvoiceDto> GetInvoices(GetInvoicesQuery cmd)
    {
        var (ClientId, Month, Year) = cmd;

        return invoiceRepository
        .GetInvoices(ClientId, Month, Year)
        .Select(invoice => new InvoiceDto(invoice));
    }
}
