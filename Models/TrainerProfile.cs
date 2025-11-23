using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMS.Models;

public class TrainerProfile
{
    [Key]
    [ForeignKey("Trainer")]
    public string TrainerId { get; set; }
    public ApplicationUser Trainer { get; set; }

    public double AverageRating { get; set; }
    public int RatingCount { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
}
