using Application.Invoices;
using Application.Invoices.Commands;
using Domain.Invoices;
using Domain.Operations;
using Moq;

namespace UnitTests;

public class InvoiceCommandServiceTests
{
    private Mock<IOperationRepository> operationRepositoryMock;
    private Mock<IInvoiceRepository> invoiceRepositoryMock;
    private InvoiceCommandService service;

    [SetUp]
    public void Setup()
    {
        operationRepositoryMock = new Mock<IOperationRepository>();
        invoiceRepositoryMock = new Mock<IInvoiceRepository>();
        var invoiceGenerator = new InvoiceGeneratorService(invoiceRepositoryMock.Object);
        service = new InvoiceCommandService(operationRepositoryMock.Object,
            invoiceGenerator,
            invoiceRepositoryMock.Object);
    }

    [Test]
    public void CalculateInvoices_ShouldNotGenerateInvoice_WhenNoOperations()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var clients = new List<Guid> { clientId };
        var operations = Enumerable.Empty<Operation>();

        operationRepositoryMock.Setup(x => x.GetAllClientsWithOperations()).Returns(clients);
        operationRepositoryMock.Setup(x => x.GetOperationsForClientInMonth(clientId, 1, 2023)).Returns(operations);

        // Act
        service.CalculateInvoices(new CalculateInvoicesCommand(1, 2023));

        // Assert
        invoiceRepositoryMock.Verify(x => x.Save(It.IsAny<Invoice>()), Times.Never);
    }

    [Test]
    public void CalculateInvoices_ShouldGenerateInvoice_WhenOperationsExist()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var clients = new List<Guid> { clientId };
        Operation[] operations =
        [
            new Operation(Guid.NewGuid(), clientId, 1, 10, new DateTime(2023, 1, 1), OperationType.StartService),
            new Operation(Guid.NewGuid(), clientId, 1, 10, new DateTime(2023, 1, 2), OperationType.EndService)
        ];

        var invoice = new Invoice(clientId);
        operationRepositoryMock.Setup(x => x.GetAllClientsWithOperations()).Returns(clients);
        operationRepositoryMock.Setup(x => x.GetOperationsForClientInMonth(clientId, 1, 2023)).Returns(operations);

        // Act
        service.CalculateInvoices(new CalculateInvoicesCommand(1, 2023));

        // Assert
        invoiceRepositoryMock.Verify(x => x.Save(It.Is<Invoice>(inv => inv.ClientId == clientId)), Times.Once);
    }
}