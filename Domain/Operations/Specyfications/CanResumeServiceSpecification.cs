namespace Domain.Operations.Specyfications;

public sealed class CanResumeServiceSpecification : IAddOperationSpecification
{
    public SpecificationResult IsSatisfiedBy(IEnumerable<Operation> operations, OperationType newOperationType)
    {
        var orderedOperations = operations.OrderBy(o => o.OperationDate);
        var isSatisfied = newOperationType == OperationType.ResumeService && orderedOperations.Last().OperationType != OperationType.PauseService;
        var message = isSatisfied ? "Cannot resume service unless last operation is pause." : string.Empty;

        return new SpecificationResult(!isSatisfied, message);
    }
}
