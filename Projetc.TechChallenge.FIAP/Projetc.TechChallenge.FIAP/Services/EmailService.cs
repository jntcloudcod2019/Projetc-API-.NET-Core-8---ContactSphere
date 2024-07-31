using System.Net.Mail;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Projetc.TechChallenge.FIAP.Interfaces;
using Projetc.TechChallenge.FIAP.Models;

namespace Projetc.TechChallenge.FIAP.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private const int DefaultTimeoutMilliseconds = 10000;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(EmailMessageType messageType, Contact contact)
        {
            try
            {
                var smtpServer = _configuration["EMAIL_SMTP_SERVER"];
                var smtpPort = int.Parse(_configuration["EMAIL_SMTP_PORT"]);
                var senderEmail = _configuration["EMAIL_SENDER"];
                var senderPassword = _configuration["EMAIL_PASSWORD"];
                var recipientEmail = _configuration["EMAIL_RECIPIENT"];

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = messageType.GetSubject(),
                    Body = messageType.GetMessage(contact),
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(recipientEmail);

                using (var smtpClient = new SmtpClient(smtpServer))
                {
                    smtpClient.Port = smtpPort;
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtpClient.EnableSsl = true;

                    using (var cts = new CancellationTokenSource(DefaultTimeoutMilliseconds))
                    {
                        var sendTask = smtpClient.SendMailAsync(mailMessage);

                        await Task.WhenAny(sendTask, Task.Delay(Timeout.Infinite, cts.Token));

                        if (sendTask.IsCompletedSuccessfully)
                        {
                            await sendTask;
                            _logger.LogInformation("Email sent successfully.");
                        }
                        else
                        {
                            throw new TimeoutException("O envio do e-mail ultrapassou o tempo limite.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email.");
                throw;
            }
        }
    }
}
