using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Entities;

namespace Webapi.Application.ProductCQRS.Commands.DeleteProductPhoto;

public record DeleteProductPhotoCommand(Guid ProductId, Guid PhotoId) : ICommand<ProductPhoto>;