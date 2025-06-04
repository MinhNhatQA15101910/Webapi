using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.Product;

namespace Webapi.Application.ProductCQRS.Commands.UpdateProduct;

public record UpdateProductCommand(Guid ProductId, UpdateProductDto ProductDto) : ICommand<ProductDto>;
