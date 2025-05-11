using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.UsersCQRS.Commands.AddPhoto;

public record AddPhotoCommand(IFormFile File) : ICommand<PhotoDto>;
