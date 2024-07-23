namespace Domain.Operations.Specyfications;

public interface IAddOperationSpecification
{
    SpecificationResult IsSatisfiedBy(IEnumerable<Operation> operations, OperationType newOperationType);
}
