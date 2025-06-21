using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.DTOs.Product;

namespace Webapi.Application.ProductCQRS.Commands.DeleteProduct;

public record DeleteProductCommand(Guid ProductId) : ICommand<ProductDto>;