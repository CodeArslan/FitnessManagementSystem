using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMS.Models;

public class WorkoutSession
{
    [Key]
    public int WorkoutId { get; set; }

    [Required]
    [ForeignKey("User")]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public DateTime SessionDate { get; set; }

    public int DurationMinutes { get; set; }
    public int CaloriesBurned { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
