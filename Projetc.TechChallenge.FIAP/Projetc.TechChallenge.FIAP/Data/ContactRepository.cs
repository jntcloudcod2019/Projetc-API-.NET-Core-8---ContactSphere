using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Dapper;
using Projetc.TechChallenge.FIAP.Interfaces;
using Projetc.TechChallenge.FIAP.Models;

namespace Projetc.TechChallenge.FIAP.Data
{
    public class ContactRepository : IContatctRepository
    {
        private readonly string _connectionString;

        public ContactRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var contacts = await connection.QueryAsync<Contact>("SELECT * FROM Contacts");
                return contacts;
            }
        }

        public async Task<Contact> GetContactByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var contact = await connection.QueryFirstOrDefaultAsync<Contact>("SELECT * FROM Contacts WHERE Id = @Id", new { Id = id });
                return contact;
            }
        }

        public async Task AddContactAsync(Contact contact)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "INSERT INTO Contacts (Name, Phone, Email, Ddd) VALUES (@Name, @Phone, @Email, @Ddd)";
                await connection.ExecuteAsync(query, new { contact.Name, contact.Phone, contact.Email, contact.DDD });
            }
        }

        public async Task UpdateContactAsync(Contact contact)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "UPDATE Contacts SET Name = @Name, Phone = @Phone, Email = @Email, Ddd = @Ddd WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { contact.Name, contact.Phone, contact.Email, contact.DDD, contact.Id });
            }
        }

        public async Task DeleteContactAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "DELETE FROM Contacts WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
