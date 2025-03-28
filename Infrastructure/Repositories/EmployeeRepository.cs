using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly HCMDbContext _context;

    public EmployeeRepository(HCMDbContext context)
    {
        _context = context;
    }

    public async Task<Guid?> CreateEmlpoyee(Employee createEmployeeRequest)
    {
        _context.Employees.Add(createEmployeeRequest);
        await _context.SaveChangesAsync();
        return createEmployeeRequest.Id;
    }

    public async Task DeleteEmployee(Guid id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee is null) return;

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<Employee>> GetEmployees(int pageNumber, int pageSize)
    {
        var query = _context.Employees.AsQueryable();

        var totalRecords = await query.CountAsync();

        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Employee>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            Data = data
        };
    }

    public async Task<Employee?> GetEmployee(Guid id)
    {
        return await _context.Employees.FindAsync(id);
    }

    public async Task<Employee?> UpdateEmlpoyee(Employee updateEmployeeRequest)
    {
        var employee = await _context.Employees.FindAsync(updateEmployeeRequest.Id);
        if (employee is null) return null;

        employee.FirstName = updateEmployeeRequest.FirstName;
        employee.LastName = updateEmployeeRequest.LastName;
        employee.Email = updateEmployeeRequest.Email;
        employee.Role = updateEmployeeRequest.Role;
        employee.Department = updateEmployeeRequest.Department;

        await _context.SaveChangesAsync();
        return employee;
    }
}
