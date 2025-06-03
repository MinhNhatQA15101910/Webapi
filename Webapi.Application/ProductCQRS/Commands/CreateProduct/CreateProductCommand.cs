using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.Product;

namespace Webapi.Application.ProductCQRS.Commands.CreateProduct;

public record CreateProductCommand(CreateProductDto ProductDto) : ICommand<ProductDto>, ICommand<SharedKernel.DTOs.ProductDto>;