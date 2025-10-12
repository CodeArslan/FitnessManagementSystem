using Microsoft.AspNetCore.Identity;

namespace FitnessManagementSystem.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HeightFeet { get; set; }
        public string HeightInches { get; set; }
        public string Weight { get; set; }
        public string Role { get; set; }
    }
}
