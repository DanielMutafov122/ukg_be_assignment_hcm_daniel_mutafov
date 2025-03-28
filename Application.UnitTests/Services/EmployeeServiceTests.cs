using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Common;
using Domain.Entities;
using Moq;
using Xunit;

namespace Application.UnitTests.Services;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
    private readonly EmployeeService _service;

    public EmployeeServiceTests()
    {
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _service = new EmployeeService(_employeeRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateEmployee_ReturnsResponseWithId()
    {
        var request = new CreateEmployeeRequestDto { FirstName = "Jane" };
        var generatedId = Guid.NewGuid();

        _employeeRepositoryMock.Setup(r => r.CreateEmlpoyee(It.IsAny<Employee>()))
            .ReturnsAsync(generatedId);

        var result = await _service.CreateEmployee(request);

        Assert.NotNull(result);
        Assert.Equal(generatedId, result.Id);
    }

    [Fact]
    public async Task GetEmployee_ReturnsMappedDto_WhenEmployeeExists()
    {
        var id = Guid.NewGuid();
        var employee = new Employee { Id = id, FirstName = "Test" };
        _employeeRepositoryMock.Setup(r => r.GetEmployee(id)).ReturnsAsync(employee);

        var result = await _service.GetEmployee(id);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetEmployee_ReturnsNull_WhenNotFound()
    {
        _employeeRepositoryMock.Setup(r => r.GetEmployee(It.IsAny<Guid>())).ReturnsAsync((Employee?)null);

        var result = await _service.GetEmployee(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetEmployees_ReturnsMappedPagedResult()
    {
        var employees = new List<Employee> { new Employee { Id = Guid.NewGuid(), FirstName = "E1" } };

        var pagedResult = new PagedResult<Employee>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalRecords = 1,
            Data = employees
        };

        _employeeRepositoryMock.Setup(r => r.GetEmployees(1, 10)).ReturnsAsync(pagedResult);

        var result = await _service.GetEmployees(new GetEmployeesRequestDto { PageNumber = 1, PageSize = 10 });

        Assert.NotNull(result);
        Assert.Single(result.Employees);
    }

    [Fact]
    public async Task UpdateEmployee_ReturnsMappedDto_WhenSuccessful()
    {
        var request = new UpdateEmployeeRequestDto { Id = Guid.NewGuid(), FirstName = "Updated" };
        var updatedEntity = new Employee { Id = request.Id, FirstName = "Updated" };

        _employeeRepositoryMock.Setup(r => r.UpdateEmlpoyee(It.IsAny<Employee>())).ReturnsAsync(updatedEntity);

        var result = await _service.UpdateEmployee(request);

        Assert.NotNull(result);
        Assert.Equal("Updated", result.FirstName);
    }

    [Fact]
    public async Task UpdateEmployee_ReturnsNull_WhenUpdateFails()
    {
        _employeeRepositoryMock.Setup(r => r.UpdateEmlpoyee(It.IsAny<Employee>())).ReturnsAsync((Employee?)null);

        var result = await _service.UpdateEmployee(new UpdateEmployeeRequestDto());

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteEmployee_CallsRepository()
    {
        var id = Guid.NewGuid();

        await _service.DeleteEmployee(id);

        _employeeRepositoryMock.Verify(r => r.DeleteEmployee(id), Times.Once);
    }
}
