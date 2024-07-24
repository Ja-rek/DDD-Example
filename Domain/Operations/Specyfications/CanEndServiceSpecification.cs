namespace Domain.Operations.Specifications;

public sealed class CanEndServiceSpecification : AddOperationSpecificationBase, IAddOperationSpecification
{
    public override SpecificationResult IsSatisfiedBy(IEnumerable<Operation> operations, OperationType newOperationType)
    {
        var lastOperationType = GetLastOperationType(operations);

        var isSatisfied = newOperationType == OperationType.EndService 
            && lastOperationType != OperationType.StartService 
            && lastOperationType != OperationType.ResumeService;

        var message = GetMessage(isSatisfied, "Cannot end service unless last operation is start or resume.");

        return new SpecificationResult(!isSatisfied, message);
    }

}
