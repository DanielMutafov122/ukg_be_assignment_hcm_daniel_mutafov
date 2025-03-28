using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeRequestDto request)
    {
        var result = await _employeeService.CreateEmployee(request);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployees([FromQuery] GetEmployeesRequestDto request)
    {
        var result = await _employeeService.GetEmployees(request);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var employee = await _employeeService.GetEmployee(id);
        if (employee == null)
            return NotFound();

        return Ok(employee);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateEmployeeRequestDto request)
    {
        var updated = await _employeeService.UpdateEmployee(request);
        if (updated == null)
            return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _employeeService.DeleteEmployee(id);
        return NoContent();
    }
}
