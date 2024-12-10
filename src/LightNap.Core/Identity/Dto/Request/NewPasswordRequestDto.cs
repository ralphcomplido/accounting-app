using LightNap.Core.Configuration;
using System.ComponentModel.DataAnnotations;

namespace LightNap.Core.Identity.Dto.Request
{
    /// <summary>
    /// Represents the data transfer object for a new password request.
    /// </summary>
    public class NewPasswordRequestDto
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [EmailAddress]
        [Required]
        [StringLength(Constants.Dto.MaxLoginLength)]
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the password reset token.
        /// </summary>
        [Required]
        [StringLength(Constants.Dto.MaxPasswordResetTokenLength)]
        public required string Token { get; set; }

        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        [Required]
        [StringLength(Constants.Dto.MaxPasswordLength)]
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to remember the user.
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// Gets or sets the device details.
        /// </summary>
        [Required]
        [StringLength(Constants.Dto.MaxDeviceDetailsLength)]
        public required string DeviceDetails { get; set; }
    }
}
