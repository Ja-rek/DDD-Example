namespace Domain.Operations;

public class Operation(Guid serviceId,
    Guid clientId,
    int quantity,
    decimal? pricePerDay,
    DateTime operationDate,
    OperationType operationType)
{
    public Guid Id { get; } = Guid.NewGuid();

    public Guid ServiceId { get; } = serviceId != Guid.Empty 
        ? serviceId 
        : throw new ArgumentException($"{nameof(serviceId)} cannot be empty", nameof(serviceId));

    public Guid ClientId { get; } = clientId != Guid.Empty 
        ? clientId 
        : throw new ArgumentException($"{nameof(clientId)} cannot be empty", nameof(clientId));

    public int Quantity { get; } = quantity > 0 
        ? quantity 
        : throw new ArgumentException($"{nameof(Quantity)} must be greater than zero", nameof(quantity));

    public decimal? PricePerDay { get; } = pricePerDay;

    public DateTime OperationDate { get; } = operationDate != default 
        ? operationDate 
        : throw new ArgumentException($"{nameof(operationDate)} cannot be the default value", nameof(operationDate));

    public OperationType OperationType { get; } = Enum.IsDefined(typeof(OperationType), operationType) 
        ? operationType 
        : throw new ArgumentException($"{nameof(operationDate)} operation type", nameof(operationType));
}

