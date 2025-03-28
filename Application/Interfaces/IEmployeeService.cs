using Application.DTOs;

namespace Application.Interfaces;

public interface IEmployeeService
{
    public Task<CreateEmployeeResponseDto> CreateEmployee(CreateEmployeeRequestDto createEmployeeRequest);
    public Task<GetEmployeeResponseDto?> GetEmployee(Guid id);
    public Task<GetEmployeeResponseDto?> UpdateEmployee(UpdateEmployeeRequestDto updateEmployeeRequest);
    public Task<GetEmployeesResponseDto?> GetEmployees(GetEmployeesRequestDto getEmployeesRequest);
    public Task DeleteEmployee(Guid id);
}
