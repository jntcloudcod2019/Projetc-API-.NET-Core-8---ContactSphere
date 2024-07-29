using Microsoft.EntityFrameworkCore;
using Projetc.TechChallenge.FIAP.Data;
using Projetc.TechChallenge.FIAP.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Projetc.TechChallenge.FIAP.Tests
{
    public class ContactRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;

        public ContactRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task AddContactAsync_ShouldAddContact()
        {
            
            using var context = new AppDbContext(_dbContextOptions);
            var repository = new ContactRepository(context);
            var contact = new Contact { Name = "John Doe", Phone = "1234567890", Email = "john@example.com", DDD = "011" };

         
            await repository.AddContactAsync(contact);

            
            var contacts = await context.Contacts.ToListAsync();

            // Assert
            Assert.Single(contacts); 
            Assert.Equal("John Doe", contacts.First().Name); 
        }

        [Fact]
        public async Task GetAllContactsAsync_ShouldReturnAllContacts()
        {
            // Arrange
            using var context = new AppDbContext(_dbContextOptions);
            context.Contacts.Add(new Contact { Name = "John Doe", Phone = "1234567890", Email = "john@example.com", DDD = "011" });
            context.Contacts.Add(new Contact { Name = "Jane Doe", Phone = "0987654321", Email = "jane@example.com", DDD = "012" });
            await context.SaveChangesAsync();
            var repository = new ContactRepository(context);

            // Act
            var contacts = await repository.GetAllContactsAsync();

            // Assert
            Assert.Equal(2, contacts.Count());
        }

        [Fact]
        public async Task GetContactByIdAsync_ShouldReturnCorrectContact()
        {
            // Arrange
            using var context = new AppDbContext(_dbContextOptions);
            var contact = new Contact { Name = "John Doe", Phone = "1234567890", Email = "john@example.com", DDD = "011" };
            context.Contacts.Add(contact);
            await context.SaveChangesAsync();
            var repository = new ContactRepository(context);

            // Act
            var result = await repository.GetContactByIdAsync(contact.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
        }

        [Fact]
        public async Task UpdateContactAsync_ShouldUpdateContact()
        {
            // Arrange
            using var context = new AppDbContext(_dbContextOptions);
            var contact = new Contact { Name = "John Doe", Phone = "1234567890", Email = "john@example.com", DDD = "011" };
            context.Contacts.Add(contact);
            await context.SaveChangesAsync();
            var repository = new ContactRepository(context);

            // Act
            contact.Name = "John Updated";
            await repository.UpdateContactAsync(contact);
            var updatedContact = await context.Contacts.FindAsync(contact.Id);

            // Assert
            Assert.Equal("John Updated", updatedContact.Name);
        }

        [Fact]
        public async Task DeleteContactAsync_ShouldDeleteContact()
        {
            // Arrange
            using var context = new AppDbContext(_dbContextOptions);
            var contact = new Contact { Name = "John Doe", Phone = "1234567890", Email = "john@example.com", DDD = "011" };
            context.Contacts.Add(contact);
            await context.SaveChangesAsync();
            var repository = new ContactRepository(context);

            // Act
            await repository.DeleteContactAsync(contact.Id);
            var deletedContact = await context.Contacts.FindAsync(contact.Id);

            // Assert
            Assert.Null(deletedContact);
        }
    }
}
