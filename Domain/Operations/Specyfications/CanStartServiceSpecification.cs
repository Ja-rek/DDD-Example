namespace Domain.Operations.Specifications;

public sealed class CanStartServiceSpecification : AddOperationSpecificationBase, IAddOperationSpecification
{
    public override SpecificationResult IsSatisfiedBy(IEnumerable<Operation> operations, OperationType newOperationType)
    {
        var lastOperationType = GetLastOperationType(operations);

        var isSatisfied = newOperationType == OperationType.StartService 
            && lastOperationType != OperationType.EndService;

        var message = GetMessage(isSatisfied, "Cannot start service when last operation is not ending the service.");

        return new SpecificationResult(!isSatisfied, message);
    }
}
