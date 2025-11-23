using FMS.Models;

namespace FMS.ViewModels;

public class MemberPlansViewModel
{
    public IEnumerable<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();
    public IEnumerable<DietPlan> DietPlans { get; set; } = new List<DietPlan>();
}
