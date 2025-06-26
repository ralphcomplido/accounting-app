using Microsoft.AspNetCore.Authorization;

namespace LightNap.WebApi.Authorization
{
    /// <summary>
    /// Specifies that access to a controller or action method is restricted by a claim-based authorization policy.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ClaimAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// The template for the claim type, such as "resource:{resourceId}". Parameters are filled by path and then query.
        /// </summary>
        public string TypeTemplate { get; set; }

        /// <summary>
        /// The template for the claim value, such as "view:{resourceId}". Parameters are filled by path and then query.
        /// </summary>
        public string ValueTemplate { get; set; }

        /// <summary>
        /// The comma-separated list of roles that override the claim requirement. The user is granted access if they are in any of these roles.
        /// </summary>
        public string OverrideRoles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="typeTemplate">The claim type template for authorization.</param>
        /// <param name="valueTemplate">The claim value template for authorization.</param>
        /// <param name="overrideRoles">A comma-separated list of roles that can override the claim requirement. Optional.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="typeTemplate"/> or <paramref name="valueTemplate"/> is null.</exception>
        public ClaimAuthorizeAttribute(string typeTemplate, string valueTemplate, string overrideRoles = "")
        {
            this.TypeTemplate = typeTemplate ?? throw new ArgumentNullException(nameof(typeTemplate));
            this.ValueTemplate = valueTemplate ?? throw new ArgumentNullException(nameof(valueTemplate));
            this.OverrideRoles = overrideRoles ?? string.Empty;
            this.Policy = nameof(ClaimAuthorizationRequirement);
        }
    }
}