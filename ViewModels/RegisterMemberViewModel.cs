using System.ComponentModel.DataAnnotations;

namespace FMS.ViewModels;

public class RegisterMemberViewModel
{
    [Required, EmailAddress] public string Email { get; set; } = null!;
    [Required, MinLength(6), DataType(DataType.Password)] public string Password { get; set; } = null!;
    [DataType(DataType.Password), Compare(nameof(Password))] public string ConfirmPassword { get; set; } = null!;
    [Phone] public string? PhoneNumber { get; set; }
    [Required] public string Role { get; set; } = "Member";
    // Member-only fields
    [Required]
    public int Age { get; set; }

    [Required]
    public float HeightCm { get; set; }

    [Required]
    public float WeightKg { get; set; }
}
