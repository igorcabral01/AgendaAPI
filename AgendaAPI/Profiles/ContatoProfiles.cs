using AgendaAPI.Data.Dtos;
using AgendaAPI.Models;
using AutoMapper;

namespace AgendaAPI.Profiles;

public class ContatoProfiles : Profile
{
    public ContatoProfiles()
    {
        CreateMap<CreatContatosDto, Contatos>();
        CreateMap<UpdateContatosDto, Contatos>();
        CreateMap<Contatos, UpdateContatosDto > ();
        CreateMap<Contatos, ReadContatosDto>();
    }
}

