using MediatR;

namespace Webapi.Application.AuthCQRS.Notifications.SignupValidated;

public record SignupValidatedNotification(string Username, string Email, string Pincode) : INotification;
