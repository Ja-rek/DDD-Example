using Domain.Operations;
using System.Collections.ObjectModel;

namespace Infrastructure;

public class OperationRepository : IOperationRepository
{
    private static readonly Collection<Operation> operations = new Collection<Operation>();

    public void Save(Operation operation)
    {
        operations.Add(operation);
    }

    public IEnumerable<Guid> GetAllClientsWithOperations()
    {
        return operations.Select(o => o.ClientId).Distinct();
    }

    public IEnumerable<Operation> GetOperationsForClientInMonth(Guid clientId, int month, int year)
    {
        return operations
            .Where(o => o.ClientId == clientId && o.OperationDate.Month == month && o.OperationDate.Year == year)
            .OrderBy(o => o.OperationDate);
    }

    public IEnumerable<Operation> GetOperationsForClientAndService(Guid clientId, Guid serviceId)
    {
        return operations
            .Where(o => o.ClientId == clientId && o.ServiceId == serviceId)
            .OrderBy(o => o.OperationDate);
    }
}
