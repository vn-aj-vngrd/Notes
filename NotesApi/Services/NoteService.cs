using Microsoft.EntityFrameworkCore;
using NotesApi.Data;
using NotesApi.Extensions;
using NotesApi.Models;

namespace NotesApi.Services;

public class NoteService : INoteService
{
    private readonly AppDbContext _context;

    public NoteService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<Note>>> GetNotesAsync()
    {
        return await Task.Run(() =>
        {
            try
            {
                var notes = _context.Notes.ToList();
                return Result<IEnumerable<Note>>.Success(notes);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Note>>.Failure($"Error retrieving notes: {ex.Message}");
            }
        });
    }

    public async Task<Result<Note>> GetNoteByIdAsync(int id)
    {
        return await Task.Run(async () =>
        {
            try
            {
                var note = await _context.Notes.FindAsync(id);
                return note != null
                    ? Result<Note>.Success(note)
                    : Result<Note>.Failure("Note not found.");
            }
            catch (Exception ex)
            {
                return Result<Note>.Failure($"Error retrieving note: {ex.Message}");
            }
        });
    }

    public async Task<Result> AddNoteAsync(Note note)
    {
        return await Task.Run(async () =>
        {
            try
            {
                _context.Notes.Add(note);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error adding note: {ex.Message}");
            }
        });
    }

    public async Task<Result> UpdateNoteAsync(Note note)
    {
        return await Task.Run(async () =>
        {
            try
            {
                _context.Entry(note).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (DbUpdateConcurrencyException)
            {
                return await _context.Notes.AnyAsync(e => e.Id == note.Id)
                    ? Result.Failure("Concurrency error.")
                    : Result.Failure("Note not found.");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error updating note: {ex.Message}");
            }
        });
    }

    public async Task<Result> DeleteNoteAsync(int id)
    {
        return await Task.Run(async () =>
        {
            try
            {
                var note = await _context.Notes.FindAsync(id);
                if (note == null)
                    return Result.Failure("Note not found.");

                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error deleting note: {ex.Message}");
            }
        });
    }

    public async Task<Result> NoteExistsAsync(int id)
    {
        return await Task.Run(async () =>
        {
            try
            {
                return await _context.Notes.AnyAsync(e => e.Id == id)
                    ? Result.Success()
                    : Result.Failure("Note not found.");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error checking for note: {ex.Message}");
            }
        });
    }
}
