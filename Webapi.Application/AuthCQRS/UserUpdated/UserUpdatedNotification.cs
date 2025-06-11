using MediatR;
using Webapi.Domain.Entities;

namespace Webapi.Application.AuthCQRS.UserUpdated;

public record UserUpdatedNotification(User User) : INotification;
