using FitnessManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessManagementSystem;

public class MemberMembership : BaseEntity
{
    [Key]
    public int MembershipId { get; set; }

    // Foreign Key to AspNetUsers
    [Required]
    public string MemberId { get; set; }

    [ForeignKey(nameof(MemberId))]
    public ApplicationUser Member { get; set; }

    // Foreign Key to MembershipPlan
    [Required]
    public int PlanId { get; set; }

    [ForeignKey(nameof(PlanId))]
    public MembershipPlan Plan { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(50)]
    public string PaymentStatus { get; set; } = "Pending"; // e.g., Paid, Pending, Failed

}
