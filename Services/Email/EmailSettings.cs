namespace BlazorWebAppMovies.Services.Email;

public class EmailSettings
{
    public string SmtpServer { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 2525;
    public string SenderName { get; set; } = "Blazor App";
    public string SenderEmail { get; set; } = "noreply@example.com";
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool UseSsl { get; set; } = true;
}
