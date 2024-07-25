using NotesApi.Extensions;
using NotesApi.Models;

namespace NotesApi.Services;

public interface INoteService
{
    Task<Result<IEnumerable<Note>>> GetNotesAsync();
    Task<Result<Note>> GetNoteByIdAsync(int id);
    Task<Result> AddNoteAsync(Note note);
    Task<Result> UpdateNoteAsync(Note note);
    Task<Result> DeleteNoteAsync(int id);
    Task<Result> NoteExistsAsync(int id);
}
