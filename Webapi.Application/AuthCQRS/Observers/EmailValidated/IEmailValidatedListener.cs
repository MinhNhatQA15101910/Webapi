namespace Webapi.Application.AuthCQRS.Observers.EmailValidated;

public interface IEmailValidatedListener
{
    Task UpdateAsync(string email, string pincode, CancellationToken cancellationToken = default);
}
