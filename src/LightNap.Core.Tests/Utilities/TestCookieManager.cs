using LightNap.Core.Interfaces;

namespace LightNap.Core.Tests.Utilities
{
    /// <summary>
    /// A test implementation of the ICookieManager interface for managing cookies in tests.
    /// </summary>
    internal class TestCookieManager : ICookieManager
    {
        private readonly Dictionary<string, (string Value, DateTime? Expires)> _cookies = [];

        /// <summary>
        /// Gets the value of a cookie by its key.
        /// </summary>
        /// <param name="key">The key of the cookie.</param>
        /// <returns>The value of the cookie if it exists and has not expired; otherwise, null.</returns>
        public string? GetCookie(string key)
        {
            if (this._cookies.TryGetValue(key, out var cookie))
            {
                if (cookie.Expires == null || cookie.Expires > DateTime.Now)
                {
                    return cookie.Value;
                }
                else
                {
                    this._cookies.Remove(key);
                }
            }
            return null;
        }

        /// <summary>
        /// Sets a cookie with the specified key, value, and expiration.
        /// </summary>
        /// <param name="key">The key of the cookie.</param>
        /// <param name="value">The value of the cookie.</param>
        /// <param name="isPersistent">Indicates whether the cookie is persistent.</param>
        /// <param name="expires">The expiration date and time of the cookie.</param>
        public void SetCookie(string key, string value, bool isPersistent, DateTime expires)
        {
            this._cookies[key] = (value, isPersistent ? expires : null);
        }

        /// <summary>
        /// Removes a cookie by its key.
        /// </summary>
        /// <param name="key">The key of the cookie to remove.</param>
        public void RemoveCookie(string key)
        {
            this._cookies.Remove(key);
        }
    }
}
