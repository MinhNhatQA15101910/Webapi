using MediatR;
using Webapi.Application.Common.Interfaces.Services;

namespace Webapi.Application.AuthCQRS.Notifications.SignupValidated;

public class SignupValidatedHandler(IEmailService emailService) : INotificationHandler<SignupValidatedNotification>
{
    public async Task Handle(SignupValidatedNotification notification, CancellationToken cancellationToken)
    {
        var displayName = notification.Username;
        var email = notification.Email;
        var pincode = notification.Pincode;
        var subject = "ACCOUNT VERIFICATION CODE";
        var message = await File.ReadAllTextAsync("../Webapi.Application/Assets/EmailContent.html", cancellationToken);
        message = message.Replace("{{PINCODE}}", pincode);

        await emailService.SendEmailAsync(displayName, email, subject, message);
    }
}
