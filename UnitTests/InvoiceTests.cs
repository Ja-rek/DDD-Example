using FluentAssertions;
using Domain.Invoices;

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
    public void AddItem_ShouldThrowArgumentNullException_WhenItemIsNull()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var invoice = new Invoice(clientId);

        // Act
        Action act = () => invoice.AddItem(null);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'item')");
    }

    [Test]
    public void AddItem_ShouldAddInvoiceItemToItemsList()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var invoice = new Invoice(clientId);
        var serviceId = Guid.NewGuid();
        var startDate = new DateTime(2023, 7, 18);
        var endDate = new DateTime(2023, 7, 19);
        var value = 100m;
        var isPaused = false;
        var item = new InvoiceItem(serviceId, startDate, endDate, value, isPaused);

        // Act
        invoice.AddItem(item);

        // Assert
        invoice.Items.Should().ContainSingle(i => i == item);
    }
}

