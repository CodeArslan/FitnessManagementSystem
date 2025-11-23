using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessManagementSystem.Models
{
    public class TrainerAvailability
    {
        [Key]
        public int AvailabilityId { get; set; }

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
    }
}
