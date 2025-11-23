using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMS.Models;

public class Feedback
{
    [Key]
    public int FeedbackId { get; set; }

    [Required]
    [ForeignKey("User")]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    [Required]
    [ForeignKey("Trainer")]
    public string TrainerId { get; set; }
    public ApplicationUser Trainer { get; set; }

    [ForeignKey("Appointment")]
    public int AppointmentId { get; set; }
    public Appointment Appointment { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    [MaxLength(500)]
    public string? Comment { get; set; }

    public bool IsApproved { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
