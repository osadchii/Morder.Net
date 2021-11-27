using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using MediatR;

namespace Infrastructure.MediatR.Companies.Commands;

public class UpdateCompanyInformationRequest : IRequest
{
    [MaxLength(Limits.CompanyName)] public string Name { get; set; }

    [MaxLength(Limits.ShopName)] public string Shop { get; set; }

    [MaxLength(Limits.ShopUrl)] public string Url { get; set; }
}