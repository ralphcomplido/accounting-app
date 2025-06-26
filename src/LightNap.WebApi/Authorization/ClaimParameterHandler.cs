using Microsoft.AspNetCore.Authorization;

namespace LightNap.WebApi.Authorization
{

    /// <summary>
    /// Authorization handler that validates user claims based on parameter values from the request.
    /// </summary>
    public class ClaimParameterHandler(IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<ClaimParameterRequirement>
    {
        /// <summary>
        /// Handles the authorization requirement by validating if the user has the required claims
        /// based on parameter values from the request.
        /// </summary>
        /// <param name="context">The authorization context containing the user and other information.</param>
        /// <param name="requirement">The claim parameter requirement to evaluate.</param>
        /// <returns>A completed task when the handling is finished.</returns>
        /// <remarks>
        /// This method evaluates <see cref="ClaimParameterAuthorizeAttribute"/> attributes on the endpoint.
        /// Access is granted if all attributes are satisfied or if the user is in any of the override roles.
        /// If no attributes are found or any attribute evaluation fails, authorization is denied.
        /// </remarks>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimParameterRequirement requirement)
        {
            if (httpContextAccessor.HttpContext == null) { return Task.CompletedTask; }

            var endpoint = httpContextAccessor.HttpContext.GetEndpoint();
            var attributes = endpoint?.Metadata.OfType<ClaimParameterAuthorizeAttribute>().ToList();

            if (attributes is null || !attributes.Any())
            {
                context.Fail();
                return Task.CompletedTask;
            }

            // NOTE: We loop through the attributes in case there are more than one because multiple claims are required for that endpoint.
            // However, this handler gets invoked once per attribute on that endpoint, so we're theoretically doing n^2 evaluations.
            // There doesn't appear to be a way to figure out which specific attribute is associated with the current invocation, so all
            // attributes are evaluated on all invocations. Fortunately, this isn't a common case and the comparisons are fast because
            // they're backed by the parsed token.
            foreach (var attribute in attributes)
            {
                if (!this.EvaluateAttribute(attribute, context, requirement))
                {
                    context.Fail();
                    return Task.CompletedTask;
                }
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Evaluates a single <see cref="ClaimParameterAuthorizeAttribute"/> against the current user and request context.
        /// </summary>
        /// <param name="attribute">The attribute to evaluate.</param>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The claim parameter requirement.</param>
        /// <returns>True if the requirement is satisfied; otherwise, false.</returns>
        private bool EvaluateAttribute(ClaimParameterAuthorizeAttribute attribute, AuthorizationHandlerContext context, ClaimParameterRequirement requirement)
        {
            if (!string.IsNullOrWhiteSpace(attribute.OverrideRoles))
            {
                foreach (var role in attribute.OverrideRoles.Split(','))
                {
                    if (context.User.IsInRole(role))
                    {
                        return true;
                    }
                }
            }

            string? parameter = attribute.Source switch
            {
                ClaimParameterSource.Route =>
                    httpContextAccessor.HttpContext!.Request.RouteValues.TryGetValue(attribute.ParameterKey, out var routeValue) && routeValue != null
                        ? routeValue.ToString()
                        : null,

                ClaimParameterSource.Query =>
                    httpContextAccessor.HttpContext!.Request.Query.TryGetValue(attribute.ParameterKey, out var queryValue)
                        ? queryValue.FirstOrDefault()
                        : null,

                ClaimParameterSource.Literal => attribute.ParameterKey,

                _ => throw new ArgumentOutOfRangeException(nameof(attribute.Source), attribute.Source, $"Unsupported ClaimParameterSource")
            };

            return !string.IsNullOrEmpty(parameter) && context.User.HasClaim(attribute.ClaimType, parameter);
        }
    }
}