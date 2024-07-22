namespace Domain.Operations.Specyfications;

public sealed class CanPauseServiceSpecification : IAddOperationSpecification
{
    public SpecificationResult IsSatisfiedBy(IEnumerable<Operation> operations, OperationType newOperationType)
    {
        var orderedOperations = operations.OrderBy(o => o.OperationDate);
        var isSatisfied = newOperationType == OperationType.PauseService && orderedOperations.Last().OperationType != OperationType.StartService && orderedOperations.Last().OperationType != OperationType.ResumeService;
        var message = isSatisfied ? "Cannot pause service unless last operation is start or resume." : string.Empty;

        return new SpecificationResult(!isSatisfied, message);
    }
}
