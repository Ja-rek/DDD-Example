using Domain.Operations;
using FluentAssertions;

namespace UnitTests;

public class OperationTests
{
    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenServiceIdIsEmpty()
    {
        // Arrange
        var serviceId = Guid.Empty;
        var clientId = Guid.NewGuid();
        var quantity = 5;
        var pricePerDay = 100m;
        var operationDate = new DateTime(2023, 7, 18);
        var operationType = OperationType.StartService;

        // Act
        Action act = () => new Operation(serviceId, clientId, quantity, pricePerDay, operationDate, operationType);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("serviceId cannot be empty*");
    }

    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenClientIdIsEmpty()
    {
        // Arrange
        var serviceId = Guid.NewGuid();
        var clientId = Guid.Empty;
        var quantity = 5;
        var pricePerDay = 100m;
        var operationDate = new DateTime(2023, 7, 18);
        var operationType = OperationType.StartService;

        // Act
        Action act = () => new Operation(serviceId, clientId, quantity, pricePerDay, operationDate, operationType);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("clientId cannot be empty*");
    }

    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenQuantityIsLessThanOrEqualToZero()
    {
        // Arrange
        var serviceId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var quantity = 0;
        var pricePerDay = 100m;
        var operationDate = new DateTime(2023, 7, 18);
        var operationType = OperationType.StartService;

        // Act
        Action act = () => new Operation(serviceId, clientId, quantity, pricePerDay, operationDate, operationType);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Quantity must be greater than zero*");
    }

    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenOperationDateIsDefault()
    {
        // Arrange
        var serviceId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var quantity = 5;
        var pricePerDay = 100m;
        var operationDate = default(DateTime);
        var operationType = OperationType.StartService;

        // Act
        Action act = () => new Operation(serviceId, clientId, quantity, pricePerDay, operationDate, operationType);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("operationDate cannot be the default value*");
    }

    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenOperationTypeIsInvalid()
    {
        // Arrange
        var serviceId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var quantity = 5;
        var pricePerDay = 100m;
        var operationDate = new DateTime(2023, 7, 18);
        var operationType = (OperationType)999;

        // Act
        Action act = () => new Operation(serviceId, clientId, quantity, pricePerDay, operationDate, operationType);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("operationDate operation type*");
    }

    [Test]
    public void Constructor_ShouldInitializeWithGivenValues()
    {
        // Arrange
        var serviceId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var quantity = 5;
        var pricePerDay = 100m;
        var operationDate = new DateTime(2023, 7, 18);
        var operationType = OperationType.StartService;

        // Act
        var operation = new Operation(serviceId, clientId, quantity, pricePerDay, operationDate, operationType);

        // Assert
        operation.Id.Should().NotBeEmpty();
        operation.ServiceId.Should().Be(serviceId);
        operation.ClientId.Should().Be(clientId);
        operation.Quantity.Should().Be(quantity);
        operation.PricePerDay.Should().Be(pricePerDay);
        operation.OperationDate.Should().Be(operationDate);
        operation.OperationType.Should().Be(operationType);
    }

    [Test]
    public void Constructor_ShouldInitializeWithNullPricePerDay()
    {
        // Arrange
        var serviceId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var quantity = 5;
        decimal? pricePerDay = null;
        var operationDate = new DateTime(2023, 7, 18);
        var operationType = OperationType.StartService;

        // Act
        var operation = new Operation(serviceId, clientId, quantity, pricePerDay, operationDate, operationType);

        // Assert
        operation.Id.Should().NotBeEmpty();
        operation.ServiceId.Should().Be(serviceId);
        operation.ClientId.Should().Be(clientId);
        operation.Quantity.Should().Be(quantity);
        operation.PricePerDay.Should().BeNull();
        operation.OperationDate.Should().Be(operationDate);
        operation.OperationType.Should().Be(operationType);
    }
}

