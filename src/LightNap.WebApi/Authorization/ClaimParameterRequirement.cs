using Microsoft.AspNetCore.Authorization;

namespace LightNap.WebApi.Authorization
{
    /// <summary>
    /// Represents a custom authorization requirement for enforcing claims based on runtime parameters. This specific class is a
    /// market since the main work is done in the attribute and handler.
    /// </summary>
    public class ClaimParameterRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimParameterRequirement"/> class.
        /// </summary>
        public ClaimParameterRequirement()
        {
        }
    }
}
