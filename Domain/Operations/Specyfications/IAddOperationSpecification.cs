namespace Domain.Operations.Specifications;

public interface IAddOperationSpecification
{
    SpecificationResult IsSatisfiedBy(IEnumerable<Operation> operations, OperationType newOperationType);
}
