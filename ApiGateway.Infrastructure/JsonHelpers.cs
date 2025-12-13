using System.Text;
using System.Text.Json;

namespace ApiGateway.Infrastructure;

internal static class JsonHelpers
{
    public static bool TryExtractGuid(string json, string propertyName, out Guid value)
    {
        value = Guid.Empty;

        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.ValueKind != JsonValueKind.Object)
                return false;

            if (!doc.RootElement.TryGetProperty(propertyName, out var prop))
                return false;

            if (prop.ValueKind == JsonValueKind.String)
                return Guid.TryParse(prop.GetString(), out value);

            return false;
        }
        catch
        {
            return false;
        }
    }

    public static string TryDecodeText(byte[] bytes)
    {
        if (bytes.Length == 0) return string.Empty;

        try { return Encoding.UTF8.GetString(bytes); } catch { }
        try { return Encoding.Unicode.GetString(bytes); } catch { }
        try { return Encoding.GetEncoding(1251).GetString(bytes); } catch { }

        return string.Empty;
    }
}