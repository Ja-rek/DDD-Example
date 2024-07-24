using Domain.Operations.Specifications;

namespace Domain.Operations;

public class OperationFactory(IAddOperationSpecification canAddOperationSpecification)
{
    private readonly IAddOperationSpecification canAddOperationSpecification = canAddOperationSpecification;
    
    public Operation Operation(Guid serviceId, 
        Guid clientId, 
        int quantity, 
        decimal? pricePerDay, 
        DateTime operationDate, 
        OperationType operationType,
        IEnumerable<Operation> operations)
    {
        var specResult = canAddOperationSpecification.IsSatisfiedBy(operations, operationType);
        
        if(!specResult.IsSatisfied)
        {
            throw new InvalidOperationException(specResult.ErrorMessage);
        }

        return new Operation(serviceId, clientId, quantity, pricePerDay, operationDate, operationType);
    }
}
