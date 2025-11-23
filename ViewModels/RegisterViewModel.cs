using System.ComponentModel.DataAnnotations;

namespace FMS.ViewModels;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    // Member Profile Data
    [Required]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty; // We might need to add this to ApplicationUser or MemberProfile if not present. Assuming ApplicationUser doesn't have it, we might put it in MemberProfile or just rely on Email for now, but usually Name is good. 
    // Wait, ApplicationUser inherits IdentityUser which doesn't have FullName by default. 
    // Let's check MemberProfile again. It has Age, Height, etc.
    // Let's add basic profile fields here.

    [Required]
    public int Age { get; set; }

    [Required]
    [Display(Name = "Height (cm)")]
    public float HeightCm { get; set; }

    [Required]
    [Display(Name = "Weight (kg)")]
    public float WeightKg { get; set; }
}
