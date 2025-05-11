using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.UsersCQRS.Commands.AddPhoto;

public class AddPhotoHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IFileService fileService,
    IMapper mapper
) : ICommandHandler<AddPhotoCommand, PhotoDto>
{
    public async Task<PhotoDto> Handle(AddPhotoCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = httpContextAccessor.HttpContext.User.GetUserId();

        var user = await unitOfWork.UserRepository.GetUserByIdAsync(currentUserId, cancellationToken)
            ?? throw new UserNotFoundException(currentUserId);

        var result = await fileService.UploadPhotoAsync($"users/{currentUserId}", request.File);
        if (result.Error != null) throw new BadRequestException(result.Error.Message);

        var photo = new UserPhoto
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            IsMain = user.Photos.Count == 0
        };
        user.Photos.Add(photo);

        if (await unitOfWork.CompleteAsync())
        {
            return mapper.Map<PhotoDto>(photo);
        }

        throw new BadRequestException("Problem adding photo");
    }
}
