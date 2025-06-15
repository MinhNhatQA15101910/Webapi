using MediatR;

namespace Webapi.Application.AuthCQRS.Notifications.UserDeleted;

public record UserDeletedNotification(string UserId) : INotification;
