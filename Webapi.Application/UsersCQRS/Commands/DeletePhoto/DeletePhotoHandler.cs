using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Domain.Interfaces;

namespace Webapi.Application.UsersCQRS.Commands.DeletePhoto;

public class DeletePhotoHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IFileService fileService
) : ICommandHandler<DeletePhotoCommand, bool>
{
    public async Task<bool> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext.User.GetUserId();

        var user = await unitOfWork.UserRepository.GetUserByIdAsync(userId, cancellationToken)
            ?? throw new UserNotFoundException(userId);

        var photo = user.Photos.FirstOrDefault(p => p.Id == request.PhotoId);

        if (photo == null || photo.IsMain) throw new BadRequestException("This photo cannot be deleted");

        if (photo.PublicId != null)
        {
            var result = await fileService.DeleteFileAsync(photo.PublicId, ResourceType.Image);
            if (result.Error != null) throw new BadRequestException(result.Error.Message);
        }

        user.Photos.Remove(photo);

        if (await unitOfWork.CompleteAsync()) return true;

        throw new BadRequestException("Problem deleting photo");
    }
}
