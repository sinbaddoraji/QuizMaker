using TestMaker.Helpers.Interfaces;
using TestMaker.Models;

namespace TestMaker.Helpers.Implementation;

public class SafeJsonSerializer : ISafeJsonSerializer
{
    public string Serialize<T>(T obj)
    {
        try
        {
            return System.Text.Json.JsonSerializer.Serialize(obj);
        }
        catch (Exception e)
        {
            return string.Empty;
        }
    }

    public T? Deserialize<T>(string json)
    {
        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }
        catch (Exception e)
        {
            if(typeof(T) == typeof(List<Question>))
                return (T)(object)new List<Question>();

            return default;
        }
    }
}