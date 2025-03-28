namespace Application.DTOs;

public class GetEmployeesResponseDto
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
    public List<GetEmployeeResponseDto> Employees { get; set; } = new();
}
