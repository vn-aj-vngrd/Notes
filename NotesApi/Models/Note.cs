using System.ComponentModel.DataAnnotations;

namespace NotesApi.Models;

public class Note
{
    // Primary Key
    public int Id { get; set; }

    // Title of the Note
    [Required]
    [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
    public required string Title { get; set; }

    // Content of the Note
    [Required]
    public required string Content { get; set; }

    // Date the Note was created
    public DateTime CreatedAt { get; set; }

    // Date the Note was last updated
    public DateTime? UpdatedAt { get; set; }
}
