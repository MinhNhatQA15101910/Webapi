using AutoMapper;
using Webapi.Application.AuthCQRS.Commands.ValidateSignup;
using Webapi.Domain.Entities;
using Webapi.Domain.ValueObjects;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.DTOs.CartItem;
using Webapi.SharedKernel.DTOs.Orders;
using Webapi.SharedKernel.DTOs.Product;
using Webapi.SharedKernel.DTOs.ProductPhoto;
using Webapi.SharedKernel.DTOs.ProductSize;
using Webapi.SharedKernel.DTOs.Review;
using Webapi.SharedKernel.DTOs.Voucher;

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

        CreateMap<ProductSize, ProductSizeDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
            .ForMember(dest => dest.ProductMainPhotoUrl, opt => opt.MapFrom(src =>
                src.Product.Photos.FirstOrDefault(p => p.IsMain)!.Url));
        CreateMap<CreateProductSizeDto, ProductSize>();
        CreateMap<UpdateProductSizeDto, ProductSize>();

        CreateMap<Product, ProductDto>()
            .ForMember(
                dest => dest.Categories,
                opt => opt.MapFrom(src =>
                    src.Categories != null ?
                    src.Categories.Select(pc => new CategoryDto
                    {
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
                    src.Photos ?? new List<ProductPhoto>()))
            .ForMember(
                dest => dest.Sizes,
                opt => opt.MapFrom(src =>
                    src.Sizes ?? new List<ProductSize>()));

        CreateMap<CartItem, CartItemDto>()
            .ForMember(
                dest => dest.ProductName,
                opt => opt.MapFrom(src => src.ProductSize.Product.Name))
            .ForMember(
                dest => dest.ProductPrice,
                opt => opt.MapFrom(src => src.ProductSize.Product.Price))
            .ForMember(
                dest => dest.ProductPhotoUrl,
                opt => opt.MapFrom(src =>
                    src.ProductSize.Product.Photos != null && src.ProductSize.Product.Photos.Any(p => p.IsMain) ?
                    src.ProductSize.Product.Photos.FirstOrDefault(p => p.IsMain)!.Url :
                    null));

        CreateMap<OrderProduct, OrderProductDto>()
            .ForMember(
                dest => dest.Size,
                opt => opt.MapFrom(src => src.ProductSize.Size))
            .ForMember(
                dest => dest.ProductName,
                opt => opt.MapFrom(src => src.ProductSize.Product.Name))
            .ForMember(
                dest => dest.ProductPrice,
                opt => opt.MapFrom(src => src.ProductSize.Product.Price))
            .ForMember(
                dest => dest.ProductPhotoUrl,
                opt => opt.MapFrom(src =>
                    src.ProductSize.Product.Photos != null && src.ProductSize.Product.Photos.Any(p => p.IsMain) ?
                    src.ProductSize.Product.Photos.FirstOrDefault(p => p.IsMain)!.Url :
                    null));
        CreateMap<Address, AddressDto>();

        CreateMap<Order, OrderDto>()
            .ForMember(
                dest => dest.OwnerAvatar,
                opt => opt.MapFrom(src => src.Owner.Photos != null && src.Owner.Photos.Any(p => p.IsMain) ?
                    src.Owner.Photos.FirstOrDefault(p => p.IsMain)!.Url :
                    null))
            .ForMember(
                dest => dest.OwnerName,
                opt => opt.MapFrom(src => src.Owner.UserName))
            .ForMember(
                dest => dest.OwnerEmail,
                opt => opt.MapFrom(src => src.Owner.Email));

        CreateMap<Voucher, VoucherDto>();
        CreateMap<CreateVoucherDto, Voucher>();
        CreateMap<UpdateVoucherDto, Voucher>();
        CreateMap<Review, ReviewDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Email))
            .ForMember(dest => dest.OwnerEmail, opt => opt.MapFrom(src => src.Owner.Email));
    }
}
