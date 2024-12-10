using LightNap.Core.Configuration;
using System.ComponentModel.DataAnnotations;

namespace LightNap.Core.Identity.Dto.Request
{
    /// <summary>
    /// Data transfer object for submitting an email verficiation code.
    /// </summary>
    public class VerifyEmailRequestDto
    {
        /// <summary>
        /// The email.
        /// </summary>
        [EmailAddress]
        [Required]
        [StringLength(Constants.Dto.MaxEmailLength)]
        public required string Email { get; set; }

        /// <summary>
        /// The verification code.
        /// </summary>
        [Required]
        [StringLength(Constants.Dto.MaxEmailVerificationCodeLength)]
        public required string Code { get; set; }
    }
}