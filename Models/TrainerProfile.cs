using System.ComponentModel.DataAnnotations;

namespace FitnessManagementSystem.Models;

public class TrainerProfile : BaseEntity
{
    [Key]
    public string TrainerId { get; set; } = null!; // PK = AspNetUsers.Id
    public double AverageRating { get; set; } = 0.0;
    public int RatingCount { get; set; } = 0;
    public DateTime? LastRatedAt { get; set; }
    // badge example: bronze,silver,gold
    public string? RatingBadge { get; set; }
}
