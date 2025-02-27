// File: Dtos/MappingProfile.cs
using AutoMapper;
using Gauniv.WebServer.Data;
using System.Linq;

namespace Gauniv.WebServer.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Game, GameDto>()
                .ForMember(dest => dest.Categories,
                           opt => opt.MapFrom(src => src.Categories.Select(c => c.Nom).ToList()));
        }
    }
}
