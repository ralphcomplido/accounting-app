using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing.Template;

namespace LightNap.WebApi.Authorization
{

    /// <summary>
    /// Authorization handler that validates user claims based on parameter values from the request.
    /// </summary>
    public class ClaimAuthorizationHandler(IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<ClaimAuthorizationRequirement>
    {
        /// <summary>
        /// Handles the authorization requirement by validating if the user has the required claims
        /// based on parameter values from the request.
        /// </summary>
        /// <param name="context">The authorization context containing the user and other information.</param>
        /// <param name="requirement">The claim parameter requirement to evaluate.</param>
        /// <returns>A completed task when the handling is finished.</returns>
        /// <remarks>
        /// This method evaluates <see cref="ClaimAuthorizeAttribute"/> attributes on the endpoint.
        /// Access is granted if all attributes are satisfied or if the user is in any of the override roles.
        /// If no attributes are found or any attribute evaluation fails, authorization is denied.
        /// </remarks>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimAuthorizationRequirement requirement)
        {
            if (httpContextAccessor.HttpContext == null) { return Task.CompletedTask; }

            var endpoint = httpContextAccessor.HttpContext.GetEndpoint();
            var attributes = endpoint?.Metadata.OfType<ClaimAuthorizeAttribute>().ToList();

            if (attributes is null || attributes.Count == 0)
            {
#if DEBUG
                throw new InvalidOperationException($"No ClaimAuthorizeAttribute found for endpoint {endpoint?.DisplayName}.");
#else
                context.Fail();
                return Task.CompletedTask;
#endif
            }

            // NOTE: We loop through the attributes in case there are more than one because multiple claims are required for that endpoint.
            // However, this handler gets invoked once per attribute on that endpoint, so we're theoretically doing n^2 evaluations.
            // There doesn't appear to be a way to figure out which specific attribute is associated with the current invocation, so all
            // attributes are evaluated on all invocations. Fortunately, this isn't a common case and the comparisons are fast because
            // they're backed by the parsed token.
            foreach (var attribute in attributes)
            {
                if (!this.EvaluateAttribute(attribute, context))
                {
                    context.Fail();
                    return Task.CompletedTask;
                }
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Evaluates a single <see cref="ClaimAuthorizeAttribute"/> against the current user and request context.
        /// </summary>
        /// <param name="attribute">The attribute to evaluate.</param>
        /// <param name="context">The authorization context.</param>
        /// <returns>True if the requirement is satisfied; otherwise, false.</returns>
        private bool EvaluateAttribute(ClaimAuthorizeAttribute attribute, AuthorizationHandlerContext context)
        {
            var typeTemplate = TemplateParser.Parse(attribute.TypeTemplate) ?? throw new ArgumentNullException(nameof(attribute), "Claim type template cannot be null.");
            var valueTemplate = TemplateParser.Parse(attribute.ValueTemplate) ?? throw new ArgumentNullException(nameof(attribute), "Claim value template cannot be null.");

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

            string typeString = ClaimAuthorizationHandler.ResolveTemplate(typeTemplate, httpContextAccessor.HttpContext!);
            string valueString = ClaimAuthorizationHandler.ResolveTemplate(valueTemplate, httpContextAccessor.HttpContext!);

            return context.User.HasClaim(typeString, valueString);
        }

        /// <summary>
        /// Resolves a <see cref="RouteTemplate"/> into a string by replacing parameter segments
        /// with values from the current HTTP context's route values or query string.
        /// </summary>
        /// <param name="template">The route template to resolve.</param>
        /// <param name="context">The current HTTP context containing route and query data.</param>
        /// <returns>The resolved string with parameters replaced by their corresponding values.</returns>
        /// <exception cref="Exception">
        /// Thrown if a parameter in the template cannot be found in either the route values or query string.
        /// </exception>
        private static string ResolveTemplate(RouteTemplate template, HttpContext context)
        {
            var result = "";
            foreach (var segment in template.Segments)
            {
                foreach (var part in segment.Parts)
                {
                    if (part.IsLiteral)
                    {
                        result += part.Text;
                    }
                    else if (part.IsParameter)
                    {
                        if (context.Request.RouteValues.TryGetValue(part.Name!, out var routeVal) && routeVal != null)
                        {
                            result += routeVal.ToString();
                        }
                        else if (context.Request.Query.TryGetValue(part.Name!, out var queryVal))
                        {
                            result += queryVal.FirstOrDefault() ?? "";
                        }
                        else
                        {
                            throw new Exception($"Parameter '{part.Name}' not found in route values or query string.");
                        }
                    }
                }
            }
            return result;
        }
    }
}