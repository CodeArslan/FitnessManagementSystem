using System.ComponentModel.DataAnnotations;

namespace FitnessManagementSystem.Models;

public class MemberProfile : BaseEntity
{
    [Key]
    public string MemberId { get; set; } = null!;
    public double PerformanceScore { get; set; } = 0.0;
    public int PerformanceCount { get; set; } = 0;
}
