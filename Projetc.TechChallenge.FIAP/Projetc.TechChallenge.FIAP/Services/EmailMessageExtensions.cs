using Projetc.TechChallenge.FIAP.Models;

namespace Projetc.TechChallenge.FIAP.Services
{
    public static class EmailMessageExtensions
    {
        public static string GetMessage(this EmailMessageType messageType, Contact contact)
        {
            return messageType switch
            {
                EmailMessageType.ContactCreated => $"A new contact named {contact.Name} was created.",
                EmailMessageType.ContactUpdated => $"The contact named {contact.Name} was updated.",
                EmailMessageType.ContactDeleted => $"The contact named {contact.Name} was deleted.",
                _ => throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null)
            };
        }

        public static string GetSubject(this EmailMessageType messageType)
        {
            return messageType switch
            {
                EmailMessageType.ContactCreated => "New Contact Created",
                EmailMessageType.ContactUpdated => "Contact Updated",
                EmailMessageType.ContactDeleted => "Contact Deleted",
                _ => throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null)
            };
        }
    }
}
