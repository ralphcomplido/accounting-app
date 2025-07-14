using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LightNap.Core.Data.Entities
{
    public class Account
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty; // Asset, Liability, Expense, etc.

        public string? Description { get; set; }

        [Precision(18, 4)] 
        public decimal Balance { get; set; } = 0;
    }
}
