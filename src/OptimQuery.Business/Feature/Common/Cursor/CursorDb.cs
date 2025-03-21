using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore.WebUtilities;

namespace OptimQuery.Business.Feature.Common.Cursor;

public sealed record CursorDb(long LastId)
{
    public static string Encode(long lastId)
    {
        var cursor = new CursorDb(lastId);
        var json = JsonSerializer.Serialize(cursor);
        return Base64UrlTextEncoder.Encode(Encoding.UTF8.GetBytes(json));
    }

    public static CursorDb? Decode(string? cursor)
    {
        if (string.IsNullOrWhiteSpace(cursor)) return null;

        try
        {
            var json = Encoding.UTF8.GetString(Base64UrlTextEncoder.Decode(cursor));
            return JsonSerializer.Deserialize<CursorDb>(json);
        }
        catch
        {
            return null;
        }
    }
}