using LightNap.Core.Configuration;
using System.ComponentModel.DataAnnotations;

namespace LightNap.Core.Identity.Dto.Request
{
    /// <summary>
    /// Data transfer object for verifying a 2FA code.
    /// </summary>
    public class VerifyCodeRequestDto
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [EmailAddress]
        [Required]
        [StringLength(Constants.Dto.MaxLoginLength)]
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the verification code.
        /// </summary>
        [Required]
        [StringLength(Constants.Dto.Max2FaCodeLength)]
        public required string Code { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to remember the user on this device.
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// Gets or sets the details of the device.
        /// </summary>
        [Required]
        [StringLength(Constants.Dto.MaxDeviceDetailsLength)]
        public required string DeviceDetails { get; set; }
    }
}