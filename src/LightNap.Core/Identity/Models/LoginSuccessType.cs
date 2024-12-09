namespace LightNap.Core.Identity.Models
{
    /// <summary>
    /// Represents the result of a successful login operation.
    /// </summary>
    public enum LoginSuccessType
    {
        /// <summary>
        /// Indicates the response contains an access token.
        /// </summary>
        AccessToken,

        /// <summary>
        /// Indicates the user must verify their email address before they can get an access token.
        /// </summary>
        EmailVerificationRequired,

        /// <summary>
        /// Indicates the user must submit the 2FA code before they can get an access token.
        /// </summary>
        TwoFactorRequired
    }
}
