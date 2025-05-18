using LightNap.Core.Profile.Dto.Response;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace LightNap.Core.Data.Comparers
{
    public class BrowserSettingsValueComparer : ValueComparer<BrowserSettingsDto>
    {
        public BrowserSettingsValueComparer()
            : base(
                (a, b) => JsonSerializer.Serialize(a, (JsonSerializerOptions?)null) == JsonSerializer.Serialize(b, (JsonSerializerOptions?)null),
                obj => obj == null ? 0 : JsonSerializer.Serialize(obj, (JsonSerializerOptions?)null).GetHashCode(),
                obj => (obj == null ? null : JsonSerializer.Deserialize<BrowserSettingsDto>(JsonSerializer.Serialize(obj, (JsonSerializerOptions?)null), (JsonSerializerOptions?)null))!
            )
        { }
    }
}