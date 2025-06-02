using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.ProductSizeCQRS.Commands.UpdateProductSize;

public record UpdateProductSizeCommand(Guid Id, UpdateProductSizeDto ProductSizeDto) : ICommand<ProductSizeDto>;