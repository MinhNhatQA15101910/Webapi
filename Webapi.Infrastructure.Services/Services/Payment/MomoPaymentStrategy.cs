using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Models;
using Webapi.Application.Common.Utils.Momo;
using Webapi.Application.OrdersCQRS.Commands.CancelOrder;
using Webapi.Application.OrdersCQRS.Commands.ProceedOrder;
using Webapi.Application.Payment.DTOs;
using Webapi.Domain.Enums;
using Webapi.Domain.Interfaces;
using Webapi.Domain.Interfaces.States;
using Webapi.Infrastructure.Persistence;
using Webapi.Infrastructure.Services.Configurations;

namespace Webapi.Infrastructure.Services.Services.Payment;

public class MomoPaymentStrategy(
    AppDbContext dbContext,
    IOptions<MomoSettings> config,
    IMediator mediator
) : IPaymentStrategy
{
    private readonly HttpClient _httpClient = new HttpClient();

    public async Task<PaymentResponseDTO> CreatePaymentAsync(Guid _orderId, CancellationToken cancellationToken = default)
    {
        var order = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == _orderId, cancellationToken) ?? throw new NotFoundException($"Order {_orderId} not found");

        var url = config.Value.BaseUrl;

        var accessKey = config.Value.AccessKey;
        var secretKey = config.Value.SecretKey;

        // Data for rawSignature
        var amount = order.TotalPrice;
        var orderId = order.Id;
        var orderInfo = order.Id.ToString();
        var requestId = order.Id;

        var storeId = config.Value.StoreId;
        var ipnUrl = config.Value.IpnUrl;
        var extraData = config.Value.ExtraData;
        var partnerCode = config.Value.PartnerCode;
        var redirectUrl = config.Value.RedirectUrl;
        var requestType = config.Value.RequestType;
        var lang = config.Value.Lang;

        var payload = new MomoCreatePaymenyRequestDTO
        {
            PartnerCode = partnerCode,
            StoreId = storeId,
            RequestId = requestId.ToString(),
            Amount = (long)(amount * 100),
            OrderId = orderId.ToString(),
            OrderInfo = orderInfo,
            RedirectUrl = redirectUrl,
            IpnUrl = ipnUrl,
            RequestType = requestType,
            ExtraData = extraData,
            Lang = lang,
        };

        var payloadString = MomoUtils.GenerateStringPayload(payload);

        payload.Signature = MomoUtils.GetSignature(payloadString, secretKey);


        // Chuyển payload thành JSON
        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Gửi yêu cầu POST
        var response = await _httpClient.PostAsync(url, content, cancellationToken);

        // Kiểm tra trạng thái phản hồi
        if (response.IsSuccessStatusCode)
        {
            // Đọc kết quả phản hồi
            var responseContent = await response.Content.ReadAsStringAsync();
            var momoResponse = JsonSerializer.Deserialize<MomoCreatePaymentResponseDTO>(responseContent)
                ?? throw new Application.Common.Exceptions.ApplicationException(
                        "Lỗi hệ thống",
                        "Có lỗi xảy ra khi đọc dữ liệu trả về từ Momo"
                );

            return new PaymentResponseDTO(orderId, SharedKernel.Enums.PaymentMethodEnum.MoMo, momoResponse.PayUrl, momoResponse.ResultCode);
        }

        throw new Application.Common.Exceptions.ApplicationException(
            "Lỗi hệ thống",
            "Có lỗi xảy ra khi sinh PaymentUrl"
        );
    }

    public async Task<IpnResponse> IpnConfirmAsync(object dto, IQueryCollection query, CancellationToken cancellationToken = default)
    {
        var request = dto as MomoCreatePaymentResponseDTO;

        var oderId = request.OrderId;

        var order = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id.ToString() == oderId, cancellationToken) ?? throw new NotFoundException($"Order {oderId} not found");

        var orderContext = new OrderContext(order);


        if (request.ResultCode != 0)
        {
            await mediator.Send(new CancelOrderCommand(order.Id), cancellationToken);

            return new IpnResponse("00", "Confirm success");
        }

        await mediator.Send(new ProceedOrderCommand(order.Id), cancellationToken);

        return new IpnResponse("00", "Confirm success");
    }
}