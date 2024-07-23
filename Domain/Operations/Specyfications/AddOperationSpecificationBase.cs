namespace Domain.Operations.Specyfications;

public abstract class  AddOperationSpecificationBase
{
    abstract public SpecificationResult IsSatisfiedBy(IEnumerable<Operation> operations, OperationType newOperationType);

    protected OperationType GetLastOperationType(IEnumerable<Operation> operations) => 
        operations.OrderBy(o => o.OperationDate)
            .Last()
            .OperationType;

    protected string GetMessage(bool isSatisfied, string message) => 
        isSatisfied ? message : string.Empty;
}
