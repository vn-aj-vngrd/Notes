using System.Net.Http.Json;
using NotesApi.DTOs;
using NotesApi.IntegrationTests.Factory;

namespace NotesApi.IntegrationTests.Tests;

public class NotesControllerTests
{
    [Fact]
    public async Task GetNotes_ReturnsNotes()
    {
        // Arrange
        using var factory = new NotesApiFactory();
        var client = factory.CreateClient();

        // Add a note
        var note = new NoteCreateDto { Title = "Test", Content = "Test" };
        var postResponse = await client.PostAsJsonAsync("api/Notes", note);
        postResponse.EnsureSuccessStatusCode();

        // Act
        var response = await client.GetAsync("api/Notes");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
