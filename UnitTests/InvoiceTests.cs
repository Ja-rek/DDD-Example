using FluentAssertions;
using Domain.Invoices;
using Domain.Operations;

namespace UnitTests;

public class InvoiceTests
{
    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenClientIdIsEmpty()
    {
        // Arrange
        var clientId = Guid.Empty;

        // Act
        Action act = () => new Invoice(clientId);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage($"{clientId} must to have a value.");
    }

    [Test]
    public void Constructor_ShouldInitializeWithGivenClientId()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        // Act
        var invoice = new Invoice(clientId);

        // Assert
        invoice.ClientId.Should().Be(clientId);
    }

    [Test]
    public void Constructor_ShouldInitializeWithNewGuidId()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        // Act
        var invoice = new Invoice(clientId);

        // Assert
        invoice.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Constructor_ShouldInitializeWithCurrentUtcDateTime()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var beforeCreation = DateTime.UtcNow;

        // Act
        var invoice = new Invoice(clientId);
        var afterCreation = DateTime.UtcNow;

        // Assert
        invoice.CreationDate.Should().BeOnOrAfter(beforeCreation).And.BeOnOrBefore(afterCreation);
    }

    [Test]
    public void Constructor_ShouldInitializeWithEmptyInvoiceItems()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        // Act
        var invoice = new Invoice(clientId);

        // Assert
        invoice.Items.Should().BeEmpty();
    }

    [Test]
    public void AddInvoiceItemWithContinuousRenditionPeriod_ShouldAddItemToInvoice()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var invoice = new Invoice(clientId);
        var serviceId = Guid.NewGuid();
        var operation = new Operation(serviceId, clientId, 10, 50m, DateTime.UtcNow.AddDays(5), OperationType.StartService);

        // Act
        invoice.AddInvoiceItemWithContinuousRenditionPeriod(operation, DateTime.UtcNow);

        // Assert
        invoice.Items.Should().HaveCount(1);
        var addedItem = invoice.Items[0];
        addedItem.ServiceId.Should().Be(serviceId);
        addedItem.StartDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        addedItem.EndDate.Should().BeCloseTo(DateTime.UtcNow.AddDays(5), TimeSpan.FromSeconds(1));
        addedItem.Value.Should().Be(50m * 10 * 5); // 5 days * 50m * 10 quantity
        addedItem.IsPaused.Should().BeFalse();
    }

    [Test]
    public void AddInvoiceItemWithContinuousRenditionPeriod_ShouldThrowException_WhenOperationIsNull()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var invoice = new Invoice(clientId);

        // Act
        Action act = () => invoice.AddInvoiceItemWithContinuousRenditionPeriod(null, DateTime.UtcNow);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'operation')");
    }

    [Test]
    public void AddInvoiceItemWithContinuousRenditionPeriod_ShouldThrowException_WhenStartIsLaterThanEnd()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var invoice = new Invoice(clientId);
        var serviceId = Guid.NewGuid();
        var operation = new Operation(serviceId, clientId, 10, 50m, DateTime.UtcNow.AddDays(5), OperationType.StartService);

        // Act
        Action act = () => invoice.AddInvoiceItemWithContinuousRenditionPeriod(operation, DateTime.UtcNow.AddDays(10));

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Argument start must be earlier than end.");
    }

    [Test]
    public void AddInvoiceItemWithContinuousRenditionPeriod_ShouldHandlePausedServiceCorrectly()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var invoice = new Invoice(clientId);
        var serviceId = Guid.NewGuid();
        var operation = new Operation(serviceId, clientId, 10, 50m, DateTime.UtcNow.AddDays(5), OperationType.PauseService);

        // Act
        invoice.AddInvoiceItemWithContinuousRenditionPeriod(operation, DateTime.UtcNow);

        // Assert
        var addedItem = invoice.Items[0];
        addedItem.IsPaused.Should().BeTrue();
    }
}

