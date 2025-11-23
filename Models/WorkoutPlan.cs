using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMS.Models;

public class WorkoutPlan
{
    [Key]
    public int PlanId { get; set; }

    [Required]
    [ForeignKey("Trainer")]
    public string TrainerId { get; set; }
    public ApplicationUser Trainer { get; set; }

    [Required]
    [ForeignKey("Member")]
    public string MemberId { get; set; }
    public ApplicationUser Member { get; set; }

    [Required]
    public string PlanDetails { get; set; } // JSON or structured text

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
