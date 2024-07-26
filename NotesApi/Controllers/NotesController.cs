using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApi.DTOs;
using NotesApi.Extensions;
using NotesApi.Models;
using NotesApi.Services;

namespace NotesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly INoteService _noteService;

    public NotesController(INoteService noteService)
    {
        _noteService = noteService;
    }

    // GET: api/Notes
    [HttpGet]
    public async Task<ActionResult<Result<IEnumerable<NoteDto>>>> GetNotes()
    {
        var notes = await _noteService.GetNotesAsync();
        return Ok(notes);
    }

    // GET: api/Notes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Result<NoteDto>>> GetNote(int id)
    {
        var note = await _noteService.GetNoteByIdAsync(id);

        if (note == null)
        {
            return NotFound();
        }

        return Ok(note);
    }

    // POST: api/Notes
    [HttpPost]
    public async Task<ActionResult<Result<NoteDto>>> PostNote(NoteCreateDto note)
    {
        var result = await _noteService.AddNoteAsync(note);
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetNote), new { id = result.Value }, result);
        }

        return BadRequest(result.Error);
    }

    // PUT: api/Notes/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Result<NoteDto>>> PutNote(int id, NoteUpdateDto note)
    {
        if (id != note.Id)
        {
            return BadRequest();
        }

        try
        {
            var result = await _noteService.UpdateNoteAsync(note);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
        }
        catch (DbUpdateConcurrencyException)
        {
            var noteExists = await _noteService.NoteExistsAsync(id);
            if (!noteExists.IsSuccess)
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return BadRequest();
    }

    // DELETE: api/Notes/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Result>> DeleteNote(int id)
    {
        var result = await _noteService.DeleteNoteAsync(id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
