namespace LightNap.Core.Accounts.Dto.Response
{
    /// <summary>
    /// Represents the data returned for an account.
    /// </summary>
    public class AccountResponseDto
    {
        /// <summary>
        /// Gets or sets the account ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the account.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the account.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the balance of the account.
        /// </summary>
        public decimal Balance { get; set; }
    }
}
