using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Projetc.TechChallenge.FIAP;
using Microsoft.AspNetCore.Hosting;
using Projetc.TechChallenge.FIAP.IntegrationTest;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.TestHost;

public class IntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public IntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetAllContacts_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/api/contacts");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("contacts", responseString);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task CreateContact_ReturnsSuccessStatusCode()
    {
        // Arrange
        var content = new StringContent("{\"name\":\"John Doe\",\"phone\":\"123456789\",\"email\":\"john.doe@example.com\",\"ddd\":\"11\"}", Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/contacts", content);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("John Doe", responseString);
    }

   
}
