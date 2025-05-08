using MediatR;

namespace Webapi.Application.Common.Interfaces.MediatR;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}
