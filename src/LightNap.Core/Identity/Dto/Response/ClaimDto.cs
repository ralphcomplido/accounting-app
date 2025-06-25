namespace LightNap.Core.Identity.Dto.Response
{
    /// <summary>
    /// Represents a claim.
    /// </summary>
    public class ClaimDto
    {
        /// <summary>
        /// Gets or sets the claim type.
        /// </summary>
        public required string Type { get; set; }

        /// <summary>
        /// Gets or sets the claim value.
        /// </summary>
        public required string Value { get; set; }
    }
}