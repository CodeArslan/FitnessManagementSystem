using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMS.Models;

public class MemberProfile
{
    [Key, ForeignKey(nameof(User))]
    public string MemberId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;

    public int Age { get; set; }
    public float HeightCm { get; set; }
    public float WeightKg { get; set; }
    public float BMI { get; set; }
    public float BodyFatPercentage { get; set; }
    public float MuscleMass { get; set; }

    public double PerformanceScore { get; set; }
    public int PerformanceCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
