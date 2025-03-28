using Domain.Common;
using Domain.Entities;

namespace Application.Interfaces;

public interface IEmployeeRepository
{
    public Task<Guid?> CreateEmlpoyee(Employee createEmployeeRequest);
    public Task<Employee?> UpdateEmlpoyee(Employee updateEmployeeRequest);
    public Task<Employee?> GetEmployee(Guid id);
    public Task<PagedResult<Employee>> GetEmployees(int pageNumber, int pageSize);
    public Task DeleteEmployee(Guid id);
}
