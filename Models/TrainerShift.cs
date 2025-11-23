using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMS.Models;

public class TrainerShift
{
    [Key]
    public int ShiftId { get; set; }

    [Required]
    [ForeignKey("Trainer")]
    public string TrainerId { get; set; }
    public ApplicationUser Trainer { get; set; }

    [Required]
    public DayOfWeek DayOfWeek { get; set; }

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    public TimeSpan EndTime { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
