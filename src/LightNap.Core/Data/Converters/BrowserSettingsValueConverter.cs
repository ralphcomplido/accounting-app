using LightNap.Core.Profile.Dto.Response;
using LightNap.Core.Profile.Helpers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace LightNap.Core.Data.Converters
{
    internal class BrowserSettingsValueConverter : ValueConverter<BrowserSettingsDto, string>
    {
        public BrowserSettingsValueConverter() : base(
            settings => JsonSerializer.Serialize(settings, null as JsonSerializerOptions),
            json => BrowserSettingsMigration.Migrate(JsonSerializer.Deserialize<BrowserSettingsDto>(json, null as JsonSerializerOptions) ?? new BrowserSettingsDto()))
        {
        }
    }

}
