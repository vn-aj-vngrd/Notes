using Microsoft.EntityFrameworkCore;
using NotesApi.Data;
using NotesApi.DTOs;
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
        try
        {
            var notes = await _context.Notes.ToListAsync();
            return Result<IEnumerable<Note>>.Success(notes);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Note>>.Failure($"Error retrieving notes: {ex.Message}");
        }
    }

    public async Task<Result<Note>> GetNoteByIdAsync(int id)
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
    }

    public async Task<Result<int>> AddNoteAsync(NoteCreateDto noteDto)
    {
        try
        {
            var note = new Note { Title = noteDto.Title, Content = noteDto.Content };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            return Result<int>.Success(note.Id);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure($"Error adding note: {ex.Message}");
        }
    }

    public async Task<Result> UpdateNoteAsync(NoteUpdateDto noteDto)
    {
        try
        {
            var note = await _context.Notes.FindAsync(noteDto.Id);
            if (note == null)
                return Result.Failure("Note not found.");

            // Update the note properties
            note.Title = noteDto.Title;
            note.Content = noteDto.Content;

            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result.Failure("Concurrency error.");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error updating note: {ex.Message}");
        }
    }

    public async Task<Result> DeleteNoteAsync(int id)
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
    }

    public async Task<Result> NoteExistsAsync(int id)
    {
        try
        {
            bool exists = await _context.Notes.AnyAsync(e => e.Id == id);
            return exists ? Result.Success() : Result.Failure("Note not found.");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error checking for note: {ex.Message}");
        }
    }
}
