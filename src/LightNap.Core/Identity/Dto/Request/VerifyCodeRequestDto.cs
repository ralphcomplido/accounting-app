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
        /// The email address or user name of the user.
        /// </summary>
        [Required]
        [StringLength(Constants.Dto.MaxLoginLength)]
        public required string Login { get; set; }

        /// <summary>
        /// The verification code.
        /// </summary>
        [Required]
        [StringLength(Constants.Dto.MaxVerificationCodeLength)]
        public required string Code { get; set; }

        /// <summary>
        /// True to remember the user on this device.
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// The details of the device.
        /// </summary>
        [Required]
        [StringLength(Constants.Dto.MaxDeviceDetailsLength)]
        public required string DeviceDetails { get; set; }
    }
}