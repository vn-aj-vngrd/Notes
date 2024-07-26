using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

    [Inject]
    public NavigationManager Navigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadNote();
    }

    private async Task LoadNote()
    {
        try
        {
            _note = await _service.GetNoteByIdAsync(Id);
        }
        catch (Exception)
        {
            Navigation.NavigateTo("/");
        }
    }

    private async Task HandleDeleteNote()
    {
        await _service.DeleteNoteAsync(Id);
        Navigation.NavigateTo("/");
    }
}
