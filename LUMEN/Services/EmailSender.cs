namespace LUMEN.Services;

using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

public class EmailSettings
{
    public string SmtpServer { get; set; } = "";
    public int Port { get; set; }
    public string SenderEmail { get; set; } = "";
    public string SenderPassword { get; set; } = "";
    public string SenderName { get; set; } = "";
}

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _settings;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IOptions<EmailSettings> settings, ILogger<EmailSender> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        try
        {
            _logger.LogInformation("Attempting to send email to {Email} via {SmtpServer}", email, _settings.SmtpServer);

            using var client = new SmtpClient(_settings.SmtpServer)
            {
                Port = _settings.Port,
                Credentials = new NetworkCredential(_settings.SenderEmail, _settings.SenderPassword),
                EnableSsl = true,
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
                Priority = MailPriority.Normal
            };

            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
            _logger.LogInformation("Email successfully sent to {Email}", email);
        }
        catch (SmtpException smtpEx)
        {
            _logger.LogError(smtpEx, "SMTP error sending email to {Email}: {Message}", email, smtpEx.Message);
            throw new Exception($"Email configuration error: {smtpEx.Message}. Please check your SMTP settings.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}", email);
            throw;
        }
    }
}

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);
}