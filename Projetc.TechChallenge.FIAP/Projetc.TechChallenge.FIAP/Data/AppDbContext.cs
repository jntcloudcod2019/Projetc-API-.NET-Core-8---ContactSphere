using Microsoft.EntityFrameworkCore;
using Projetc.TechChallenge.FIAP.Models;
using System.Collections.Generic;

namespace Projetc.TechChallenge.FIAP.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}

