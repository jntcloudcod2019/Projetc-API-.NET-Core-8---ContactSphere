using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Projetc.TechChallenge.FIAP.Data;
using System;
using System.Linq;

namespace Projetc.TechChallenge.FIAP.IntegrationTest
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // remove a configuração do contexto existente 
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Adiciona uma nova configuração do banco em memoria 
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // Build do serviço 
                var sp = services.BuildServiceProvider();

               
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<AppDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                   
                    db.Database.EnsureCreated();

                    try
                    {
                        // Seed the database with test data if necessary
                        // SeedDatabase(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"An error occurred seeding the database. Error: {ex.Message}");
                    }
                }
            });
        }

    }
}
