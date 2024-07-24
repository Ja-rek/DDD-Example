namespace Domain.Operations.Specifications;

public readonly ref struct SpecificationResult(bool isSatisfied, string errorMessage = "")
{
    public bool IsSatisfied { get; } = isSatisfied;
    public string ErrorMessage { get; } = errorMessage;
}
