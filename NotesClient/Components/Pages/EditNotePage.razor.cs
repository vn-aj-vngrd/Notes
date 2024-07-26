using Microsoft.AspNetCore.Components;
using NotesApi.DTOs;
using NotesClient.Services.Contracts;

namespace NotesClient.Components.Pages;

partial class EditNotePage
{
    [Parameter]
    public int Id { get; set; }

    [Inject]
    public required INotesService _service { get; set; }

    [Inject]
    public NavigationManager Navigation { get; set; }
    
    private NoteDto? _note;
    private string noteTitle = string.Empty;
    private string noteContent = string.Empty;
    private bool _isSuccess = false;
    private string _message = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await LoadNote();
    }

    private async Task LoadNote()
    {
        try
        {
            _note = await _service.GetNoteByIdAsync(Id);
            noteTitle = _note.Title;
            noteContent = _note.Content;
        }
        catch (Exception)
        {
            Navigation.NavigateTo("/");
        }
    }

    private async Task HandleSubmit()
    {
        try
        {
            await _service.UpdateNoteAsync(new NoteUpdateDto
            {
                Id = _note!.Id,
                Title = _note.Title,
                Content = _note.Content
            });
            _isSuccess = true;
            _message = "Note updated successfully!";
        }
        catch (Exception ex)
        {
            _isSuccess = false;
            _message = ex.Message;
        }
    }
}
