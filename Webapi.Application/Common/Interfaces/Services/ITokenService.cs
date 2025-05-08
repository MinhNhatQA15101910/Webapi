using Webapi.Domain.Entities;

namespace Webapi.Application.Common.Interfaces.Services;

public interface ITokenService
{
    Task<string> CreateTokenAsync(User user);
    string CreateVerifyPincodeToken(string email, string action);
}
