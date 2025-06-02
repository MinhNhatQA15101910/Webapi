using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.ProductSizeCQRS.Commands.DeleteProductSize;

public record DeleteProductSizeCommand(Guid Id) : ICommand<ProductSizeDto>;