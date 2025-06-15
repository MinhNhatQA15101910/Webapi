using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Webapi.Application.AuthCQRS.Observers.EmailValidated;
using Webapi.Application.Common.Helpers;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Domain.Entities;

namespace Webapi.Application.AuthCQRS.Commands.ValidateEmail;

public class ValidateEmailHandler : ICommandHandler<ValidateEmailCommand, object>
{
    private readonly UserManager<User> _userManager;
    private readonly PincodeStore _pincodeStore;
    private readonly ITokenService _tokenService;

    private readonly List<IEmailValidatedListener> _listeners = [];

    public ValidateEmailHandler(
        UserManager<User> userManager,
        PincodeStore pincodeStore,
        ITokenService tokenService,
        IEmailService emailService
    )
    {
        _userManager = userManager;
        _pincodeStore = pincodeStore;
        _tokenService = tokenService;

        Subscribe(new EmailValidatedListener(emailService));
    }

    public async Task<object> Handle(ValidateEmailCommand request, CancellationToken cancellationToken)
    {
        if (!await UserExists(request.ValidateEmailDto.Email, cancellationToken))
        {
            return false;
        }

        // Add to pincode map
        var pincode = PincodeStore.GeneratePincode();
        _pincodeStore.AddPincode(request.ValidateEmailDto.Email, pincode);

        // Send pincode email
        await NotifyListenersAsync(request.ValidateEmailDto.Email, pincode, cancellationToken);

        return _tokenService.CreateVerifyPincodeToken(request.ValidateEmailDto.Email, PincodeAction.VerifyEmail.ToString());
    }

    private async Task<bool> UserExists(string email, CancellationToken cancellationToken)
    {
        return await _userManager.Users.AnyAsync(x => x.NormalizedEmail == email.ToUpper(), cancellationToken: cancellationToken);
    }

    private void Subscribe(IEmailValidatedListener listener)
    {
        if (listener is null)
        {
            throw new ArgumentNullException(nameof(listener));
        }

        _listeners.Add(listener);
    }
    
    private async Task NotifyListenersAsync(string email, string pincode, CancellationToken cancellationToken)
    {
        foreach (var listener in _listeners)
        {
            await listener.UpdateAsync(email, pincode, cancellationToken);
        }
    }
}
