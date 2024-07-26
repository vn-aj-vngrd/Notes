using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotesApi.Data;
using NotesApi.DTOs;
using NotesApi.Extensions;
using NotesApi.Models;

namespace NotesApi.Services;

public class NotesService : INotesService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public NotesService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<NoteDto>>> GetNotesAsync()
    {
        try
        {
            var notes = await _context.Notes.ToListAsync();
            var notesDto = _mapper.Map<IEnumerable<NoteDto>>(notes);
            return Result<IEnumerable<NoteDto>>.Success(notesDto);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<NoteDto>>.Failure($"Error retrieving notes: {ex.Message}");
        }
    }

    public async Task<Result<NoteDto>> GetNoteByIdAsync(int id)
    {
        try
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
                return Result<NoteDto>.Failure("Note not found.");

            var noteDto = _mapper.Map<NoteDto>(note);
            return Result<NoteDto>.Success(noteDto);
        }
        catch (Exception ex)
        {
            return Result<NoteDto>.Failure($"Error retrieving note: {ex.Message}");
        }
    }

    public async Task<Result<NoteDto>> AddNoteAsync(NoteCreateDto noteCreateDto)
    {
        try
        {
            var note = new Note
            {
                Title = noteCreateDto.Title,
                Content = noteCreateDto.Content,
                CreatedAt = DateTime.Now,
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            var noteDto = _mapper.Map<NoteDto>(note);
            return Result<NoteDto>.Success(noteDto);
        }
        catch (Exception ex)
        {
            return Result<NoteDto>.Failure($"Error adding note: {ex.Message}");
        }
    }

    public async Task<Result<NoteDto>> UpdateNoteAsync(NoteUpdateDto noteUpdateDto)
    {
        try
        {
            var note = await _context.Notes.FindAsync(noteUpdateDto.Id);
            if (note == null)
                return Result<NoteDto>.Failure("Note not found.");

            // Update the note properties
            note.Title = noteUpdateDto.Title;
            note.Content = noteUpdateDto.Content;
            note.UpdatedAt = DateTime.Now;

            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            var noteDto = _mapper.Map<NoteDto>(note);
            return Result<NoteDto>.Success(noteDto);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result<NoteDto>.Failure("Note not found.");
        }
        catch (Exception ex)
        {
            return Result<NoteDto>.Failure($"Error updating note: {ex.Message}");
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
