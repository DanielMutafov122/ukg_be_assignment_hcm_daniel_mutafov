using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<CreateEmployeeResponseDto> CreateEmployee(CreateEmployeeRequestDto createEmployeeRequest)
    {
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = createEmployeeRequest.FirstName,
            LastName = createEmployeeRequest.LastName,
            Email = createEmployeeRequest.Email,
            Role = createEmployeeRequest.Role,
            Department = createEmployeeRequest.Department
        };

        var newId = await _employeeRepository.CreateEmlpoyee(employee);

        return new CreateEmployeeResponseDto
        {
            Id = newId.Value
        };
    }

    public async Task DeleteEmployee(Guid id)
    {
        await _employeeRepository.DeleteEmployee(id);
    }

    public async Task<GetEmployeesResponseDto?> GetEmployees(GetEmployeesRequestDto getEmployeesRequest)
    {
        var result = await _employeeRepository.GetEmployees(
            getEmployeesRequest.PageNumber,
            getEmployeesRequest.PageSize
        );

        var response = new GetEmployeesResponseDto
        {
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords,
            Employees = result.Data.Select(e => new GetEmployeeResponseDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Role = e.Role,
                Department = e.Department,
                CreatedOn = e.CreatedAt,
                LastUpdatedOn = e.LastUpdatedAt
            }).ToList()
        };

        return response;
    }

    public async Task<GetEmployeeResponseDto?> GetEmployee(Guid id)
    {
        var employee = await _employeeRepository.GetEmployee(id);

        if (employee is null)
            return null;

        return new GetEmployeeResponseDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Role = employee.Role,
            Department = employee.Department
        };
    }

    public async Task<GetEmployeeResponseDto?> UpdateEmployee(UpdateEmployeeRequestDto updateEmployeeRequest)
    {
        var employee = new Employee
        {
            Id = updateEmployeeRequest.Id,
            FirstName = updateEmployeeRequest.FirstName,
            LastName = updateEmployeeRequest.LastName,
            Email = updateEmployeeRequest.Email,
            Role = updateEmployeeRequest.Role,
            Department = updateEmployeeRequest.Department
        };

        var updated = await _employeeRepository.UpdateEmlpoyee(employee);
        if (updated is null)
            return null;

        return new GetEmployeeResponseDto
        {
            Id = updated.Id,
            FirstName = updated.FirstName,
            LastName = updated.LastName,
            Email = updated.Email,
            Role = updated.Role,
            Department = updated.Department
        };
    }
}
