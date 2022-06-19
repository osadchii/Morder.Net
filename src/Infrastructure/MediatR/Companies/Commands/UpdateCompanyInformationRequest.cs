using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using MediatR;

namespace Infrastructure.MediatR.Companies.Commands;

public class UpdateCompanyInformationRequest : IRequest
{
    [MaxLength(Limits.CompanyName)] public string Name { get; set; } = null!;

    [MaxLength(Limits.ShopName)] public string Shop { get; set; } = null!;

    [MaxLength(Limits.ShopUrl)] public string Url { get; set; } = null!;

    [Required] public Guid? PriceTypeExternalId { get; set; }

    public int? PriceTypeId { get; set; }
}