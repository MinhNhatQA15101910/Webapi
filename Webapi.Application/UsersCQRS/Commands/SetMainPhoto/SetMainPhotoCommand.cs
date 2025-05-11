using Webapi.Application.Common.Interfaces.MediatR;

namespace Webapi.Application.UsersCQRS.Commands.SetMainPhoto;

public record SetMainPhotoCommand(Guid PhotoId) : ICommand<bool>;
