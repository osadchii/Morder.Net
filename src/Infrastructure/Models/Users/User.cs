
using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models.Users;

public class ApplicationUser : IdentityUser
{
    [Required] 
    [MaxLength(Limits.UserNameLength)]
    public string Name { get; set; } = null!;

    public bool Active { get; set; }
}

public class UserDto
{
    [Required]
    [MaxLength(Limits.UserNameLength)]
    public string Name { get; set; } = null!;
    
    [Required]
    [MaxLength(Limits.UserPasswordLength)]
    public string Password { get; set; } = null!;
}