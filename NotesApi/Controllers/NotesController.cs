using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
    {
        var notes = await _noteService.GetNotesAsync();
        return Ok(notes);
    }

    // GET: api/Notes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Note>> GetNote(int id)
    {
        var note = await _noteService.GetNoteByIdAsync(id);

        if (note == null)
        {
            return NotFound();
        }

        return Ok(note);
    }

    // PUT: api/Notes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutNote(int id, Note note)
    {
        if (id != note.Id)
        {
            return BadRequest();
        }

        try
        {
            await _noteService.UpdateNoteAsync(note);
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

        return NoContent();
    }

    // POST: api/Notes
    [HttpPost]
    public async Task<ActionResult<Note>> PostNote(Note note)
    {
        var result = await _noteService.AddNoteAsync(note);
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetNote), new { id = note.Id }, note);
        }

        return BadRequest(result.Error);
    }

    // DELETE: api/Notes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNote(int id)
    {
        await _noteService.DeleteNoteAsync(id);
        return NoContent();
    }
}
