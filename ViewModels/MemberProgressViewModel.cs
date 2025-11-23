using FitnessManagementSystem.Models;

namespace FitnessManagementSystem.ViewModels;

public class MemberProgressViewModel
{
    public IEnumerable<ProgressRecord> ProgressRecords { get; set; } = new List<ProgressRecord>();
    public IEnumerable<WorkoutSession> WorkoutSessions { get; set; } = new List<WorkoutSession>();
}
