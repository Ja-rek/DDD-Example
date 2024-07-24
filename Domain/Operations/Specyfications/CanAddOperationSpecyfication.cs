namespace Domain.Operations.Specifications;

public class CanAddOperationSpecification(IEnumerable<IAddOperationSpecification> specifications) : IAddOperationSpecification
{
    private readonly IEnumerable<IAddOperationSpecification> specifications = specifications;

    public SpecificationResult IsSatisfiedBy(IEnumerable<Operation> operations, OperationType newOperationType)
    {
        if (!operations.Any())
        {
            return new SpecificationResult(true);
        }

        foreach (var specification in specifications)
        {
            var specificationResult = specification.IsSatisfiedBy(operations, newOperationType);
            if (!specificationResult.IsSatisfied)
            {
                return specificationResult;
            }
        }

        return new SpecificationResult(true);
    }
}
