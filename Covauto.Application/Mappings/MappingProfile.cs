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
                .ForMember(dest => dest.kenteken, opt => opt.MapFrom(src => src.Kenteken))
                .ForMember(dest => dest.kilometerstand, opt => opt.MapFrom(src => src.Kilometerstand))
                .ReverseMap();

            CreateMap<LeenAutoReservering, LeenAutoReserveringDTO>()
                .ForMember(dest => dest.WerknemerId, opt => opt.MapFrom(src => src.WerknemerId))
                .ForMember(dest => dest.WerknemerUserName, opt => opt.MapFrom(src => src.Werknemer.UserName))
                .ForMember(dest => dest.WerknemerEmail, opt => opt.MapFrom(src => src.Werknemer.Email))
                .ForMember(dest => dest.AutoId, opt => opt.MapFrom(src => src.AutoId))
                .ForMember(dest => dest.Auto, opt => opt.MapFrom(src => src.Auto))
                .ReverseMap();

            CreateMap<LeenAutoRit, LeenAutoRitDTO>()
                .ForMember(dest => dest.WerknemerId, opt => opt.MapFrom(src => src.WerknemerId))
                .ForMember(dest => dest.AutoId, opt => opt.MapFrom(src => src.AutoId))
                .ForMember(dest => dest.VanAdres, opt => opt.MapFrom(src => src.VanAdres))
                .ForMember(dest => dest.NaarAdres, opt => opt.MapFrom(src => src.NaarAdres))
                .ForMember(dest => dest.VertrekTijd, opt => opt.MapFrom(src => src.VertrekTijd))
                .ForMember(dest => dest.AankomstTijd, opt => opt.MapFrom(src => src.AankomstTijd))
                .ForMember(dest => dest.KilometerstandBegin, opt => opt.MapFrom(src => src.KilometerstandBegin))
                .ForMember(dest => dest.KilometerstandEind, opt => opt.MapFrom(src => src.KilometerstandEind))
                .ReverseMap();
        }
    }
}