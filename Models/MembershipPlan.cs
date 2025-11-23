using FitnessManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace FitnessManagementSystem;

public class MembershipPlan : BaseEntity
{
    [Key]
    public int PlanId { get; set; }

    [Required, MaxLength(100)]
    public string PlanName { get; set; }

    public int DurationMonths { get; set; }

    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    public bool IsActive { get; set; } = true;
}
