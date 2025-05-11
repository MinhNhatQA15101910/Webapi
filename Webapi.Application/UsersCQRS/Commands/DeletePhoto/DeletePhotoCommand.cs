using Webapi.Application.Common.Interfaces.MediatR;

namespace Webapi.Application.UsersCQRS.Commands.DeletePhoto;

public record DeletePhotoCommand(Guid PhotoId) : ICommand<bool>;
