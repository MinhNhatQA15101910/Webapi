namespace Webapi.Application.Common.Exceptions;

public class ForbiddenAccessException(string message) : ApplicationException("Forbidden", message)
{
}
