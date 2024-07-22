namespace Domain.Operations;

public interface IOperationRepository
{
    void Save(Operation operation);
    IEnumerable<Guid> GetAllClientsWithOperations();
    IEnumerable<Operation> GetOperationsForClientInMonth(Guid clientId, int month, int year);
    IEnumerable<Operation> GetOperationsForClientAndService(Guid clientId, Guid serviceId);
}
