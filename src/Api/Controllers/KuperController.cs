using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.MediatR.Marketplaces.Kuper.Commands;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Kuper;
using Integration.Kuper.Clients.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/kuper")]
public class KuperController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IKuperGetOrdersClient _kuperGetOrdersClient;
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public KuperController(IMediator mediator, MContext context, IMapper mapper,
        IKuperGetOrdersClient kuperGetOrdersClient)
    {
        _mediator = mediator;
        _context = context;
        _mapper = mapper;
        _kuperGetOrdersClient = kuperGetOrdersClient;
    }

    [HttpPost]
    public async Task<Result> CreateMeso([Required] [FromBody] UpdateKuperRequest command)
    {
        command.Id = null;
        var result = await _mediator.Send(command);
        return result.AsResult();
    }

    [HttpPost]
    [Route("{id:int}")]
    public async Task<Result> UpdateMeso([Required] [FromBody] UpdateKuperRequest command,
        [Required] int id)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return result.AsResult();
    }

    [HttpPost]
    [Route("runtest")]
    public async Task<Result> RunTest()
    {
        var marketplace = await _context.Marketplaces
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Type == MarketplaceType.Kuper);

        if (marketplace is null)
        {
            return new Result(ResultCode.Error).AddError("Kuper marketplace not found");
        }

        var kuper = _mapper.Map<KuperDto>(marketplace);

        using var httpClient = new HttpClient();

        var token = await _kuperGetOrdersClient.GetToken(kuper, httpClient);


        await Delay();
        var result = await _kuperGetOrdersClient.GetOrdersWIthPagination(kuper, 1, token: token);
        await Delay();
        var nextPageResult = await _kuperGetOrdersClient.GetOrdersWIthPagination(kuper, nextPageToken: result.NextPageToken, token: token);

        return result.AsResult();

        Task Delay() => Task.Delay(TimeSpan.FromSeconds(1));
    }
}