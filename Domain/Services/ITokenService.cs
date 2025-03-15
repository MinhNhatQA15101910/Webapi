using Domain.Entities;

namespace Domain.Services;

public interface ITokenService
{
    Task<string> CreateTokenAsync(User user);
    string CreateVerifyPincodeToken(string email, string action);
}
