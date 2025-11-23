using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessManagementSystem.Models
{
    public class Plan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MemberId { get; set; }

        [ForeignKey("MemberId")]
        public ApplicationUser Member { get; set; }

        [Required]
        [StringLength(50)]
        public string PlanType { get; set; } 

        [Required]
        public string PlanDetails { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}