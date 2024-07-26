using NotesApi.DTOs;

namespace NotesClient.Services.Contracts;

public interface INotesService
{
    Task<IEnumerable<NoteDto>> GetNotesAsync();
    // Task<NoteDto> GetNoteByIdAsync(int id);
    // Task<NoteDto> AddNoteAsync(NoteCreateDto noteCreateDto);
    // Task UpdateNoteAsync(NoteUpdateDto noteUpdateDto);
    // Task DeleteNoteAsync(int id);
}
