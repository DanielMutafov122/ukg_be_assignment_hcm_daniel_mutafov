namespace Application.DTOs;

public class GetEmployeeResponseDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Department { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastUpdatedOn { get; set; }
}
