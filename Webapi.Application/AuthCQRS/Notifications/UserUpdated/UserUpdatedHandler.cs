using MediatR;
using Webapi.Domain.Interfaces;

namespace Webapi.Application.AuthCQRS.Notifications.UserUpdated;

public class UserUpdatedHandler(
    ICacheService cacheService
) : INotificationHandler<UserUpdatedNotification>
{
    public Task Handle(UserUpdatedNotification notification, CancellationToken cancellationToken)
    {
        var cacheKeyForList = "Users_?";
        cacheService.RemoveKeysStartingWith(cacheKeyForList);

        var cacheKeyForSingle = $"Users_/{notification.User.Id}";
        cacheService.Set(cacheKeyForSingle, notification.User);

        return Task.CompletedTask;
    }
}
