using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.DTOs.ProductSize;

namespace Webapi.Application.ProductSizeCQRS.Commands.DeleteProductSize;

public record DeleteProductSizeCommand(Guid Id) : ICommand<ProductSizeDto>;