using Application.DTOs;
using Application.Interfaces;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using API.Controllers;


namespace API.Tests.Controllers;

public class EmployeeControllerTests
{
    private readonly Mock<IEmployeeService> _employeeServiceMock;
    private readonly EmployeeController _controller;

    public EmployeeControllerTests()
    {
        _employeeServiceMock = new Mock<IEmployeeService>();
        _controller = new EmployeeController(_employeeServiceMock.Object);
    }

    [Fact]
    public async Task Create_ReturnsOk_WhenSuccessful()
    {
        var request = new CreateEmployeeRequestDto { FirstName = "Test" };
        var response = new CreateEmployeeResponseDto { Id = Guid.NewGuid() };

        _employeeServiceMock.Setup(s => s.CreateEmployee(request)).ReturnsAsync(response);

        var result = await _controller.Create(request);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task GetEmployees_ReturnsOk_WithData()
    {
        var request = new GetEmployeesRequestDto { PageNumber = 1, PageSize = 10 };
        var response = new GetEmployeesResponseDto
        {
            PageNumber = 1,
            PageSize = 10,
            TotalRecords = 1,
            Employees = new List<GetEmployeeResponseDto> { new GetEmployeeResponseDto { FirstName = "Jane" } }
        };

        _employeeServiceMock.Setup(s => s.GetEmployees(request)).ReturnsAsync(response);

        var result = await _controller.GetEmployees(request);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        var id = Guid.NewGuid();
        var dto = new GetEmployeeResponseDto { Id = id, FirstName = "John" };

        _employeeServiceMock.Setup(s => s.GetEmployee(id)).ReturnsAsync(dto);

        var result = await _controller.GetById(id);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenMissing()
    {
        var id = Guid.NewGuid();
        _employeeServiceMock.Setup(s => s.GetEmployee(id)).ReturnsAsync((GetEmployeeResponseDto?)null);

        var result = await _controller.GetById(id);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Update_ReturnsOk_WhenSuccessful()
    {
        var request = new UpdateEmployeeRequestDto { Id = Guid.NewGuid(), FirstName = "Updated" };
        var response = new GetEmployeeResponseDto { Id = request.Id, FirstName = "Updated" };

        _employeeServiceMock.Setup(s => s.UpdateEmployee(request)).ReturnsAsync(response);

        var result = await _controller.Update(request);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenMissing()
    {
        var request = new UpdateEmployeeRequestDto { Id = Guid.NewGuid() };
        _employeeServiceMock.Setup(s => s.UpdateEmployee(request)).ReturnsAsync((GetEmployeeResponseDto?)null);

        var result = await _controller.Update(request);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenSuccessful()
    {
        var id = Guid.NewGuid();
        _employeeServiceMock.Setup(s => s.DeleteEmployee(id)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(id);

        result.Should().BeOfType<NoContentResult>();
    }
}
