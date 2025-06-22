using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;

namespace Webapi.Application.OrdersCQRS.Observers.OrderCreated;

public class SendEmailOrderCreatedListener(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IEmailService emailService
) : IOrderCreatedListener
{
    public async Task UpdateAsync(Order order, List<CartItem> cartItems, List<Voucher> vouchers, CancellationToken cancellationToken = default)
    {
        var httpContext = httpContextAccessor.HttpContext
            ?? throw new UnauthorizedAccessException("No HttpContext found.");

        var userId = httpContext.User.GetUserId();
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(userId, cancellationToken)
            ?? throw new UnauthorizedAccessException("User not found.");

        var displayName = user.Email;
        var email = user.Email;
        var subject = $"We've Received Your Order – {order.Id}";
        var message = await File.ReadAllTextAsync("../Webapi.Application/Assets/OrderCreatedEmail.html", cancellationToken);
        message = message.Replace("{{UserName}}", user.UserName);
        message = message.Replace("{{OrderNumber}}", order.Id.ToString());
        message = message.Replace("{{OrderDate}}", order.CreatedAt.ToString("MMMM dd, yyyy"));
        message = message.Replace("{{TotalAmount}}", order.TotalPrice.ToString("C", System.Globalization.CultureInfo.CurrentCulture));

        await emailService.SendEmailAsync(displayName!, email!, subject, message);
    }
}
