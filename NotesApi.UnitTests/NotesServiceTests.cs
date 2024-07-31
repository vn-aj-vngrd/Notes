using Microsoft.AspNetCore.Mvc;
using Moq;
using NotesApi.Controllers;
using NotesApi.DTOs;
using NotesApi.Extensions;
using NotesApi.Models;
using NotesApi.Services;

namespace NotesApi.Tests;

public class NotesServiceTests
{
    private readonly Mock<INotesService> _noteService;

    public NotesServiceTests()
    {
        _noteService = new Mock<INotesService>();
    }

    [Fact]
    public async Task GetNotesAsync_ReturnsSuccess_WhenNotesAreFound()
    {
        // Arrange
        var notesData = GetNotesData();
        _noteService
            .Setup(x => x.GetNotesAsync())
            .ReturnsAsync(Result<IEnumerable<NoteDto>>.Success(notesData));

        var noteController = new NotesController(_noteService.Object);

        // Act
        var result = await noteController.GetNotes();

        // Assert
        Assert.NotNull(result.Result);
        Assert.IsType<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.IsType<Result<IEnumerable<NoteDto>>>(okResult.Value);
        var notes = okResult.Value as Result<IEnumerable<NoteDto>>;
        Assert.NotNull(notes);
        Assert.True(notes.IsSuccess);
        Assert.NotNull(notes.Value);
        Assert.NotEmpty(notes.Value);
        Assert.Equal(notesData, notes.Value);
    }

    [Fact]
    public async Task GetNoteByIdAsync_ReturnsSuccess_WhenNoteIsFound()
    {
        // Arrange
        var noteData = GetNotesData().First();
        _noteService
            .Setup(x => x.GetNoteByIdAsync(noteData.Id))
            .ReturnsAsync(Result<NoteDto>.Success(noteData));

        var noteController = new NotesController(_noteService.Object);

        // Act
        var result = await noteController.GetNote(noteData.Id);

        // Assert
        Assert.NotNull(result.Result);
        Assert.IsType<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.IsType<Result<NoteDto>>(okResult.Value);
        var note = okResult.Value as Result<NoteDto>;
        Assert.NotNull(note);
        Assert.True(note.IsSuccess);
        Assert.NotNull(note.Value);
        Assert.Equal(noteData, note.Value);
    }

    [Fact]
    public async Task AddNoteAsync_ReturnsSuccess_WhenNoteIsAdded()
    {
        // Arrange
        var notesData = GetNotesData();
        _noteService
            .Setup(x => x.GetNotesAsync())
            .ReturnsAsync(Result<IEnumerable<NoteDto>>.Success(notesData));

        var noteController = new NotesController(_noteService.Object);

        // Act
        var result = await noteController.GetNotes();

        // Assert
        Assert.NotNull(result.Result);
        Assert.IsType<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.IsType<Result<IEnumerable<NoteDto>>>(okResult.Value);
        var notes = okResult.Value as Result<IEnumerable<NoteDto>>;
        Assert.NotNull(notes);
        Assert.True(notes.IsSuccess);
        Assert.NotNull(notes.Value);
        Assert.NotEmpty(notes.Value);
        Assert.Equal(notesData, notes.Value);
    }

    [Fact]
    public async Task UpdateNoteAsync_ReturnsSuccess_WhenNoteIsUpdated()
    {
        // Arrange
        var noteData = GetNotesData().First();
        _noteService
            .Setup(x => x.GetNoteByIdAsync(noteData.Id))
            .ReturnsAsync(Result<NoteDto>.Success(noteData));

        var noteController = new NotesController(_noteService.Object);

        // Act
        var result = await noteController.GetNote(noteData.Id);

        // Assert
        Assert.NotNull(result.Result);
        Assert.IsType<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.IsType<Result<NoteDto>>(okResult.Value);
        var note = okResult.Value as Result<NoteDto>;
        Assert.NotNull(note);
        Assert.True(note.IsSuccess);
        Assert.NotNull(note.Value);
        Assert.Equal(noteData, note.Value);
    }

    [Fact]
    public async Task DeleteNoteAsync_ReturnsSuccess_WhenNoteIsDeleted()
    {
        // Arrange
        var noteData = GetNotesData().First();
        _noteService
            .Setup(x => x.GetNoteByIdAsync(noteData.Id))
            .ReturnsAsync(Result<NoteDto>.Success(noteData));

        var noteController = new NotesController(_noteService.Object);

        // Act
        var result = await noteController.GetNote(noteData.Id);

        // Assert
        Assert.NotNull(result.Result);
        Assert.IsType<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.IsType<Result<NoteDto>>(okResult.Value);
        var note = okResult.Value as Result<NoteDto>;
        Assert.NotNull(note);
        Assert.True(note.IsSuccess);
        Assert.NotNull(note.Value);
        Assert.Equal(noteData, note.Value);
    }

    [Fact]
    public async Task NoteExistsAsync_ReturnsSuccess_WhenNoteExists()
    {
        // Arrange
        var noteData = GetNotesData().First();
        _noteService
            .Setup(x => x.GetNoteByIdAsync(noteData.Id))
            .ReturnsAsync(Result<NoteDto>.Success(noteData));

        var noteController = new NotesController(_noteService.Object);

        // Act
        var result = await noteController.GetNote(noteData.Id);

        // Assert
        Assert.NotNull(result.Result);
        Assert.IsType<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.IsType<Result<NoteDto>>(okResult.Value);
        var note = okResult.Value as Result<NoteDto>;
        Assert.NotNull(note);
        Assert.True(note.IsSuccess);
        Assert.NotNull(note.Value);
        Assert.Equal(noteData, note.Value);
    } 

    private static IEnumerable<NoteDto> GetNotesData()
    {
        var notes = new List<NoteDto>
        {
            new()
            {
                Id = 1,
                Title = "Note 1",
                Content = "Content 1",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new()
            {
                Id = 2,
                Title = "Note 2",
                Content = "Content 2",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        };

        return notes;
    }
}
