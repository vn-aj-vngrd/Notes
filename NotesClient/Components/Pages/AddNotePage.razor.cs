using Microsoft.AspNetCore.Components;
using NotesApi.DTOs;
using NotesClient.Services.Contracts;

namespace NotesClient.Components.Pages;

partial class AddNotePage
{
    [Inject]
    public required INotesService _service { get; set; }
    private string noteTitle = string.Empty;
    private string noteContent = string.Empty;
    private bool isSuccess = false;
    private string message = string.Empty;

    private async Task HandleSubmit()
    {
        try
        {
            var newNote = new NoteCreateDto { Title = noteTitle, Content = noteContent, };

            var response = await _service.AddNoteAsync(newNote);
            if (response.Content == null) { }

            isSuccess = true;
            message = "Note added successfully.";
            noteTitle = string.Empty;
            noteContent = string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
