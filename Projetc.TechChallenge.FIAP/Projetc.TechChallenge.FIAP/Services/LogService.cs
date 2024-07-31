using System;
using System.Threading.Tasks;
using Projetc.TechChallenge.FIAP.Interfaces;
using Projetc.TechChallenge.FIAP.Data;
using Projetc.TechChallenge.FIAP.Models;

namespace Projetc.TechChallenge.FIAP.Services
{
    public class LogService : ILogService
    {
        private readonly AppDbContext _context;

        public LogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(int? contactId, string action, string details)
        {
            var log = new Log
            {
                ContactId = contactId,
                Action = action,
                Timestamp = DateTime.UtcNow,
                Details = details
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
