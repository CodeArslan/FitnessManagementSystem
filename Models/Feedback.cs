using FitnessManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace FitnessManagementSystem;

public class Feedback : BaseEntity
{
    [Key]
    public int FeedbackId { get; set; }
    [Required]
    public string MemberId { get; set; }
    [Required]
    public string TrainerId { get; set; }
    [Range(1, 5)]
    public int Rating { get; set; }
    [MaxLength(500)]
    public string? Comment { get; set; }
    public bool IsVisible { get; set; } = true;
    public bool IsApproved { get; set; } = false;
    public string? SessionId { get; set; }

}
