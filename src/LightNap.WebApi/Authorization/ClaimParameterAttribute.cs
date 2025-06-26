using Microsoft.AspNetCore.Authorization;

namespace LightNap.WebApi.Authorization
{
    /// <summary>
    /// Attribute for authorizing access based on a claim value that matches a parameter from a specified source.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ClaimParameterAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Gets or sets the key of the parameter to match against the claim value.
        /// </summary>
        public string ParameterKey { get; set; }

        /// <summary>
        /// Gets or sets the source from which the parameter value is obtained.
        /// </summary>
        public ClaimParameterSource Source { get; set; }

        /// <summary>
        /// Gets or sets the type of the claim to check.
        /// </summary>
        public string ClaimType { get; set; }

        /// <summary>
        /// Gets or sets a comma-separated list of roles that override the claim check.
        /// </summary>
        public string OverrideRoles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimParameterAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="claimType">The type of the claim to check.</param>
        /// <param name="parameterKey">The key of the parameter to match against the claim value. Defaults to "id".</param>
        /// <param name="source">The source from which the parameter value is obtained. Defaults to <see cref="ClaimParameterSource.Route"/>.</param>
        /// <param name="overrideRoles">A comma-separated list of roles that override the claim check. Defaults to empty string.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="claimType"/> or <paramref name="parameterKey"/> is null.</exception>
        public ClaimParameterAuthorizeAttribute(
            string claimType,
            string parameterKey = "id",
            ClaimParameterSource source = ClaimParameterSource.Route,
            string overrideRoles = "")
        {
            this.ParameterKey = parameterKey ?? throw new ArgumentNullException(nameof(parameterKey));
            this.Source = source;
            this.ClaimType = claimType ?? throw new ArgumentNullException(nameof(claimType));
            this.OverrideRoles = overrideRoles ?? string.Empty;
            this.Policy = nameof(ClaimParameterRequirement);
        }
    }
}