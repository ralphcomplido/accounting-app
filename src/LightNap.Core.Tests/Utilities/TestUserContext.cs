using LightNap.Core.Interfaces;

namespace LightNap.Core.Tests.Utilities
{
    /// <summary>
    /// Test implementation of IUserContext for unit testing purposes.
    /// </summary>
    internal class TestUserContext : IUserContext
    {
        /// <summary>
        /// Gets or sets the IP address of the user.
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Retrieves the IP address of the user.
        /// </summary>
        /// <returns>The IP address of the user, or null if not set.</returns>
        public string? GetIpAddress()
        {
            return this.IpAddress;
        }

        /// <summary>
        /// Retrieves the user ID.
        /// </summary>
        /// <returns>The user ID.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the user ID is not set.</exception>
        public string GetUserId()
        {
            if (this.UserId is null) { throw new InvalidOperationException("GetUserId was called without having UserId set first"); }
            return this.UserId;
        }
    }
}
