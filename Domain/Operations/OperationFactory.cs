using Domain.Operations.Specyfications;

namespace Domain.Operations;

public class OperationFactory(IAddOperationSpecification canAddOperationSpecyfication)
{
    private readonly IAddOperationSpecification canAddOperationSpecyfication = canAddOperationSpecyfication;
    
    public Operation Operation(Guid serviceId, 
        Guid clientId, 
        int quantity, 
        decimal? pricePerDay, 
        DateTime operationDate, 
        OperationType operationType,
        IEnumerable<Operation> operations)
    {
        var specResult = canAddOperationSpecyfication.IsSatisfiedBy(operations, operationType);
        
        if(!specResult.IsSatisfied)
        {
            throw new InvalidOperationException(specResult.ErrorMessage);
        }

        return new Operation(serviceId, clientId, quantity, pricePerDay, operationDate, operationType);
    }
}
