using Domain.Operations;

namespace Application.Operations;

public class OperationCommandService(IOperationRepository operationRepository, 
    OperationFactory operationFactory)
{
    private readonly IOperationRepository operationRepository = operationRepository;
    private readonly OperationFactory operationFactory = operationFactory;

    public void AddOperation(AddOperationCommand cmd)
    {
        var (ServiceId, ClientId, Quantity, PricePerDay, OperationDate, OperationType) = cmd;

        var operations = operationRepository.GetOperationsForClientAndService(ClientId, ServiceId);
        var operation = operationFactory.Operation(ServiceId,
            ClientId,
            Quantity,
            PricePerDay,
            OperationDate,
            OperationType,
            operations);

        operationRepository.Save(operation);
    }
}

