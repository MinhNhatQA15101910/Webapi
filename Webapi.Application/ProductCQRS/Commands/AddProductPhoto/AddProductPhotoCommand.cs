using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.DTOs.ProductPhoto;

namespace Webapi.Application.ProductCQRS.Commands.AddProductPhoto;

public record AddProductPhotoCommand(Guid ProductId, IFormFile File) : ICommand<ProductPhotoDto>;