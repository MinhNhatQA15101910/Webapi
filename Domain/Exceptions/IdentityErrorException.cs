using Microsoft.AspNetCore.Identity;

namespace Domain.Exceptions;

public class IdentityErrorException(IEnumerable<IdentityError> errors) : BadRequestException(string.Join(", ", errors.Select(e => e.Description)))
{
}
