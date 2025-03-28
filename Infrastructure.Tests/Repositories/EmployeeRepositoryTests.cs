using Domain.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistance;
using Xunit;

namespace Infrastructure.Tests.Repositories;

public class EmployeeRepositoryTests
{
    private readonly HCMDbContext _context;
    private readonly EmployeeRepository _repository;

    public EmployeeRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<HCMDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new HCMDbContext(options);
        _repository = new EmployeeRepository(_context);
    }

    [Fact]
    public async Task CreateEmployee_AddsNewEmployee()
    {
        var employee = new Employee
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Role = "Developer",
            Department = "IT"
        };

        var id = await _repository.CreateEmlpoyee(employee);

        var created = await _context.Employees.FindAsync(id);
        Assert.NotNull(created);
        Assert.Equal("John", created.FirstName);
    }

    [Fact]
    public async Task GetEmployee_ReturnsEmployee_WhenExists()
    {
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@example.com",
            Role = "Developer",
            Department = "IT"
        };
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        var result = await _repository.GetEmployee(employee.Id);
        Assert.NotNull(result);
        Assert.Equal("Jane", result.FirstName);
    }

    [Fact]
    public async Task GetEmployee_ReturnsNull_WhenNotFound()
    {
        var result = await _repository.GetEmployee(Guid.NewGuid());
        Assert.Null(result);
    }

    [Fact]
    public async Task GetEmployees_ReturnsEmployees()
    {
        _context.Employees.AddRange(new List<Employee>
        {
            new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "JohnA",
                LastName = "Doe",
                Email = "john@example.com",
                Role = "Developer",
                Department = "IT"
            },
             new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "JohnB",
                LastName = "Doe",
                Email = "john@example.com",
                Role = "Developer",
                Department = "IT"
            }
        });
        await _context.SaveChangesAsync();

        var result = await _repository.GetEmployees(1, 100);
        Assert.Equal(2, result.Data.Count);
    }

    [Fact]
    public async Task UpdateEmployee_ChangesValues()
    {
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = "JohnA",
            LastName = "Doe",
            Email = "john@example.com",
            Role = "Developer",
            Department = "IT"
        };
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        employee.FirstName = "New";
        var updated = await _repository.UpdateEmlpoyee(employee);

        Assert.Equal("New", updated.FirstName);
    }

    [Fact]
    public async Task DeleteEmployee_RemovesEmployee()
    {
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = "ToDelete",
            LastName = "Doe",
            Email = "john@example.com",
            Role = "Developer",
            Department = "IT"
        };
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        await _repository.DeleteEmployee(employee.Id);

        var exists = await _context.Employees.FindAsync(employee.Id);
        Assert.Null(exists);
    }

    [Fact]
    public async Task GetAllEmployeesAsync_ReturnsPagedResult()
    {
        for (int i = 0; i < 20; i++)
        {
            _context.Employees.Add(new Employee
            {
                FirstName = $"test{1}",
                LastName = "Doe",
                Email = "john@example.com",
                Role = "Developer",
                Department = "IT"
            });
        }
        await _context.SaveChangesAsync();

        var paged = await _repository.GetEmployees(2, 5);

        Assert.Equal(5, paged.Data.Count);
        Assert.Equal(2, paged.PageNumber);
    }
}

