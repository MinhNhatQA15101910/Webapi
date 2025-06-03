using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.ProductCQRS.Commands.AddProductPhoto;

public record AddProductPhotoCommand(Guid ProductId, IFormFile File) : ICommand<ProductPhotoDto>;