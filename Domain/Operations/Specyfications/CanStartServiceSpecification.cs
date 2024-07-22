namespace Domain.Operations.Specyfications;

public sealed class CanStartServiceSpecification : IAddOperationSpecification
{
    public SpecificationResult IsSatisfiedBy(IEnumerable<Operation> operations, OperationType newOperationType)
    {
        var orderedOperations = operations.OrderBy(o => o.OperationDate);
        var isSatisfied = newOperationType == OperationType.StartService && orderedOperations.Any() && orderedOperations.Last().OperationType != OperationType.EndService;
        var message = isSatisfied ? "Cannot start service when last operation is not ending the service." : string.Empty;

        return new SpecificationResult(!isSatisfied, message);
    }
}
