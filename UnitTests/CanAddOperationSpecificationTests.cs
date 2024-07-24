using Domain.Operations;
using Domain.Operations.Specifications;
using FluentAssertions;

namespace UnitTests;

public class CanAddOperationSpecificationTests
{
    private IAddOperationSpecification startSpec;
    private IAddOperationSpecification pauseSpec;
    private IAddOperationSpecification resumeSpec;
    private IAddOperationSpecification endSpec;
    private CanAddOperationSpecification _specification;

    [SetUp]
    public void SetUp()
    {
        startSpec = new CanStartServiceSpecification();
        pauseSpec = new CanPauseServiceSpecification();
        resumeSpec = new CanResumeServiceSpecification();
        endSpec = new CanEndServiceSpecification();

        var specifications = new List<IAddOperationSpecification>
        {
            startSpec,
            pauseSpec,
            resumeSpec,
            endSpec
        };

        _specification = new CanAddOperationSpecification(specifications);
    }

    [Test]
    public void IsSatisfiedBy_ShouldReturnTrue_WhenAllSpecificationsAreSatisfied()
    {
        // Arrange
        var operations = new List<Operation>();
        var newOperationType = OperationType.StartService;

        // Act
        var result = _specification.IsSatisfiedBy(operations, newOperationType);

        // Assert
        result.IsSatisfied.Should().BeTrue();
        result.ErrorMessage.Should().BeEmpty();
    }

    [Test]
    public void IsSatisfiedBy_ShouldReturnFalse_WhenStartServiceNotEnding()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var operations = new List<Operation>
        {
            new Operation(Guid.NewGuid(), clientId, 1, 10, new DateTime(2023, 1, 1), OperationType.StartService),
            new Operation(Guid.NewGuid(), clientId, 1, 10, new DateTime(2023, 1, 2), OperationType.PauseService)
        };
        var newOperationType = OperationType.StartService;

        // Act
        var result = _specification.IsSatisfiedBy(operations, newOperationType);

        // Assert
        result.IsSatisfied.Should().BeFalse();
        result.ErrorMessage.Should().Be("Cannot start service when last operation is not ending the service.");
    }

    [Test]
    public void IsSatisfiedBy_ShouldReturnFalse_WhenPauseServiceWithoutStartOrResume()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var operations = new List<Operation>
        {
            new Operation(Guid.NewGuid(), clientId, 1, 10, new DateTime(2023, 1, 1), OperationType.PauseService)
        };
        var newOperationType = OperationType.PauseService;

        // Act
        var result = _specification.IsSatisfiedBy(operations, newOperationType);

        // Assert
        result.IsSatisfied.Should().BeFalse();
        result.ErrorMessage.Should().Be("Cannot pause service unless last operation is start or resume.");
    }

    [Test]
    public void IsSatisfiedBy_ShouldReturnFalse_WhenResumeServiceWithoutPause()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var operations = new List<Operation>
        {
            new Operation(Guid.NewGuid(), clientId, 1, 10, new DateTime(2023, 1, 1), OperationType.StartService)
        };
        var newOperationType = OperationType.ResumeService;

        // Act
        var result = _specification.IsSatisfiedBy(operations, newOperationType);

        // Assert
        result.IsSatisfied.Should().BeFalse();
        result.ErrorMessage.Should().Be("Cannot resume service unless last operation is pause.");
    }

    [Test]
    public void IsSatisfiedBy_ShouldReturnFalse_WhenEndServiceWithoutStartOrResume()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var operations = new List<Operation>
        {
            new Operation(Guid.NewGuid(), clientId, 1, 10, new DateTime(2023, 1, 1), OperationType.PauseService)
        };
        var newOperationType = OperationType.EndService;

        // Act
        var result = _specification.IsSatisfiedBy(operations, newOperationType);

        // Assert
        result.IsSatisfied.Should().BeFalse();
        result.ErrorMessage.Should().Be("Cannot end service unless last operation is start or resume.");
    }

    [Test]
    public void IsSatisfiedBy_ShouldReturnTrue_ForValidEndService()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var operations = new List<Operation>
        {
            new Operation(Guid.NewGuid(), clientId, 1, 10, new DateTime(2023, 1, 1), OperationType.StartService),
            new Operation(Guid.NewGuid(), clientId, 1, 10, new DateTime(2023, 1, 10), OperationType.ResumeService)
        };
        var newOperationType = OperationType.EndService;

        // Act
        var result = _specification.IsSatisfiedBy(operations, newOperationType);

        // Assert
        result.IsSatisfied.Should().BeTrue();
        result.ErrorMessage.Should().BeEmpty();
    }

    [Test]
    public void IsSatisfiedBy_ShouldReturnTrue_ForValidPauseService()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var operations = new List<Operation>
        {
            new Operation(Guid.NewGuid(), clientId, 1, 10, new DateTime(2023, 1, 1), OperationType.StartService)
        };
        var newOperationType = OperationType.PauseService;

        // Act
        var result = _specification.IsSatisfiedBy(operations, newOperationType);

        // Assert
        result.IsSatisfied.Should().BeTrue();
        result.ErrorMessage.Should().BeEmpty();
    }

    [Test]
    public void IsSatisfiedBy_ShouldReturnTrue_ForValidResumeService()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var operations = new List<Operation>
        {
            new Operation(Guid.NewGuid(), clientId, 1, 10, new DateTime(2023, 1, 1), OperationType.StartService),
            new Operation(Guid.NewGuid(), clientId, 1, 10, new DateTime(2023, 1, 5), OperationType.PauseService)
        };
        var newOperationType = OperationType.ResumeService;

        // Act
        var result = _specification.IsSatisfiedBy(operations, newOperationType);

        // Assert
        result.IsSatisfied.Should().BeTrue();
        result.ErrorMessage.Should().BeEmpty();
    }
}
