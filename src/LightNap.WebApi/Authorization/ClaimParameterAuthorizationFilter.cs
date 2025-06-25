using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LightNap.WebApi.Authorization
{
    public class ClaimParameterAuthorizationFilter(string claimType, string parameterKey = "id", ClaimParameterSource source = ClaimParameterSource.Route) : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string? parameter = source switch
            {
                ClaimParameterSource.Route =>
                    context.HttpContext.Request.RouteValues.TryGetValue(parameterKey, out var routeValue) && routeValue != null
                        ? routeValue.ToString()
                        : null,

                ClaimParameterSource.Query =>
                    context.HttpContext.Request.Query.TryGetValue(parameterKey, out var queryValue)
                        ? queryValue.FirstOrDefault()
                        : null,

                ClaimParameterSource.Literal => parameterKey,

                _ => throw new ArgumentOutOfRangeException(nameof(source), source, $"Unsupported ClaimParameterSource")
            };

            if (string.IsNullOrEmpty(parameter) || !context.HttpContext.User.HasClaim(claimType, parameter))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}