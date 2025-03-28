namespace Application.DTOs;

public class GetEmployeesRequestDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 100;
}
