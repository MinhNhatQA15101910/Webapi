using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;

namespace Webapi.Application.UsersCQRS.Commands.SetMainPhoto;

public class SetMainPhotoHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork
) : ICommandHandler<SetMainPhotoCommand, bool>
{
    public async Task<bool> Handle(SetMainPhotoCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext.User.GetUserId();

        var user = await unitOfWork.UserRepository.GetUserByIdAsync(userId, cancellationToken)
            ?? throw new UserNotFoundException(userId);

        var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);

        if (photo == null || photo.IsMain) throw new BadRequestException("Cannot use this as main photo");

        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if (currentMain != null) currentMain.IsMain = false;

        photo.IsMain = true;

        if (await unitOfWork.CompleteAsync()) return true;

        throw new BadRequestException("Problem setting main photo");
    }
}
