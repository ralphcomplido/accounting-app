using LightNap.Core.Configuration;
using LightNap.Core.Identity.Models;
using System.ComponentModel.DataAnnotations;

namespace LightNap.Core.Identity.Dto.Request
{
    /// <summary>
    /// Represents the login request data transfer object.
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// Specifies the type of login. Uses LoginType.Unknown if not specified.
        /// <seealso cref="LightNap.Core.Identity.Models.LoginType"/>
        /// </summary>
        public LoginType? Type { get; set; }

        /// <summary>
        /// The login identifier.
        /// </summary>
        [Required]
        [StringLength(Constants.Dto.MaxLoginLength)]
        public required string Login { get; set; }

        /// <summary>
        /// Gets or sets the password.
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
