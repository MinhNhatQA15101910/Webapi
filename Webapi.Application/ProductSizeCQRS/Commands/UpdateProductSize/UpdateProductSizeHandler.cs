using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.DTOs.ProductSize;

namespace Webapi.Application.ProductSizeCQRS.Commands.UpdateProductSize;

public class UpdateProductSizeHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<UpdateProductSizeCommand, ProductSizeDto>
{
    public async Task<ProductSizeDto> Handle(UpdateProductSizeCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext.User.GetUserId();
        
        // Get existing product size
        var productSize = await unitOfWork.ProductSizeRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new Exception($"Product size with ID {request.Id} not found");
            
        // Update properties
        productSize.Size = request.ProductSizeDto.Size;
        productSize.Quantity = request.ProductSizeDto.Quantity;
        productSize.UpdatedAt = DateTime.UtcNow;
        
        // Update in repository
        unitOfWork.ProductSizeRepository.Update(productSize);
        await unitOfWork.CompleteAsync();
        
        // Return mapped DTO
        return mapper.Map<ProductSizeDto>(productSize);
    }
}