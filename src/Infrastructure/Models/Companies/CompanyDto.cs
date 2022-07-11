using Infrastructure.Models.Prices;

namespace Infrastructure.Models.Companies;

public class CompanyDto
{
    public string Name { get; set; }

    public string Shop { get; set; }

    public string Url { get; set; }

    public int? PriceTypeId { get; set; }

    public PriceType PriceType { get; set; }
}