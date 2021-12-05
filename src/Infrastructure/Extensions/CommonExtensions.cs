using Newtonsoft.Json;

namespace Infrastructure.Extensions;

public static class CommonExtensions
{
    public static string ToJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj);
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

    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }
}