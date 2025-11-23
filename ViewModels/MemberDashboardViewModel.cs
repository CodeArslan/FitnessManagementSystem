using FMS.Models;

namespace FMS.ViewModels;

public class MemberDashboardViewModel
{
    public MemberProfile Profile { get; set; }
    public UserMembership? ActiveMembership { get; set; }
    public List<Appointment> UpcomingAppointments { get; set; } = new();
}
