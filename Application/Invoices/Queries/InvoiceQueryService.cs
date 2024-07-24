using System.Globalization;
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
            .Select(invoice => new InvoiceDto {
                Id = invoice.Id,
                ClientId = invoice.ClientId,
                CreationDate = ConvertToPolishLocalTime(invoice.CreationDate),
                Items = invoice.Items
            });
    }

    private string ConvertToPolishLocalTime(DateTime CreationDate)
    {
        TimeZoneInfo polishTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

        return TimeZoneInfo.ConvertTimeFromUtc(CreationDate, polishTimeZone).ToString(new CultureInfo("pl-PL"));
    }
}
