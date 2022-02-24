using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using MediatR;

namespace Infrastructure.MediatR.Stocks.Queries;

public class GetAllStocksRequest : IRequest<Result>
{
    [Required] public Guid? WarehouseExternalId { get; set; }
}