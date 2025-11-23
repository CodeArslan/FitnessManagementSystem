namespace FMS.DTOs;


public record FeedbackCreateDto(string UserId, string TrainerId, int Rating, string? Comment, int? AppointmentId);


