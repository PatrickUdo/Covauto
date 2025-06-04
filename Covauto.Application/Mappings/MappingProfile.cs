using AutoMapper;
using Covauto.Domain.Entities;
using Covauto.Shared.DTOs;

namespace Covauto.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Auto, AutoDTO>()
                .ForMember(dest => dest.Kenteken, opt => opt.MapFrom(src => src.Kenteken))
                .ForMember(dest => dest.Kilometerstand, opt => opt.MapFrom(src => src.Kilometerstand))
                .ReverseMap();

            CreateMap<LeenAutoReservering, LeenAutoReserveringDTO>()
               .ForMember(dest => dest.WerknemerId, opt => opt.MapFrom(src => src.WerknemerId))
               .ForMember(dest => dest.WerknemerUserName, opt => opt.MapFrom(src => src.Werknemer.UserName))
               .ForMember(dest => dest.WerknemerEmail, opt => opt.MapFrom(src => src.Werknemer.Email))
               .ForMember(dest => dest.AutoId, opt => opt.MapFrom(src => src.AutoId))
               .ForMember(dest => dest.Auto, opt => opt.MapFrom(src => src.Auto))
               .ReverseMap()
               .ForMember(dest => dest.Auto, opt => opt.Ignore())
               .ForMember(dest => dest.Werknemer, opt => opt.Ignore());
        }
    }
}