using MediatR;
using Webapi.Domain.Interfaces;

namespace Webapi.Application.AuthCQRS.Notifications.UserAdded;

public class UserAddedHandler(ICacheService cacheService) : INotificationHandler<UserAddedNotification>
{
    public Task Handle(UserAddedNotification notification, CancellationToken cancellationToken)
    {
        // Invalidate the cache for the user collection.
        var cacheKey = "Users_?";
        cacheService.RemoveKeysStartingWith(cacheKey);

        // Optionally, you can log or perform additional actions here.
        return Task.CompletedTask;
    }
}
