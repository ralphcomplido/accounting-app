namespace LightNap.Core.Accounts.Dto.Request
{
    /// <summary>
    /// Represents the data required to update an existing account.
    /// </summary>
    public class UpdateAccountDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the account to update.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the account.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the description of the account.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the balance of the account.
        /// </summary>
        public decimal? Balance { get; set; }
    }
}