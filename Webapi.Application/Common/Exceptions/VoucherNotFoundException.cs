namespace Webapi.Application.Common.Exceptions;

public class VoucherNotFoundException(Guid voucherId)
    : NotFoundException($"The voucher with the identifier {voucherId} was not found.")
{
}
