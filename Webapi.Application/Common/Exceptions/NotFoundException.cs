namespace Webapi.Application.Common.Exceptions;

public class NotFoundException(string message) : ApplicationException("Not Found", message)
{
}
