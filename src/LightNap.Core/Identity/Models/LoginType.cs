namespace LightNap.Core.Identity.Models
{
    /// <summary>
    /// Indicates the specific type of login field provided in a login attempt.
    /// </summary>
    public enum LoginType
    {
        /// <summary>
        /// The login type is unknown then email will be tried, followed by username.
        /// </summary>
        Unknown,

        /// <summary>
        /// The login is an email.
        /// </summary>
        Email,

        /// <summary>
        /// The login is a username.
        /// </summary>
        UserName,

        /// <summary>
        /// The login is an email but the password is a MagicLink token.
        /// </summary>
        MagicLink
    }
}
