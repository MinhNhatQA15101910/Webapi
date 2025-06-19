using AutoMapper;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.Domain.Interfaces.States;
using Webapi.SharedKernel.DTOs.Orders;

namespace Webapi.Application.OrdersCQRS.Commands.ProceedOrder;

public class ProceedOrderHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<ProceedOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(ProceedOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.OrderRepository.GetOrderByIdAsync(request.OrderId, cancellationToken)
            ?? throw new OrderNotFoundException(request.OrderId);

        var orderContext = new OrderContext(order);
        orderContext.NextState();

        unitOfWork.OrderRepository.Update(order);

        if (!await unitOfWork.CompleteAsync(cancellationToken))
        {
            throw new BadRequestException("Order cannot proceed to the next state.");
        }

        return mapper.Map<OrderDto>(order);
    }
}
