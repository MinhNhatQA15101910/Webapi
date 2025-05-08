using MediatR;

namespace Webapi.Application.Common.Interfaces.MediatR;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
