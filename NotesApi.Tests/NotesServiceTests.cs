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
    private readonly Mock<INoteService> _noteService;

    public NotesServiceTests()
    {
        _noteService = new Mock<INoteService>();
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
