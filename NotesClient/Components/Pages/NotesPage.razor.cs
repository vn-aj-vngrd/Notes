using Microsoft.AspNetCore.Components;
using NotesApi.DTOs;
using NotesClient.Services.Contracts;

namespace NotesClient.Components.Pages;

public partial class NotesPage
{
    [Inject]
    public required INotesService _service { get; set; }

    private IQueryable<NoteDto>? _notes;

    protected override async Task OnInitializedAsync()
    {
        await LoadNotes();
    }

    private async Task LoadNotes()
    {
        _notes = (await _service.GetNotesAsync()).AsQueryable();
    }
}
