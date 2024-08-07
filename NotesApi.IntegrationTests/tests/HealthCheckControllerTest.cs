using NotesApi.IntegrationTests;

namespace NotesApi.IntegrationTests.Tests;

[Collection("Integration Tests")]
public class HealthCheckControllerTest
{
    private readonly NotesApiFactory _factory;

    public HealthCheckControllerTest(NotesApiTestFixture fixture)
    {
        _factory = fixture.Factory;
    }

    [Fact]
    public async Task HealthCheck_ReturnsHealthy()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/HealthCheck");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Equal("Healthy", responseString);
    }
}
