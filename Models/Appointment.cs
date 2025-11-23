using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMS.Models;

public class Appointment
{
    [Key]
    public int AppointmentId { get; set; }

    [Required, ForeignKey(nameof(User))]
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;

    [Required, ForeignKey(nameof(Trainer))]
    public string TrainerId { get; set; } = null!;
    public ApplicationUser Trainer { get; set; } = null!;

    public DateTime SessionDate { get; set; }
    [MaxLength(150)]
    public string? SessionType { get; set; }
    [MaxLength(50)]
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
