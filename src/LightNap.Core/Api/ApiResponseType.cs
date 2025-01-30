namespace LightNap.Core.Api
{
    /// <summary>
    /// Represents the response types for an API.
    /// </summary>
    public enum ApiResponseType
    {
        /// <summary>
        /// The API call was successful.
        /// </summary>
        Success,

        /// <summary>
        /// An error occurred during the API call. User-friendly error messages are available in the <see cref="ApiResponseDto{T}.ErrorMessages"/> property.
        /// </summary>
        Error,

        /// <summary>
        /// An unexpected error occurred during the API call.
        /// </summary>
        UnexpectedError
    }
}