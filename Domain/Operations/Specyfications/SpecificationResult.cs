namespace Domain.Operations.Specyfications;

public readonly ref struct SpecificationResult(bool isSatisfied, string errorMessage = "")
{
    public bool IsSatisfied { get; } = isSatisfied;
    public string ErrorMessage { get; } = errorMessage;
}
