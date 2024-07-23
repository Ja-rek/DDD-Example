namespace Domain.Operations.Specyfications;

public sealed class CanResumeServiceSpecification : AddOperationSpecificationBase, IAddOperationSpecification
{
    public override SpecificationResult IsSatisfiedBy(IEnumerable<Operation> operations, OperationType newOperationType)
    {
        var lastOperationType = GetLastOperationType(operations);

        var isSatisfied = newOperationType == OperationType.ResumeService 
            && lastOperationType != OperationType.PauseService;

        var message = GetMessage(isSatisfied, "Cannot resume service unless last operation is pause.");

        return new SpecificationResult(!isSatisfied, message);
    }
}
