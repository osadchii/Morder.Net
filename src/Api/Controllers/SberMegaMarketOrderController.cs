using Infrastructure.Extensions;
using Infrastructure.MediatR.Orders.Marketplace.Common.Commands;
using Integration.SberMegaMarket.Orders;
using Integration.SberMegaMarket.Orders.Messages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/v1/sbermegamarketorder")]
public class SberMegaMarketOrderController : ControllerBase
{
    private readonly ISberMegaMarketOrderAdapter _adapter;
    private readonly IMediator _mediator;
    private readonly ILogger<SberMegaMarketOrderController> _logger;

    public SberMegaMarketOrderController(ISberMegaMarketOrderAdapter adapter, IMediator mediator,
        ILogger<SberMegaMarketOrderController> logger)
    {
        _adapter = adapter;
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [Route("create")]
    public async Task<SberMegaMarketResponse> CreateOrder(
        BaseSberMegaMarketOrderRequest<CreateSberMegaMarketOrdersRequest> request)
    {
        _logger.LogInformation("SberMegaMarket Create order request: {RequestJson}", request.ToJson());
        IEnumerable<CreateOrderRequest> createRequests = await _adapter.CreateOrderRequests(request);

        foreach (CreateOrderRequest createRequest in createRequests)
        {
            try
            {
                await _mediator.Send(createRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "SberMegaMarketOrder error");
                return new SberMegaMarketResponse(request.Meta, e.Message);
            }
        }

        return new SberMegaMarketResponse(request.Meta);
    }

    [HttpPost]
    [Route("cancel")]
    public async Task<SberMegaMarketResponse> CancelOrder(
        BaseSberMegaMarketOrderRequest<CancelSberMegaMarketOrdersRequest> request)
    {
        _logger.LogInformation("SberMegaMarket Cancel order request: {RequestJson}", request.ToJson());
        IEnumerable<CancelOrderItemsByExternalIdRequest> cancelRequests = await _adapter.CancelOrderRequests(request);

        foreach (CancelOrderItemsByExternalIdRequest cancelRequest in cancelRequests)
        {
            try
            {
                await _mediator.Send(cancelRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "SberMegaMarketOrder error");
                return new SberMegaMarketResponse(request.Meta, e.Message);
            }
        }

        return new SberMegaMarketResponse(request.Meta);
    }
}