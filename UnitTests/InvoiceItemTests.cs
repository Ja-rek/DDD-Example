using Domain.Invoices;

namespace UnitTests;

public class InvoiceItemTests
{
    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenServiceIdIsEmpty()
    {
        // Arrange
        var serviceId = Guid.Empty;
        var startDate = new DateTime(2023, 7, 18);
        var endDate = new DateTime(2023, 7, 19);
        var value = 100m;
        var isPaused = false;

        // Act
        Action act = () => new InvoiceItem(serviceId, startDate, endDate, value, isPaused);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("ServiceId must have a value.*");
    }

    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenStartDateIsDefault()
    {
        // Arrange
        var serviceId = Guid.NewGuid();
        var startDate = default(DateTime);
        var endDate = new DateTime(2023, 7, 19);
        var value = 100m;
        var isPaused = false;

        // Act
        Action act = () => new InvoiceItem(serviceId, startDate, endDate, value, isPaused);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("StartDate must have a value.*");
    }

    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenEndDateIsDefault()
    {
        // Arrange
        var serviceId = Guid.NewGuid();
        var startDate = new DateTime(2023, 7, 18);
        var endDate = default(DateTime);
        var value = 100m;
        var isPaused = false;

        // Act
        Action act = () => new InvoiceItem(serviceId, startDate, endDate, value, isPaused);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("EndDate must have a value.*");
    }

    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenStartDateIsGreaterThanOrEqualToEndDate()
    {
        // Arrange
        var serviceId = Guid.NewGuid();
        var startDate = new DateTime(2023, 7, 19);
        var endDate = new DateTime(2023, 7, 18);
        var value = 100m;
        var isPaused = false;

        // Act
        Action act = () => new InvoiceItem(serviceId, startDate, endDate, value, isPaused);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("startDate must be earlier than EndDate.");
    }

    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenValueIsLessThanOrEqualToZero()
    {
        // Arrange
        var serviceId = Guid.NewGuid();
        var startDate = new DateTime(2023, 7, 18);
        var endDate = new DateTime(2023, 7, 19);
        var value = 0m;
        var isPaused = false;

        // Act
        Action act = () => new InvoiceItem(serviceId, startDate, endDate, value, isPaused);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Value must be greater than zero.*");
    }

    [Test]
    public void Constructor_ShouldInitializeWithGivenValues()
    {
        // Arrange
        var serviceId = Guid.NewGuid();
        var startDate = new DateTime(2023, 7, 18);
        var endDate = new DateTime(2023, 7, 19);
        var value = 100m;
        var isPaused = false;

        // Act
        var invoiceItem = new InvoiceItem(serviceId, startDate, endDate, value, isPaused);

        // Assert
        invoiceItem.ServiceId.Should().Be(serviceId);
        invoiceItem.StartDate.Should().Be(startDate);
        invoiceItem.EndDate.Should().Be(endDate);
        invoiceItem.Value.Should().Be(value);
        invoiceItem.IsPaused.Should().Be(isPaused);
    }
}

