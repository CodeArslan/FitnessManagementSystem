using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMS.Models;

public class ProgressRecord
{
    [Key]
    public int ProgressId { get; set; }

    [Required]
    [ForeignKey("User")]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public float Weight { get; set; }
    public float BMI { get; set; }
    public float BodyFatPercentage { get; set; }

    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
}
