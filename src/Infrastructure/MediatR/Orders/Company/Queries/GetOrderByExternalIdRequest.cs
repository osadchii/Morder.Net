using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using MediatR;

namespace Infrastructure.MediatR.Orders.Company.Queries;

public class GetOrderByExternalIdRequest : IRequest<Result>
{
    [Required] public Guid? ExternalId { get; set; }
}