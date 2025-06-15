using MediatR;
using Webapi.Domain.Interfaces;

namespace Webapi.Application.AuthCQRS.Notifications.UserDeleted;

public class UserDeletedHandler(ICacheService cacheService) : INotificationHandler<UserDeletedNotification>
{
    public Task Handle(UserDeletedNotification notification, CancellationToken cancellationToken)
    {
        var cacheKeyForList = "Users_?";
        cacheService.RemoveKeysStartingWith(cacheKeyForList);

        var cacheKeyForSingle = $"Users_/{notification.UserId}";
        cacheService.Remove(cacheKeyForSingle);

        return Task.CompletedTask;
    }
}
