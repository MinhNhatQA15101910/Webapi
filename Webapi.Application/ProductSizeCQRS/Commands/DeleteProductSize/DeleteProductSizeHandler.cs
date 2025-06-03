using AutoMapper;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.ProductSizeCQRS.Commands.DeleteProductSize;

public class DeleteProductSizeHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<DeleteProductSizeCommand, ProductSizeDto>
{
    public async Task<ProductSizeDto> Handle(DeleteProductSizeCommand request, CancellationToken cancellationToken)
    {
        // Get existing product size
        var productSize = await unitOfWork.ProductSizeRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new Exception($"Product size with ID {request.Id} not found");

        // Map to DTO before removal for return value
        var productSizeDto = mapper.Map<ProductSizeDto>(productSize);

        // Remove from repository
        unitOfWork.ProductSizeRepository.Remove(productSize);
        await unitOfWork.CompleteAsync();

        return productSizeDto;
    }
}
