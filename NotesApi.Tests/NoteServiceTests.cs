using Microsoft.EntityFrameworkCore;
using Moq;
using NotesApi.Data;
using NotesApi.Models;
using NotesApi.Services;

namespace NotesApi.Tests;

public class NoteServiceTests
{
    private readonly NoteService _service;
    private readonly Mock<AppDbContext> _mockContext;

    public NoteServiceTests()
    {
        _mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        _service = new NoteService(_mockContext.Object);
    }

    [Fact]
    public async Task GetNoteByIdAsync_ReturnsNote_WhenNoteExists()
    {
        // Arrange
        var noteId = 1;
        var note = new Note
        {
            Id = noteId,
            Title = "Test Note",
            Content = "Test Content"
        };
        var notes = new List<Note> { note }.AsQueryable();

        var mockSet = new Mock<DbSet<Note>>();
        mockSet.As<IQueryable<Note>>().Setup(m => m.Provider).Returns(notes.Provider);
        mockSet.As<IQueryable<Note>>().Setup(m => m.Expression).Returns(notes.Expression);
        mockSet.As<IQueryable<Note>>().Setup(m => m.ElementType).Returns(notes.ElementType);
        mockSet.As<IQueryable<Note>>().Setup(m => m.GetEnumerator()).Returns(notes.GetEnumerator());

        _mockContext.Setup(c => c.Notes).Returns(mockSet.Object);

        // Act
        var result = await _service.GetNoteByIdAsync(noteId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(note, result.Value);
    }

    [Fact]
    public async Task GetNoteByIdAsync_ReturnsFailure_WhenNoteDoesNotExist()
    {
        // Arrange
        var noteId = 1;
        var notes = new List<Note>().AsQueryable();

        var mockSet = new Mock<DbSet<Note>>();
        mockSet.As<IQueryable<Note>>().Setup(m => m.Provider).Returns(notes.Provider);
        mockSet.As<IQueryable<Note>>().Setup(m => m.Expression).Returns(notes.Expression);
        mockSet.As<IQueryable<Note>>().Setup(m => m.ElementType).Returns(notes.ElementType);
        mockSet.As<IQueryable<Note>>().Setup(m => m.GetEnumerator()).Returns(notes.GetEnumerator());

        _mockContext.Setup(c => c.Notes).Returns(mockSet.Object);

        // Act
        var result = await _service.GetNoteByIdAsync(noteId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Note not found.", result.Error);
    }

    // Additional tests for AddNoteAsync, UpdateNoteAsync, DeleteNoteAsync
}
