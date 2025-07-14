namespace LightNap.Core.Accounts.Dto.Request
{
    /// <summary>
    /// Represents the data required to create a new account.
    /// </summary>
    public class CreateAccountDto
    {
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
        /// Gets or sets the initial balance of the account.
        /// </summary>
        public decimal Balance { get; set; }
    }
}
