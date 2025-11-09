using System.ComponentModel.DataAnnotations;

namespace BlazorSignalRApp.Models;

public class ChatMessage
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Message { get; set; } = string.Empty;

    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
