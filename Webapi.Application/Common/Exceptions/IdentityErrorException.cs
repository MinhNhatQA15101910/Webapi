using Microsoft.AspNetCore.Identity;

namespace Webapi.Application.Common.Exceptions;

public class IdentityErrorException(IEnumerable<IdentityError> errors) : BadRequestException(string.Join("\n", errors.Select(e => e.Description)))
{
}
