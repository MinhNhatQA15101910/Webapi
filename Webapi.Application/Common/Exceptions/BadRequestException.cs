namespace Webapi.Application.Common.Exceptions;

public class BadRequestException(string message) : ApplicationException("Bad Request", message)
{
}
