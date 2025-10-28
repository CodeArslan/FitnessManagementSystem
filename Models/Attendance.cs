using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessManagementSystem.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }

        [Required]
        public string MemberId { get; set; }

        [ForeignKey("MemberId")]
        public ApplicationUser Member { get; set; }

        [Required]
        public string TrainerId { get; set; }

        [ForeignKey("TrainerId")]
        public ApplicationUser Trainer { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime AttendanceDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } // "Present" or "Absent"

        public DateTime MarkedAt { get; set; } = DateTime.Now;
    }
}