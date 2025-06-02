using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.ProductSizeCQRS.Commands.CreateProductSize;

public record CreateProductSizeCommand(CreateProductSizeDto ProductSizeDto) : ICommand<ProductSizeDto>;