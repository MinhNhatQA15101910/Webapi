using Webapi.Application.Common.Interfaces.Services;

namespace Webapi.Application.AuthCQRS.Observers.EmailValidated;

public class EmailValidatedListener(IEmailService emailService) : IEmailValidatedListener
{
    public async Task UpdateAsync(string email, string pincode, CancellationToken cancellationToken = default)
    {
        var displayName = email;
        var subject = "ACCOUNT VERIFICATION CODE";
        var message = await File.ReadAllTextAsync("../Webapi.Application/Assets/EmailContent.html", cancellationToken);
        message = message.Replace("{{PINCODE}}", pincode);

        await emailService.SendEmailAsync(displayName, email, subject, message);
    }
}
