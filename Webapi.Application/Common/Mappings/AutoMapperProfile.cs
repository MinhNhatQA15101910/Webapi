using AutoMapper;
using Webapi.Application.AuthCQRS.Commands.ValidateSignup;
using Webapi.Domain.Entities;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.DTOs.CartItem;
using Webapi.SharedKernel.DTOs.Product;
using Webapi.SharedKernel.DTOs.ProductPhoto;
using Webapi.SharedKernel.DTOs.ProductSize;

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
        
        CreateMap<ProductPhoto, ProductPhotoDto>();
        
        // Fix the circular reference issue
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.Products, opt => opt.Ignore()); // Ignore Products to prevent circular mapping

        CreateMap<ProductSize, ProductSizeDto>();
        CreateMap<CreateProductSizeDto, ProductSize>();
        CreateMap<UpdateProductSizeDto, ProductSize>();
        
        CreateMap<Product, ProductDto>()
            .ForMember(
                dest => dest.Categories,
                opt => opt.MapFrom(src => 
                    src.Categories != null ? 
                    src.Categories.Select(pc => new CategoryDto { 
                        Id = pc.Category.Id, 
                        Name = pc.Category.Name 
                        // Don't include Products here to avoid circular reference
                    }).ToList() : 
                    new List<CategoryDto>()))
            .ForMember(
                dest => dest.MainPhotoUrl,
                opt => opt.MapFrom(src => 
                    src.Photos != null && src.Photos.Any(p => p.IsMain) ? 
                    src.Photos.FirstOrDefault(p => p.IsMain)!.Url : 
                    null))
            .ForMember(
                dest => dest.Photos,
                opt => opt.MapFrom(src => 
                    src.Photos != null ? src.Photos : new List<ProductPhoto>()))
            .ForMember(
                dest => dest.Sizes,
                opt => opt.MapFrom(src => 
                    src.Sizes != null ? src.Sizes : new List<ProductSize>()));

        CreateMap<CartItem, CartItemDto>()
            .ForMember(
                dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(
                dest => dest.ProductPrice,
                opt => opt.MapFrom(src => src.Product.Price))
            .ForMember(
                dest => dest.ProductPhotoUrl,
                opt => opt.MapFrom(src => 
                    src.Product.Photos != null && src.Product.Photos.Any(p => p.IsMain) ? 
                    src.Product.Photos.FirstOrDefault(p => p.IsMain)!.Url : 
                    null));
    }
}
