namespace LightNap.Core.Identity.Models
{
    /// <summary>
    /// Indicates the specific type of login field provided in a login attempt.
    /// </summary>
    public enum LoginType
    {
        /// <summary>
        /// The login type is unknown and all fields should be tried until a record is found.
        /// </summary>
        Unknown,

        /// <summary>
        /// The login is an email.
        /// </summary>
        Email,

        /// <summary>
        /// The login is a username.
        /// </summary>
        UserName
    }
}
