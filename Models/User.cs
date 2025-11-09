using System.ComponentModel.DataAnnotations;

namespace BlazorSignalRApp.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string? Name { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? Role { get; set; }

    [StringLength(450)]
    public string? IdentityUserId { get; set; }
}
