using Microsoft.AspNetCore.Identity;

namespace FitnessManagementSystem.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? HeightFeet { get; set; }
        public string? HeightInches { get; set; }
        public string? Weight { get; set; }
        public string? Specialization { get; set; }
        public string? Certification { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? JoinDate { get; set; }
        public string? FitnessGoal { get; set; }
        public string? Role { get; set; } = String.Empty;
    }
}
