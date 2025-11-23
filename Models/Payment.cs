using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMS.Models;

public class Payment
{
    [Key]
    public int PaymentId { get; set; }

    [Required]
    [ForeignKey("User")]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public decimal Amount { get; set; }

    [MaxLength(50)]
    public string PaymentMethod { get; set; }  // Card/Wallet/Bank

    public string? TransactionId { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Paid"; // Paid/Failed/Refunded

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
