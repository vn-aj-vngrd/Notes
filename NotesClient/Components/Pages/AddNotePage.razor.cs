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
    private bool _isSuccess = false;
    private string _message = string.Empty;

    private async Task HandleSubmit()
    {
        try
        {
            var newNote = new NoteCreateDto { Title = noteTitle, Content = noteContent, };

            var response = await _service.AddNoteAsync(newNote);
            if (response.Content == null)
            {
                _isSuccess = false;
                _message = "Failed to add note.";
                return;
            }

            _isSuccess = true;
            _message = "Note added successfully.";
            noteTitle = string.Empty;
            noteContent = string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
