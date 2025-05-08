using MediatR;

namespace Webapi.Application.Common.Interfaces.MediatR;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
