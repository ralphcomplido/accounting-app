namespace LightNap.WebApi.Authorization
{
    /// <summary>
    /// Specifies the source from which a claim parameter value is obtained.
    /// </summary>
    public enum ClaimParameterSource
    {
        /// <summary>
        /// The claim parameter is obtained from the route values.
        /// </summary>
        Route,

        /// <summary>
        /// The claim parameter is obtained from the query string.
        /// </summary>
        Query,

        /// <summary>
        /// The claim parameter is a literal value.
        /// </summary>
        Literal,
    }
}