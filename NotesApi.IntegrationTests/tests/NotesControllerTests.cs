using System.Net.Http.Json;
using System.Text.Json;
using NotesApi.DTOs;
using NotesApi.Extensions;
using NotesApi.IntegrationTests.Factory;
using NotesClient.Models;

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
        var content = await response.Content.ReadAsStringAsync();

        var apiResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<NoteDto>>>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        Assert.True(apiResponse?.IsSuccess);
        Assert.NotNull(apiResponse?.Value);
        Assert.Contains(apiResponse.Value, n => n.Title == note.Title);
    }

    [Fact]
    public async Task GetNote_ReturnsNote()
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
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        var apiResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<NoteDto>>>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        var noteId = apiResponse?.Value?.FirstOrDefault()?.Id;
        var noteResponse = await client.GetAsync($"api/Notes/{noteId}");

        // Assert
        noteResponse.EnsureSuccessStatusCode();
        var noteContent = await noteResponse.Content.ReadAsStringAsync();

        var noteApiResponse = JsonSerializer.Deserialize<ApiResponse<NoteDto>>(
            noteContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        Assert.True(noteApiResponse?.IsSuccess);
        Assert.NotNull(noteApiResponse?.Value);
        Assert.Equal(noteApiResponse.Value?.Title, note.Title);
    }
}
