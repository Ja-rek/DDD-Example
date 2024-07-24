using Domain.Operations;
using Domain.Operations.Specifications;

namespace UnitTests;

public class OperationFactoryTests
{
    private Mock<IAddOperationSpecification> _canAddOperationSpecificationMock;
    private OperationFactory _operationFactory;

    [SetUp]
    public void SetUp()
    {
        _canAddOperationSpecificationMock = new Mock<IAddOperationSpecification>();
        _operationFactory = new OperationFactory(_canAddOperationSpecificationMock.Object);
    }

    [Test]
    public void Operation_ShouldThrowInvalidOperationException_WhenSpecificationIsNotSatisfied()
    {
        // Arrange
        var serviceId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var quantity = 10;
        decimal? pricePerDay = 100.0m;
        var operationDate = DateTime.Now;
        var operationType = OperationType.StartService;
        var operations = new List<Operation>();

        var specResult = new SpecificationResult(false, "Error message");

        _canAddOperationSpecificationMock
            .Setup(spec => spec.IsSatisfiedBy(operations, operationType))
            .Returns(specResult);

        // Act
        Action act = () => _operationFactory.Operation(serviceId, clientId, quantity, pricePerDay, operationDate, operationType, operations);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage(specResult.ErrorMessage);
    }

    [Test]
    public void Operation_ShouldReturnNewOperation_WhenSpecificationIsSatisfied()
    {
        // Arrange
        var serviceId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var quantity = 10;
        decimal? pricePerDay = 100.0m;
        var operationDate = DateTime.Now;
        var operationType = OperationType.StartService;
        var operations = new List<Operation>();

        var specResult = new SpecificationResult(true);
        _canAddOperationSpecificationMock
            .Setup(spec => spec.IsSatisfiedBy(operations, operationType))
            .Returns(specResult);

        // Act
        var result = _operationFactory.Operation(serviceId, clientId, quantity, pricePerDay, operationDate, operationType, operations);

        // Assert
        result.Should().NotBeNull();
        result.ServiceId.Should().Be(serviceId);
        result.ClientId.Should().Be(clientId);
        result.Quantity.Should().Be(quantity);
        result.PricePerDay.Should().Be(pricePerDay);
        result.OperationDate.Should().Be(operationDate);
        result.OperationType.Should().Be(operationType);
    }
}
