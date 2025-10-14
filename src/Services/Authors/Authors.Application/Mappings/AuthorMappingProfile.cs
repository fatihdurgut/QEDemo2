using AutoMapper;
using Authors.Application.DTOs;
using Authors.Domain.Aggregates;

namespace Authors.Application.Mappings;

/// <summary>
/// AutoMapper profile for Authors
/// </summary>
public class AuthorMappingProfile : Profile
{
    public AuthorMappingProfile()
    {
        CreateMap<Author, AuthorDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.LastName))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name.FullName))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone.Value))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address != null ? src.Address.Street : null))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address != null ? src.Address.City : null))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Address != null ? src.Address.State : null))
            .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Address != null ? src.Address.ZipCode : null));
    }
}
