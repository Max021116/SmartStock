using Microsoft.AspNetCore.Identity;

namespace SmartStock.Server;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
}