namespace NotesApi.IntegrationTests;

public class HealthCheckTest
{
    [Fact]
    public async Task HealthCheck_ReturnsHealthy()
    {
        // Arrange
        using var factory = new NotesApiFactory();
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/HealthCheck");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Equal("Healthy", responseString);
    }
}
