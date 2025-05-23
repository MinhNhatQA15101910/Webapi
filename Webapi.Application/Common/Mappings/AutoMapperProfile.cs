
using AutoMapper;
using Webapi.Application.AuthCQRS.Commands.ValidateSignup;
using Webapi.Domain.Entities;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.Common.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserPhoto, PhotoDto>();
        CreateMap<User, UserDto>()
            .ForMember(
                dest => dest.Roles,
                opt => opt.MapFrom(
                    src => src.UserRoles.Select(
                        ur => ur.Role.Name).ToList()))
            .ForMember(
                dest => dest.PhotoUrl,
                opt => opt.MapFrom(
                    src => src.Photos.FirstOrDefault(
                        p => p.IsMain)!.Url));
        CreateMap<ValidateSignupDto, User>();
    }
}
