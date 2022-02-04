using System.Collections.Concurrent;
using AutoMapper;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Orders.Marketplace.Common.Commands;
using Infrastructure.MediatR.Orders.Marketplace.Common.Queries;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Integration.Common.Services.Orders;
using Integration.SberMegaMarket.Clients;
using Integration.SberMegaMarket.Clients.Orders.Messages;
using Integration.SberMegaMarket.Orders;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Integration.SberMegaMarket.Services.Orders;

public class SberMegaMarketOrderLoader : MarketplaceOrderLoader
{
    private readonly SberMegaMarketDto _sberMegaMarketDto;
    private const int IntervalDays = 7;
    private const int LoadOrdersPortionSize = 100;

    public SberMegaMarketOrderLoader(Marketplace marketplace, IServiceProvider serviceProvider, IMapper mapper,
        DateTime startDate) : base(marketplace, serviceProvider, mapper, startDate)
    {
        _sberMegaMarketDto = Mapper.Map<SberMegaMarketDto>(Marketplace);
    }

    public override async Task LoadOrders()
    {
        if (!_sberMegaMarketDto.Settings.LoadArchiveOrders)
        {
            return;
        }

        string[] numbers = await LoadOrderNumbers();

        var mediatr = ServiceProvider.GetRequiredService<IMediator>();
        var adapter = ServiceProvider.GetRequiredService<ISberMegaMarketOrderAdapter>();

        IEnumerable<string> doesNotExists = await mediatr.Send(new OrdersDoesNotExistsRequest()
        {
            MarketplaceId = _sberMegaMarketDto.Id,
            Numbers = numbers
        });

        UpdateOrderResponseDataShipment[] shipments = await LoadShipments(doesNotExists.ToArray());
        CreateOrdersRequest request = await adapter.CreateArchiveOrdersRequest(shipments, _sberMegaMarketDto);

        await mediatr.Send(request);
    }

    private async Task<UpdateOrderResponseDataShipment[]> LoadShipments(IReadOnlyCollection<string> numbers)
    {
        var result = new ConcurrentBag<UpdateOrderResponseDataShipment>();

        var portions = new List<IEnumerable<string>>();
        var skip = 0;

        for (var i = 0; i < numbers.Count; i += LoadOrdersPortionSize)
        {
            portions.Add(numbers.Skip(skip).Take(LoadOrdersPortionSize));
            skip += LoadOrdersPortionSize;
        }

        await Parallel.ForEachAsync(portions, new ParallelOptions()
        {
            MaxDegreeOfParallelism = 10
        }, async (portion, _) => { await LoadOrderPortion(portion, result); });

        return result.ToArray();
    }

    private async Task LoadOrderPortion(IEnumerable<string> portion,
        ConcurrentBag<UpdateOrderResponseDataShipment> result)
    {
        var client = ServiceProvider.GetRequiredService<ISberMegaMarketClient<UpdateOrdersData>>();
        var request = new SberMegaMarketMessage<UpdateOrdersData>(_sberMegaMarketDto.Settings.Token)
        {
            Data =
            {
                Shipments = portion
            }
        };

        string content;
        try
        {
            content = await client.SendRequest(ApiUrls.GetOrders, _sberMegaMarketDto, request);
        }
        catch (Exception ex)
        {
            var logger = ServiceProvider.GetRequiredService<ILogger<SberMegaMarketOrderLoader>>();
            logger.LogError(ex, "Error while loading sber orders: {Message}", ex.Message);
            return;
        }

        var response = content.FromJson<UpdateOrderResponse>();

        if (response is null || response.Success != 1)
        {
            throw new Exception("Unexpected order update response." +
                                $"{Environment.NewLine}Url: {ApiUrls.GetOrders}" +
                                $"{Environment.NewLine}Request: {request.ToJson()}" +
                                $"{Environment.NewLine}Response: {response}");
        }

        foreach (UpdateOrderResponseDataShipment shipment in response.Data.Shipments)
        {
            result.Add(shipment);
        }
    }

    private async Task<string[]> LoadOrderNumbers()
    {
        var result = new ConcurrentBag<string>();

        DateTime currentDate = new DateTime(StartDate.Year, StartDate.Month, 1, 0, 0, 0).ToUtcTime();
        var intervals = new List<DateTime>();

        while (currentDate < DateTime.UtcNow)
        {
            currentDate = currentDate.AddDays(IntervalDays);
            intervals.Add(currentDate);
        }

        await Parallel.ForEachAsync(intervals, new ParallelOptions()
        {
            MaxDegreeOfParallelism = 10
        }, async (time, _) => { await LoadOrderNumbersByInterval(time, time.AddDays(IntervalDays), result); });

        return result.ToArray();
    }

    private async Task LoadOrderNumbersByInterval(DateTime from, DateTime to, ConcurrentBag<string> numbers)
    {
        var client = ServiceProvider.GetRequiredService<ISberMegaMarketClient<SearchOrdersData>>();
        var request = new SberMegaMarketMessage<SearchOrdersData>(_sberMegaMarketDto.Settings.Token)
        {
            Data =
            {
                DateFrom = from,
                DateTo = to
            }
        };

        string body;
        try
        {
            body = await client.SendRequest(ApiUrls.SearchOrders, _sberMegaMarketDto, request);
        }
        catch (Exception ex)
        {
            var logger = ServiceProvider.GetRequiredService<ILogger<SberMegaMarketOrderLoader>>();
            logger.LogError(ex, "Error while loading sber order numbers: {Message}", ex.Message);
            return;
        }

        var response = body.FromJson<SearchOrdersResponse>();

        if (response is null || response.Success != 1)
        {
            throw new Exception("Unexpected order search response." +
                                $"{Environment.NewLine}Url: {ApiUrls.GetOrders}" +
                                $"{Environment.NewLine}Request: {request.ToJson()}" +
                                $"{Environment.NewLine}Response: {body}");
        }

        foreach (string shipment in response.Data.Shipments)
        {
            numbers.Add(shipment);
        }
    }
}