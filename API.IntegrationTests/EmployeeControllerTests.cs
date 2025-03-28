using Application.DTOs;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.IntegrationTests;

public class EmployeeControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public EmployeeControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<HCMDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<HCMDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });
            });
        }).CreateClient();
    }

    [Fact]
    public async Task CreateEmployee_ReturnsCreated()
    {
        var request = new CreateEmployeeRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Role = "Developer",
            Department = "IT"
        };

        var response = await _client.PostAsJsonAsync("/api/employee", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<CreateEmployeeResponseDto>();
        result.Should().NotBeNull();
        result!.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetEmployees_ReturnsOk()
    {
        var request = new GetEmployeesRequestDto { PageNumber = 1, PageSize = 10 };
        var response = await _client.GetAsync("/api/employee?pageNumber=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var data = await response.Content.ReadFromJsonAsync<GetEmployeesResponseDto>();
        data.Should().NotBeNull();
    }

    [Fact]
    public async Task GetEmployeeById_ReturnsOkOrNotFound()
    {
        var create = new CreateEmployeeRequestDto
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test.user@example.com",
            Role = "Tester",
            Department = "QA"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/employee", create);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateEmployeeResponseDto>();

        var getResponse = await _client.GetAsync($"/api/employee/{created!.Id}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var employee = await getResponse.Content.ReadFromJsonAsync<GetEmployeeResponseDto>();
        employee.Should().NotBeNull();
        employee!.Id.Should().Be(created.Id);
    }

    [Fact]
    public async Task UpdateEmployee_ReturnsOk()
    {
        var create = new CreateEmployeeRequestDto
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@example.com",
            Role = "QA",
            Department = "Testing"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/employee", create);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateEmployeeResponseDto>();

        var update = new UpdateEmployeeRequestDto
        {
            Id = created!.Id,
            FirstName = "Jane Updated",
            LastName = "Doe",
            Email = "jane.updated@example.com",
            Role = "Lead QA",
            Department = "QA"
        };

        var updateResponse = await _client.PutAsJsonAsync("/api/employee", update);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await updateResponse.Content.ReadFromJsonAsync<GetEmployeeResponseDto>();
        result!.FirstName.Should().Be("Jane Updated");
    }

    [Fact]
    public async Task DeleteEmployee_ReturnsNoContent()
    {
        var create = new CreateEmployeeRequestDto
        {
            FirstName = "Delete",
            LastName = "Me",
            Email = "deleteme@example.com",
            Role = "Temp",
            Department = "Temp"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/employee", create);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateEmployeeResponseDto>();

        var deleteResponse = await _client.DeleteAsync($"/api/employee/{created!.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
