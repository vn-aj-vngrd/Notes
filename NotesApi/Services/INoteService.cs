using NotesApi.DTOs;
using NotesApi.Extensions;
using NotesApi.Models;

namespace NotesApi.Services;

public interface INoteService
{
    Task<Result<IEnumerable<NoteDto>>> GetNotesAsync();
    Task<Result<NoteDto>> GetNoteByIdAsync(int id);
    Task<Result<NoteDto>> AddNoteAsync(NoteCreateDto noteCreateDto);
    Task<Result<NoteDto>> UpdateNoteAsync(NoteUpdateDto noteUpdateDto);
    Task<Result> DeleteNoteAsync(int id);
    Task<Result> NoteExistsAsync(int id);
}
