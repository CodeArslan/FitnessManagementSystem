using System.ComponentModel.DataAnnotations;

namespace FMS.Models;

public class MembershipPlan
{
    [Key]
    public int PlanId { get; set; }
    [Required, MaxLength(150)]
    public string PlanName { get; set; } = null!;
    public int DurationDays { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
