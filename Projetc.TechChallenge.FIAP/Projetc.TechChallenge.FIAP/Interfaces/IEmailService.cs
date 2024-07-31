using Projetc.TechChallenge.FIAP.Models;

namespace Projetc.TechChallenge.FIAP.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessageType messageType, Contact contact);

    }
}
