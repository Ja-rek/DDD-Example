namespace Domain.Operations.Specyfications;

public sealed class CanPauseServiceSpecification : AddOperationSpecificationBase, IAddOperationSpecification
{
    public override SpecificationResult IsSatisfiedBy(IEnumerable<Operation> operations, OperationType newOperationType)
    {
        var lastOperationType = GetLastOperationType(operations);

        var isSatisfied = newOperationType == OperationType.PauseService 
            && lastOperationType != OperationType.StartService 
            && lastOperationType != OperationType.ResumeService;

        var message = GetMessage(isSatisfied, "Cannot pause service unless last operation is start or resume.");

        return new SpecificationResult(!isSatisfied, message);
    }
}
