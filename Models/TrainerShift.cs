using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessManagementSystem.Models
{
    public class TrainerShift
    {
        [Key]
        public int TrainerShiftId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        // Foreign key for Trainer
        public string TrainerId { get; set; }

        [ForeignKey("TrainerId")]
        public ApplicationUser? Trainer { get; set; }

        // Foreign key for Shift
        public int ShiftId { get; set; }

        [ForeignKey("ShiftId")]
        public Shift? Shift { get; set; }
    }
}