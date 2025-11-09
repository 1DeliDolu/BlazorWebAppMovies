using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlazorWebAppMovies.Services.Email;

public class MailtrapEmailSender : IEmailSender
{
    private readonly EmailSettings _settings;
    private readonly ILogger<MailtrapEmailSender> _logger;

    public MailtrapEmailSender(IOptions<EmailSettings> settings, ILogger<MailtrapEmailSender> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        if (string.IsNullOrWhiteSpace(_settings.SmtpServer) || string.IsNullOrWhiteSpace(_settings.UserName) || string.IsNullOrWhiteSpace(_settings.Password))
        {
            _logger.LogWarning("Email settings are incomplete. Skipping email to {Email}", email);
            return;
        }

        using var message = new MailMessage
        {
            From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };
        message.To.Add(email);

        using var client = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort)
        {
            EnableSsl = _settings.UseSsl,
            Credentials = new NetworkCredential(_settings.UserName, _settings.Password)
        };

        try
        {
            await client.SendMailAsync(message);
            _logger.LogInformation("Mailtrap email sent to {Email}", email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}", email);
            throw;
        }
    }
}
