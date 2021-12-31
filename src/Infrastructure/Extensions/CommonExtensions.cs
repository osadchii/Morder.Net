using Newtonsoft.Json;

namespace Infrastructure.Extensions;

public static class CommonExtensions
{
    public static string ToJson(this object obj)
    {
        var settings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        return JsonConvert.SerializeObject(obj, settings);
    }

    public static T? FromJson<T>(this string s)
    {
        try
        {
            var result = JsonConvert.DeserializeObject<T>(s);
            return result ?? default;
        }
        catch
        {
            return default;
        }
    }

    public static bool IsNullOrEmpty(this string? value)
    {
        return string.IsNullOrEmpty(value);
    }
}