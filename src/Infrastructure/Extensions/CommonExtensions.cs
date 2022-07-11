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

    public static T FromJson<T>(this string s)
    {
        try
        {
            var result = JsonConvert.DeserializeObject<T>(s);
            return result ?? default;
        }
        catch
        {
#if DEBUG
            throw;
#endif
#pragma warning disable CS0162
            return default;
#pragma warning restore CS0162
        }
    }

    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    public static DateTime ToMoscowTime(this DateTime dateTime)
    {
        return TimeZoneInfo.ConvertTime(dateTime,
            TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
    }

    public static DateTime ToCommonTime(this DateTime dateTime)
    {
        return TimeZoneInfo.ConvertTime(dateTime,
            TimeZoneInfo.Utc);
    }

    public static DateTime ToUtcTime(this DateTime dateTime)
    {
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }
}