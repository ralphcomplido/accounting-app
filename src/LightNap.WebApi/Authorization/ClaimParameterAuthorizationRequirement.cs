using Microsoft.AspNetCore.Authorization;

namespace LightNap.WebApi.Authorization
{
    public class ClaimParameterAuthorizationRequirement : IAuthorizationRequirement
    {
        public string ClaimType { get; }
        public string ParameterKey { get; }
        public ClaimParameterSource Source { get; }

        public ClaimParameterAuthorizationRequirement(string claimType, string parameterKey, ClaimParameterSource source)
        {
            this.ClaimType = claimType;
            this.ParameterKey = parameterKey;
            this.Source = source;
        }
    }
}
