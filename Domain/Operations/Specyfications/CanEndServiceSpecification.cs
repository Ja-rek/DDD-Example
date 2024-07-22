namespace Domain.Operations.Specyfications;

public sealed class CanEndServiceSpecification : IAddOperationSpecification
{
    public SpecificationResult IsSatisfiedBy(IEnumerable<Operation> operations, OperationType newOperationType)
    {
        var orderedOperations = operations.OrderBy(o => o.OperationDate);
        var isSatisfied = newOperationType == OperationType.EndService && orderedOperations.Last().OperationType != OperationType.StartService && orderedOperations.Last().OperationType != OperationType.ResumeService;
        var message = isSatisfied ? "Cannot end service unless last operation is start or resume." : string.Empty;

        return new SpecificationResult(!isSatisfied, message);
    }
}
