using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Infrastructure.Extensions;

public static class CommonExtensions
{
    public static string ToJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }
}