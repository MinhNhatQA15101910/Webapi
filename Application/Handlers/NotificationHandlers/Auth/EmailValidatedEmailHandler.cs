using Application.Notifications.Auth;
using Domain.Services;
using MediatR;

namespace Application.Handlers.NotificationHandlers.Auth;

public class EmailValidatedEmailHandler(IEmailService emailService) : INotificationHandler<EmailValidatedNotification>
{
    public async Task Handle(EmailValidatedNotification notification, CancellationToken cancellationToken)
    {
        var displayName = notification.Email;
        var email = notification.Email;
        var subject = "ACCOUNT VERIFICATION CODE";
        var message = await File.ReadAllTextAsync("../Application/Assets/EmailContent.html", cancellationToken);
        message = message.Replace("{{PINCODE}}", notification.Pincode);

        await emailService.SendEmailAsync(displayName, email, subject, message);
    }
}
