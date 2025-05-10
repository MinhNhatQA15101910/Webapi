using Webapi.Application.Common.Interfaces.MediatR;

namespace Webapi.Application.AuthCQRS.Commands.VerifyPincode;

public record VerifyPincodeCommand(VerifyPincodeDto VerifyPincodeDto) : ICommand<object>;
