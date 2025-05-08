using MediatR;

namespace Webapi.Application.AuthCQRS.Notifications.EmailValidated;

public record EmailValidatedNotification(string Email, string Pincode) : INotification;
