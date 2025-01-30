namespace LightNap.Core.Api
{
    /// <summary>
    /// Represents the response of an API call.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    public class ApiResponseDto<T>
    {
        /// <summary>
        /// Gets or sets the result of the API call.
        /// </summary>
        public T? Result { get; set; }

        /// <summary>
        /// Gets or sets the type of the API response.
        /// </summary>
        public ApiResponseType Type { get; set; }

        /// <summary>
        /// Gets or sets the error messages associated with the API response.
        /// </summary>
        public IEnumerable<string>? ErrorMessages { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseDto{T}"/> class.
        /// </summary>
        public ApiResponseDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseDto{T}"/> class set to Success.
        /// </summary>
        /// <param name="result">The result to return.</param>
        public ApiResponseDto(T? result)
        {
            this.Result = result;
            this.Type = ApiResponseType.Success;
        }
    }
}
