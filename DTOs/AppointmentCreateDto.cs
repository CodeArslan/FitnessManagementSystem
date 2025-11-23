namespace FMS.DTOs;
public record AppointmentCreateDto(string UserId, string TrainerId, DateTime SessionDate, string? SessionType);

