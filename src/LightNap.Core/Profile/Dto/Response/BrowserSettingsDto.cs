namespace LightNap.Core.Profile.Dto.Response
{
    /// <summary>
    /// Data transfer object representing the user's settings for the browser front-end app.
    /// </summary>
    public class BrowserSettingsDto
    {
        /// <summary>
        /// The version of this setting to support migrations if necessary.
        /// </summary>
        public int Version { get; set; } = 1;

        /// <summary>
        /// Style data like the theme, menu preferences, etc.
        /// </summary>
        public Dictionary<string, object> Style { get; set; } = [];

        /// <summary>
        /// Preferences like the language, timezone, etc.
        /// </summary>
        public Dictionary<string, object> Preferences { get; set; } = [];

        /// <summary>
        /// Features like feature flags, etc.
        /// </summary>
        public Dictionary<string, object> Features { get; set; } = [];

        /// <summary>
        /// Contains extended settings that might be less commonly set and/or temporary.
        /// </summary>
        public Dictionary<string, object> Extended { get; set; } = [];

        public BrowserSettingsDto()
        {
            this.Style["darkTheme"] = false;
            this.Style["menuMode"] = "static";
            this.Style["preset"] = "Aura";
            this.Style["primary"] = "emerald";
        }
    }
}