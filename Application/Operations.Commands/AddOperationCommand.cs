using Domain.Operations;

namespace Application.Operations;

public record AddOperationCommand(Guid ServiceId,
    Guid ClientId,
    int Quantity,
    decimal? PricePerDay,
    DateTime OperationDate,
    OperationType OperationType);
