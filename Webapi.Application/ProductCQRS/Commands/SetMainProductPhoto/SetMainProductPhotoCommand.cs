using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Entities;

namespace Webapi.Application.ProductCQRS.Commands.SetMainProductPhoto;

public record SetMainProductPhotoCommand(Guid ProductId, Guid PhotoId) : ICommand<ProductPhoto>;