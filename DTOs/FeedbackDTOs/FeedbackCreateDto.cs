namespace FitnessManagementSystem.DTOs.FeedbackDTOs;

public record FeedbackCreateDto(string MemberId, string TrainerId, int Rating, string? Comments, string? SessionId);

