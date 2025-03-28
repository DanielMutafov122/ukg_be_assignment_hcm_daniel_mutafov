using Microsoft.AspNetCore.Identity;

namespace Domain.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
    public string Department { get; set; }
}
