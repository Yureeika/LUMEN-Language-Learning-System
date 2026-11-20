namespace LUMEN.Services;

using Microsoft.Extensions.Logging;

public class EmailService(ILogger<EmailService> logger) : IEmailService
{
    public Task SendEmailAsync(string toEmail, string subject, string htmlMessage, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Simulating email send to {Email} with subject: {Subject}", toEmail, subject);
        return Task.CompletedTask;
    }
}