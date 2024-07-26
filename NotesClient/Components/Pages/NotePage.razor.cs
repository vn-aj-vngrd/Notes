using Microsoft.AspNetCore.Components;
using NotesApi.DTOs;
using NotesClient.Services.Contracts;

namespace NotesClient.Components.Pages;

public partial class NotePage
{
    [Parameter]
    public int Id { get; set; }

    [Inject]
    public required INotesService _service { get; set; }

    private NoteDto? _note;

    protected override async Task OnInitializedAsync()
    {
        await LoadNote();
    }

    private async Task LoadNote() { 
        _note = await _service.GetNoteByIdAsync(Id);
    }
}
