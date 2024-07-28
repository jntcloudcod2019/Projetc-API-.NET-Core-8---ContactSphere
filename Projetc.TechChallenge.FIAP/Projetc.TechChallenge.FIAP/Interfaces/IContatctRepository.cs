using Projetc.TechChallenge.FIAP.Models;

namespace Projetc.TechChallenge.FIAP.Interfaces
{
    public interface IContatctRepository
    {
        Task<IEnumerable<Contact>> GetAllContactsAsync();
        Task<Contact> GetContactByIdAsync(int id);
        Task AddContactAsync(Contact contact);
        Task UpdateContactAsync(Contact contact);
        Task DeleteContactAsync(int id);
    }
}
