using Infrastructure.Models.Marketplaces;
using MediatR;

namespace Infrastructure.MediatR.Marketplaces.Common.Commands;

public class CreateMarketplaceOrderTaskRequest : IRequest<Unit>
{
    public int MarketplaceId { get; set; }
    public int OrderId { get; set; }
    public TaskType Type { get; set; }
    public string TaskContext { get; set; } = null!;
}