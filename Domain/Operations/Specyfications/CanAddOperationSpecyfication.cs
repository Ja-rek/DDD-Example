namespace Domain.Operations.Specyfications;

public class CanAddOperationSpecification(IEnumerable<IAddOperationSpecification> specyfications) : IAddOperationSpecification
{
    private readonly IEnumerable<IAddOperationSpecification> specyfications = specyfications;

    public SpecificationResult IsSatisfiedBy(IEnumerable<Operation> operations, OperationType newOperationType)
    {
        if (!operations.Any())
        {
            return new SpecificationResult(true);
        }

        foreach (var specyfication in specyfications)
        {
            var specyficationResult = specyfication.IsSatisfiedBy(operations, newOperationType);
            if (!specyficationResult.IsSatisfied)
            {
                return specyficationResult;
            }
        }

        return new SpecificationResult(true);
    }
}
