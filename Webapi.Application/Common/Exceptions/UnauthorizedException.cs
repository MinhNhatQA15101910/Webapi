namespace Webapi.Application.Common.Exceptions;

public class UnauthorizedException(string message) : ApplicationException("Unauthorized", message)
{
}
