using Microsoft.AspNetCore.Authorization;

namespace LightNap.WebApi.Authorization
{
    public class ClaimParameterAuthorizationHandler(IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<ClaimParameterAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimParameterAuthorizationRequirement requirement)
        {
            if (httpContextAccessor.HttpContext == null) { return Task.CompletedTask; }

            string? parameter = requirement.Source switch
            {
                ClaimParameterSource.Route =>
                    httpContextAccessor.HttpContext.Request.RouteValues.TryGetValue(requirement.ParameterKey, out var routeValue) && routeValue != null
                        ? routeValue.ToString()
                        : null,

                ClaimParameterSource.Query =>
                    httpContextAccessor.HttpContext.Request.Query.TryGetValue(requirement.ParameterKey, out var queryValue)
                        ? queryValue.FirstOrDefault()
                        : null,

                _ => throw new ArgumentOutOfRangeException(nameof(requirement.Source), requirement.Source, $"Unsupported ClaimParameterSource")
            };

            if (!string.IsNullOrEmpty(parameter) && context.User.HasClaim(requirement.ClaimType, parameter))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}