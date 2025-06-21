using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.DTOs.ProductSize;

namespace Webapi.Application.ProductSizeCQRS.Commands.CreateProductSize;

public record CreateProductSizeCommand(CreateProductSizeDto ProductSizeDto) : ICommand<ProductSizeDto>;