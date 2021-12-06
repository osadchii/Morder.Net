using Infrastructure.Models.Warehouses;
using MediatR;

namespace Infrastructure.MediatR.Warehouses.Queries;

public class GetWarehouseByExternalIdRequest : IRequest<WarehouseDto>
{
    public Guid ExternalId { get; set; }
}