namespace Application.Invoices.Queries;

public record GetInvoicesQuery(Guid ClientId, int Month, int Year);
