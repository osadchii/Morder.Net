using Infrastructure.Models.Products;

namespace Integration.Kuper.Extensions;

public static class CommonExtensions
{
    public static string ConvertToString(this Vat? vat)
    {
        if (!vat.HasValue)
        {
            return "NO_VAT";
        }

        return vat.Value.ConvertToString();
    }

    public static string ConvertToString(this Vat vat)
    {
        return vat switch
        {
            Vat.Vat_0 => "VAT0",
            Vat.Vat_10 => "VAT10",
            Vat.Vat_20 => "VAT20",
            _ => "NO_VAT"
        };
    }
}