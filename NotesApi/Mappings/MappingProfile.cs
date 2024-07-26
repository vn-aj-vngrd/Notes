using AutoMapper;
using NotesApi.DTOs;
using NotesApi.Models;

namespace NotesApi.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Note, NoteDto>();
    }
}
