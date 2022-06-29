using Infrastructure.Models.Prices;

namespace Infrastructure.Models.Companies;

public class CompanyDto
{
    public string Name { get; set; } = null!;

    public string Shop { get; set; } = null!;

    public string Url { get; set; } = null!;

    public int? PriceTypeId { get; set; }

    public PriceType? PriceType { get; set; }
}