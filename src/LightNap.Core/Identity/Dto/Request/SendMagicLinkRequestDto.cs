using LightNap.Core.Configuration;
using System.ComponentModel.DataAnnotations;

namespace LightNap.Core.Identity.Dto.Request
{
    /// <summary>
    /// Data transfer object for requesting a magic link.
    /// </summary>
    public class SendMagicLinkRequestDto
    {
        /// <summary>
        /// The email address to send the magic link to.
        /// </summary>
        [EmailAddress]
        [Required]
        [StringLength(Constants.Dto.MaxLoginLength)]
        public required string Email { get; set; }
    }
}
