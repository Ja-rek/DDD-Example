namespace Application.Invoices.Commands;

public record GetInvoicesQuery(Guid ClientId, int Month, int Year);
