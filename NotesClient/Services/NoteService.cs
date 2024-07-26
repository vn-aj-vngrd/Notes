using System.Text;
using System.Text.Json;
using NotesApi.DTOs;
using NotesClient.Models;
using NotesClient.Services.Contracts;

namespace NotesClient.Services;

public class NotesService : INotesService
{
    private readonly HttpClient _httpClient;
    private const string _baseUri = "api/notes";

    public NotesService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("NotesApi");
    }

    public async Task<IEnumerable<NoteDto>> GetNotesAsync()
    {
        var response = await _httpClient.GetAsync(_baseUri);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var apiResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<NoteDto>>>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        if (apiResponse == null || !apiResponse.IsSuccess)
        {
            throw new Exception(apiResponse?.Error ?? "Unknown error occurred.");
        }

        return apiResponse.Value ?? [];
    }

    public async Task<NoteDto> GetNoteByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_baseUri}/{id}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var apiResponse = JsonSerializer.Deserialize<ApiResponse<NoteDto>>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        if (apiResponse == null || !apiResponse.IsSuccess)
        {
            throw new Exception(apiResponse?.Error ?? "Unknown error occurred.");
        }

        return apiResponse.Value ?? throw new Exception("Note not found.");
    }

    public async Task<NoteDto> AddNoteAsync(NoteCreateDto noteCreateDto)
    {
        var noteCreateJson = JsonSerializer.Serialize(noteCreateDto);
        var noteCreateContent = new StringContent(
            noteCreateJson,
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(_baseUri, noteCreateContent);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var apiResponse = JsonSerializer.Deserialize<ApiResponse<NoteDto>>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        if (apiResponse == null || !apiResponse.IsSuccess)
        {
            throw new Exception(apiResponse?.Error ?? "Unknown error occurred.");
        }

        return apiResponse.Value ?? throw new Exception("Note not found.");
    }

    public async Task UpdateNoteAsync(NoteUpdateDto noteUpdateDto)
    {
        var noteUpdateJson = JsonSerializer.Serialize(noteUpdateDto);
        var noteUpdateContent = new StringContent(
            noteUpdateJson,
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PutAsync(
            $"{_baseUri}/{noteUpdateDto.Id}",
            noteUpdateContent
        );
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteNoteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUri}/{id}");
        response.EnsureSuccessStatusCode();
    }
}
