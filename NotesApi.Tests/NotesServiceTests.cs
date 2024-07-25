using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using NotesApi.Data;
using NotesApi.Models;
using NotesApi.Services;

namespace NotesApi.Tests;

public class NotesServiceTests
{
    private readonly NoteService _service;
    private readonly AppDbContext _context;

    public NotesServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "NotesTestDatabase_" + Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _service = new NoteService(_context);
    }

    [Fact]
    public async Task AddNoteAsync_ReturnsSuccess_WhenNoteIsAdded()
    {
        // Arrange
        var note = new Note
        {
            Id = 1,
            Title = "Test Note",
            Content = "Test Content"
        };

        // Act
        var result = await _service.AddNoteAsync(note);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GetNotesAsync_ReturnsListOfNotes()
    {
        // Arrange
        var note1 = new Note
        {
            Id = 1,
            Title = "Test Note 1",
            Content = "Test Content 1"
        };

        var note2 = new Note
        {
            Id = 2,
            Title = "Test Note 2",
            Content = "Test Content 2"
        };

        _context.Notes.Add(note1);
        _context.Notes.Add(note2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetNotesAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value);
        Assert.Contains(result.Value, n => n.Id == 1);
        Assert.Contains(result.Value, n => n.Id == 2);
    }

    [Fact]
    public async Task GetNoteByIdAsync_ReturnsNote()
    {
        // Arrange
        var note = new Note
        {
            Id = 1,
            Title = "Test Note",
            Content = "Test Content"
        };

        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetNoteByIdAsync(note.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(note.Id, result.Value.Id);
    }

    [Fact]
    public async Task UpdateNoteAsync_ReturnsSuccess_WhenNoteIsUpdated()
    {
        // Arrange
        var note = new Note
        {
            Id = 1,
            Title = "Original Title",
            Content = "Original Content"
        };

        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        note.Title = "Updated Title";
        note.Content = "Updated Content";

        // Act
        var result = await _service.UpdateNoteAsync(note);

        // Assert
        Assert.True(result.IsSuccess);

        var updatedNote = await _context.Notes.FindAsync(note.Id);
        Assert.Equal("Updated Title", updatedNote.Title);
        Assert.Equal("Updated Content", updatedNote.Content);
    }

    [Fact]
    public async Task DeleteNoteAsync_ReturnsSuccess_WhenNoteIsDeleted()
    {
        // Arrange
        var note = new Note
        {
            Id = 2,
            Title = "Test Note",
            Content = "Test Content"
        };

        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.DeleteNoteAsync(note.Id);

        // Assert
        Assert.True(result.IsSuccess);

        var deletedNote = await _context.Notes.FindAsync(note.Id);
        Assert.Null(deletedNote);
    }

    [Fact]
    public async Task NoteExistsAsync_ReturnsTrue_WhenNoteExists()
    {
        // Arrange
        var note = new Note
        {
            Id = 1,
            Title = "Test Note",
            Content = "Test Content"
        };

        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.NoteExistsAsync(note.Id);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task NoteExistsAsync_ReturnsFalse_WhenNoteDoesNotExist()
    {
        // Arrange
        var note = new Note
        {
            Id = 1,
            Title = "Test Note",
            Content = "Test Content"
        };

        // Act
        var result = await _service.NoteExistsAsync(note.Id);

        // Assert
        Assert.False(result.IsSuccess);
    }
}
