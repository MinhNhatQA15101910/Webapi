using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;

namespace Webapi.Application.VoucherCQRS.Commands.DeleteVoucher;

public class DeleteVoucherHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteVoucherCommand, bool>
{
    public async Task<bool> Handle(DeleteVoucherCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get existing voucher
            var voucher = await unitOfWork.VoucherRepository.GetByIdAsync(request.VoucherId, cancellationToken)
                ?? throw new BadRequestException($"Voucher with ID {request.VoucherId} not found");
            
            // Remove from repository
            unitOfWork.VoucherRepository.Remove(voucher);
            
            // Save changes
            var result = await unitOfWork.CompleteAsync(cancellationToken);
            
            return result;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BadRequestException($"Failed to delete voucher: {ex.Message}");
        }
    }
}