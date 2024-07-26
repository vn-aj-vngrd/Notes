namespace NotesApi.DTOs;

public class NoteUpdateDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
}
