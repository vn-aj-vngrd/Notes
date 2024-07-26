using NotesApi.DTOs;
using NotesApi.Extensions;
using NotesApi.Models;

namespace NotesApi.Services;

public interface INoteService
{
    Task<Result<IEnumerable<Note>>> GetNotesAsync();
    Task<Result<Note>> GetNoteByIdAsync(int id);
    Task<Result<Note>> AddNoteAsync(NoteCreateDto noteDto);
    Task<Result<Note>> UpdateNoteAsync(NoteUpdateDto noteDto);
    Task<Result> DeleteNoteAsync(int id);
    Task<Result> NoteExistsAsync(int id);
}
