using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Application.Common.Models;

namespace Webapi.Application.PaymentCQRS.Commands.ConfirmPayment;

public class IpnConfirmHandler(
    IPaymentService paymentService
) : ICommandHandler<IpnConfirmCommand, IpnResponse>
{
    public async Task<IpnResponse> Handle(IpnConfirmCommand request, CancellationToken cancellationToken)
    {
        return await paymentService.Confirm(request.PaymentMethod, request.Data, request.Query, cancellationToken);
    }
}
