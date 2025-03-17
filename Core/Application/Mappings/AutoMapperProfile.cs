using Application.DTOs.Auth;
using Application.DTOs.Photos;
using Application.DTOs.Users;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<UserPhoto, PhotoDto>();
        CreateMap<User, UserDto>()
            .ForMember(
                d => d.PhotoUrl,
                o => o.MapFrom(
                    s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url
                )
            )
            .ForMember(
                d => d.Roles,
                o => o.MapFrom(
                    s => s.UserRoles.Select(x => x.Role.Name)
                )
            );
        CreateMap<ValidateSignupDto, User>();
    }
}
