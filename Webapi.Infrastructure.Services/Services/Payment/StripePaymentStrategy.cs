//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Options;
//using Stripe.Checkout;
//using Webapi.Application.Common.Exceptions;
//using Webapi.Application.Payment.DTOs;
//using Webapi.Domain.Interfaces;
//using Webapi.Infrastructure.Persistence;
//using Webapi.Infrastructure.Services.Configurations;

//namespace Webapi.Infrastructure.Services.Services.Payment;

//public class StripePaymentStrategy(AppDbContext dbContext, IOptions<StripeSettings> config) : IPaymentStrategy
//{
//    public async Task<PaymentResponseDTO> CreatePaymentAsync(Guid orderId, CancellationToken cancellationToken = default)
//    {
//        var order = await dbContext.Orders.Include(x => x.Products)
//            .ThenInclude(x => x.ProductSize)
//            .ThenInclude(x => x.Product)
//            .ThenInclude(x => x.Photos)
//            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken)
//        ?? throw new NotFoundException($"Order {orderId} not found");

//        var options = new SessionCreateOptions
//        {
//            PaymentMethodTypes = new List<string> { "card" },
//            LineItems = new List<SessionLineItemOptions>(),
//            Metadata = new Dictionary<string, string>
//            {
//                { "PaymentId", order.Id.ToString() }
//            },
//            Mode = "payment",
//            SuccessUrl = config.Value.SuccessUrl + "?sessionId={CHECKOUT_SESSION_ID}",
//            CancelUrl = config.Value.CancelUrl + "?sessionId={CHECKOUT_SESSION_ID}",

//        };

//        foreach (var item in order.Products)
//        {
//            options.LineItems.Add(
//                new SessionLineItemOptions
//                {
//                    PriceData = new SessionLineItemPriceDataOptions
//                    {
//                        Currency = "vnd",
//                        UnitAmount = (long)(item.ProductSize.Product.Price * 100),
//                        ProductData = new SessionLineItemPriceDataProductDataOptions
//                        {
//                            Name = item.ProductSize.Product.Name,
//                            Description = item.ProductSize.Product.Description,
//                            Images = item.ProductSize.Product.Photos.Select(x => x.Url).ToList()
//                        }
//                    },
//                    Quantity = item.Quantity
//                }
//            );
//        }

//        var service = new Stripe.Checkout.SessionService();
//        var session = await service.CreateAsync(options, cancellationToken: cancellationToken);

//        var payUrl = session.Url;

//        return new PaymentResponseDTO
//        (
//            orderId,
//            SharedKernel.Enums.PaymentEnum.Stripe,
//            payUrl,
//            0
//        );
//    }
//}
