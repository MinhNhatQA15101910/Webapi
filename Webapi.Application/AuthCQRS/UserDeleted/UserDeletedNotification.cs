using MediatR;

namespace Webapi.Application.AuthCQRS.UserDeleted;

public record UserDeletedNotification(string UserId) : INotification;
