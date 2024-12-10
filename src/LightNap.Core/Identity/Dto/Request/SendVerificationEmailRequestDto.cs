using LightNap.Core.Configuration;
using System.ComponentModel.DataAnnotations;

namespace LightNap.Core.Identity.Dto.Request
{
    /// <summary>
    /// Data transfer object for requesting a verification email.
    /// </summary>
    public class SendVerificationEmailRequestDto
    {
        /// <summary>
        /// The email to send to.
        /// </summary>
        [EmailAddress]
        [Required]
        [StringLength(Constants.Dto.MaxLoginLength)]
        public required string Email { get; set; }
    }
}
