using System.ComponentModel.DataAnnotations;

namespace FitnessManagementSystem.Models
{
    public class Shift
    {
        [Key]
        public int ShiftId { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }
    }
}