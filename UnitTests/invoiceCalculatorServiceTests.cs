using Domain.Invoices;
using Domain.Operations;
using FluentAssertions;
using Moq;

namespace UnitTests;

public class invoiceCalculatorServiceTests
{
    private Mock<IInvoiceRepository> invoiceRepositoryMock;
    private InvoiceCalculatorService invoiceCalculatorService;

    [SetUp]
    public void Setup()
    {
        invoiceRepositoryMock = new Mock<IInvoiceRepository>();
        invoiceCalculatorService = new InvoiceCalculatorService(invoiceRepositoryMock.Object);
    }

    [Test]
    public void Calculate_ShouldThrowException_WhenLastOperationIsNotEndService()
    {
        // Arrange
        var operations = new List<Operation>
        {
            new Operation(Guid.NewGuid(), Guid.NewGuid(), 1, 10, new DateTime(2023, 1, 1), OperationType.StartService)
        };

        // Act
        Action act = () => invoiceCalculatorService.Calculate(Guid.NewGuid(), operations, 1, 2023);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Cannot generate invoice for client unless the last operation is ending the service.");
    }

    [Test]
    public void Calculate_ShouldThrowException_WhenInvoiceAlreadyExists()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var operations = new List<Operation>
        {
            new Operation(Guid.NewGuid(), clientId, 1, 10, new DateTime(2023, 1, 1), OperationType.StartService),
            new Operation(Guid.NewGuid(), clientId, 1, 10, new DateTime(2023, 1, 2), OperationType.EndService)
        };

        invoiceRepositoryMock.Setup(x => x.GetInvoice(clientId, 1, 2023)).Returns(new Invoice(clientId));

        // Act
        Action act = () => invoiceCalculatorService.Calculate(clientId, operations, 1, 2023);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Invoice for this client and period already exists.");
    }

    [Test]
    public void Calculate_ShouldCreateInvoice_WhenValidOperationsProvided()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();
        var operations = new List<Operation>
        {
            new Operation(serviceId, clientId, 1, 10, new DateTime(2023, 1, 1), OperationType.StartService),
            new Operation(serviceId, clientId, 1, 10, new DateTime(2023, 1, 2), OperationType.PauseService),
            new Operation(serviceId, clientId, 1, 10, new DateTime(2023, 1, 3), OperationType.ResumeService),
            new Operation(serviceId, clientId, 1, 10, new DateTime(2023, 1, 20), OperationType.EndService)
        };

        invoiceRepositoryMock.Setup(x => x.GetInvoice(clientId, 1, 2023)).Returns((Invoice)null);

        // Act
        var invoice = invoiceCalculatorService.Calculate(clientId, operations, 1, 2023);

        // Assert
        invoice.Should().NotBeNull();
        invoice.ClientId.Should().Be(clientId);
        invoice.Items.Should().HaveCount(2);

        var firstItem = invoice.Items[0];
        firstItem.ServiceId.Should().Be(serviceId);
        firstItem.StartDate.Should().Be(new DateTime(2023, 1, 1));
        firstItem.EndDate.Should().Be(new DateTime(2023, 1, 2));
        firstItem.Value.Should().Be(20); // 2 day * 10 price * 1 quantity
        firstItem.IsPaused.Should().BeTrue();

        var secondItem = invoice.Items[1];
        secondItem.ServiceId.Should().Be(serviceId);
        secondItem.StartDate.Should().Be(new DateTime(2023, 1, 3));
        secondItem.EndDate.Should().Be(new DateTime(2023, 1, 20));
        secondItem.Value.Should().Be(180); // 18 days * 10 price * 1 quantity
        secondItem.IsPaused.Should().BeFalse();
    }
}
