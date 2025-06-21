using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.DTOs.ProductSize;

namespace Webapi.Application.ProductSizeCQRS.Commands.UpdateProductSize;

public record UpdateProductSizeCommand(Guid Id, UpdateProductSizeDto ProductSizeDto) : ICommand<ProductSizeDto>;