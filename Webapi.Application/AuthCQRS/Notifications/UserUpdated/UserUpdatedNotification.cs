using MediatR;
using Webapi.Domain.Entities;

namespace Webapi.Application.AuthCQRS.Notifications.UserUpdated;

public record UserUpdatedNotification(User User) : INotification;
