namespace Webapi.Application.Common.Exceptions;

public class UserNotFoundException(Guid userId)
    : NotFoundException($"The user with the identifier {userId} was not found.")
{
}
