namespace Domain.Operations.Specifications;

public record SpecificationResult(bool isSatisfied, string errorMessage = "")
{
    public bool IsSatisfied { get; } = isSatisfied;
    public string ErrorMessage { get; } = errorMessage;
}
